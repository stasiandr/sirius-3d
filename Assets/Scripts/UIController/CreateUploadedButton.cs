using System;
using Commands;
using SceneProvider;
using UnityEngine;

namespace UIController
{
    public class CreateUploadedButton : MonoBehaviour
    {
        public string Name;

        public void CreateUploadedByName()
        {
            SceneData.ExecutionQueue.Enqueue(new CreatePrimitiveCommand(Name));
        }
    }
}
