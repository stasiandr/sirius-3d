using System;
using System.Collections.Generic;
using UIController;
using UnityEngine;

namespace SceneProvider
{
    public class SceneData : MonoBehaviour
    {
        public static Queue<ICommand> ExecutionQueue = new Queue<ICommand>();

        public void Start()
        {
            
        }

        public void Update()
        {
            while (ExecutionQueue.Count > 0)
            {
                var command = ExecutionQueue.Dequeue();
                command.Apply();
            }
        }

        public static void CreateMesh(MyMesh.MyMesh mesh)
        {
            var go = new GameObject();
            go.name = "Cube";

            var meshfilter = go.AddComponent<MeshFilter>();
            meshfilter.mesh = mesh.ToUnityMesh();
            go.AddComponent<MeshRenderer>();
        }
    }
}
