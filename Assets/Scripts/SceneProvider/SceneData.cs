using System;
using System.Collections.Generic;
using Commands;
using MeshTools;
using UIController;
using UnityEngine;

namespace SceneProvider
{
    public class SceneData : MonoBehaviour
    {
        public static Queue<ICommand> ExecutionQueue = new Queue<ICommand>();
        
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
                name = "Cube"
            };

            go.AddComponent<MeshFilter>().mesh = mesh.ToUnityMesh();
            
            go.AddComponent<MeshRenderer>();
        }
    }
}
