using System;
using Commands;
using SceneProvider;
using UnityEngine;

namespace UIController
{
    public class CreateUploadedButton : MonoBehaviour
    {
        public void CreateUploadedByName(string name)
        {
            SceneData.ExecutionQueue.Enqueue(new CreatePrimitiveCommand(name));
        }
    }
}
