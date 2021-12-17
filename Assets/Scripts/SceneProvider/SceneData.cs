using System.Collections.Generic;
using CameraClickController;
using Commands;
using MeshTools;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace SceneProvider
{
    public class SceneData : MonoBehaviour
    {
        public static Queue<ICommand> ExecutionQueue = new Queue<ICommand>();

        public static class Selection
        {
            public static GameObject Target;
        }

        public void OnEnable()
        {
            CameraSelectController.ObjectsSelected += CameraSelectControllerOnObjectsSelected;
        }

        private void CameraSelectControllerOnObjectsSelected(List<Collider> obj)
        {
            Debug.Log(obj[0]);
        }

        private static SceneData _instance;

        [SerializeField]
        private Material defaultMaterial;

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