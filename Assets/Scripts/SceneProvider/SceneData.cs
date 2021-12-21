using System.Collections.Generic;
using System.Linq;
using CameraClickController;
using Commands;
using MeshTools;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SceneProvider
{
    public class SceneData : MonoBehaviour
    {
        public static Queue<ICommand> ExecutionQueue = new Queue<ICommand>();
        public static List<ICommand> ExecutedCommands = new List<ICommand>();
        public static List<GameObject> Targets = new List<GameObject>();
        static int NewObjID = 0;
        public static Dictionary<int, GameObject> ObjectsByID = new Dictionary<int, GameObject>();

        public void OnEnable()
        {
            CameraSelectController.ObjectsSelected += CameraSelectControllerOnObjectsSelected;
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
        }

        private static SceneData _instance;

        public Material defaultMaterial;
        public Material selectedMaterial;

        private void Start()
        {
            _instance = this;
            Targets = new List<GameObject>();
            NewObjID = 0;
            ObjectsByID = new Dictionary<int, GameObject>();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Backspace))
            {
                ExecutionQueue.Enqueue(new DeleteCommand(Targets));
            }
            if (ExecutionQueue.Count > 0)
            {
                var command = ExecutionQueue.Dequeue();
                command.Apply();
                ExecutedCommands.Add(command);
            }
            
            if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.Z) && ExecutedCommands.Count > 0)
            {
                Undo();
            }
            
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

            var boxCollider = go.AddComponent<BoxCollider>();
            boxCollider.center = meshFilter.mesh.bounds.center;
            boxCollider.size = meshFilter.mesh.bounds.size;

            go.AddComponent<MeshRenderer>().sharedMaterial = _instance.defaultMaterial;
            ObjectsByID[NewObjID] = go;
            NewObjID++;
            return NewObjID - 1;
        }
    }
}