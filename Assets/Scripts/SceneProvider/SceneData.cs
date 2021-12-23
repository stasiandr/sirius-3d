using System;
using System.Collections.Generic;
using System.Linq;
using CameraClickController;
using Commands;
using MeshTools;
using UnityEngine;
using UnityEngine.EventSystems;
using VRCameraClickController;

namespace SceneProvider
{
    public class SceneData : MonoBehaviour
    {
        public static Queue<ICommand> ExecutionQueue = new Queue<ICommand>();
        public static Queue<ICommand> RequestQueue = new Queue<ICommand>();
        public static List<ICommand> ExecutedCommands = new List<ICommand>();
        public static List<GameObject> Targets = new List<GameObject>();
        public static event Action<List<GameObject>> ObjectsSelected;
        public static Dictionary<string, MyMesh> UploadedMeshes = new Dictionary<string, MyMesh>();
        public static bool HasStarted, SinglePlayer;
        
        static int NewObjID = 0;
        public static Dictionary<int, GameObject> ObjectsByID = new Dictionary<int, GameObject>();

        public static GameObject create_button_prefab;
        public static Transform buttons_scrollview_transform;
        public GameObject _create_button_prefab;
        public Transform _buttons_scrollview_transform;
        public void OnEnable()
        {
            // CameraSelectController.ObjectsSelected += CameraSelectControllerOnObjectsSelected;
            VRCameraSelectControllerLeft.ObjectsSelected += CameraSelectControllerOnObjectsSelected;
            VRCameraSelectControllerRight.ObjectsSelected += CameraSelectControllerOnObjectsSelected;
            // create_button_prefab = _create_button_prefab;
            // buttons_scrollview_transform = _buttons_scrollview_transform;
        }

        private bool OverUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        private void CameraSelectControllerOnObjectsSelected(List<Collider> obj)
        {
            if (OverUI())
            {
                return;
            }

            foreach (var target in Targets.Where(target => target != null))
            {
                target.GetComponent<MeshRenderer>().sharedMaterial = defaultMaterial;
            }
            Targets = new List<GameObject>();
            if (obj == null)
            {
                ObjectsSelected?.Invoke(Targets);
                return;
            }
            foreach (var col in obj.Where(col => col != null))
            {
                Targets.Add(col.gameObject);
            }

            foreach (var target in Targets)
            {
                target.GetComponent<MeshRenderer>().sharedMaterial = selectedMaterial;
            }

            ObjectsSelected?.Invoke(Targets);
        }

        public static SceneData _instance;

        public Material defaultMaterial;
        public Material selectedMaterial;

        private void Start()
        {
            ExecutionQueue = new Queue<ICommand>();
            RequestQueue = new Queue<ICommand>();
            ExecutedCommands = new List<ICommand>();
            _instance = this;
            Targets = new List<GameObject>();
            NewObjID = 0;
            ObjectsByID = new Dictionary<int, GameObject>();
            HasStarted = false;
            SinglePlayer = false;
            ClientProcessing.client = new Core.Scripts.Networking.Client();
        }

        public void Update()
        {
            if (!HasStarted)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                RequestQueue.Enqueue(new DeleteCommand(Targets));
            }

            if (RequestQueue.Count > 0)
            {
                var command = RequestQueue.Dequeue();
                if (SinglePlayer)
                {
                    ExecutionQueue.Enqueue(command);
                }
                else
                {
                    ClientProcessing.client.SendRequest(command.Serialize());
                }
            }

            if (ExecutionQueue.Count > 0)
            {
                var command = ExecutionQueue.Dequeue();
                command.Apply();
                ExecutedCommands.Add(command);
            }

            //if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.Z) && ExecutedCommands.Count > 0)
            //{
            //   Undo();
            //}
            
        }

        public static void Undo()
        {
            var command = ExecutedCommands.Last();
            command.Revert();
            ExecutedCommands.RemoveAt(ExecutedCommands.Count - 1);
        }

        public static int CreateMesh(MyMesh mesh)
        {
            var go = new GameObject
            {
                name = "" + NewObjID,
                layer = LayerMask.NameToLayer("Objects")
            };

            var meshFilter = go.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh.ToUnityMesh();

            var meshCollider = go.AddComponent<MeshCollider>();

            go.AddComponent<MeshRenderer>().sharedMaterial = _instance.defaultMaterial;
            ObjectsByID[NewObjID] = go;
            NewObjID++;
            return NewObjID - 1;
        }
    }
}