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
            var com = new CreatePrimitiveCommand(Name);
            com.MaterialID = SceneData.CurrentMaterial;
            SceneData.RequestQueue.Enqueue(com);
        }
    }
}
