using System.Collections.Generic;
using System.Linq;
using CameraClickController;
using Commands;
using MeshTools;
using UnityEngine;

namespace SceneProvider
{
    public class SceneData : MonoBehaviour
    {
        public static Queue<ICommand> ExecutionQueue = new Queue<ICommand>();
        public static List<GameObject> Targets = new List<GameObject>();

        public void OnEnable()
        {
            CameraSelectController.ObjectsSelected += CameraSelectControllerOnObjectsSelected;
        }

        private void CameraSelectControllerOnObjectsSelected(List<Collider> obj)
        {
            foreach (var target in Targets.Where(target => target != null))
            {
                target.GetComponent<MeshRenderer>().sharedMaterial = defaultMaterial;
            }   
            
            Targets = new List<GameObject>();

            if (obj == null)
            {
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
            
            Debug.Log(Targets);
        }

        private static SceneData _instance;

        public Material defaultMaterial;
        public Material selectedMaterial;

        private void Start()
        {
            _instance = this;
        }

        public void Update()
        {
            if (ExecutionQueue.Count > 0)
            {
                var command = ExecutionQueue.Dequeue();
                command.Apply();
            }
        }
        public static void CreateMesh(MyMesh mesh)
        {
            var go = new GameObject
            {
                name = "Cube",
                layer = LayerMask.NameToLayer("Handles")
            };

            var meshFilter = go.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh.ToUnityMesh();

            var boxCollider = go.AddComponent<BoxCollider>();
            boxCollider.center = meshFilter.mesh.bounds.center;
            boxCollider.size = meshFilter.mesh.bounds.size;

            go.AddComponent<MeshRenderer>().sharedMaterial = _instance.defaultMaterial;
        }
    }
}