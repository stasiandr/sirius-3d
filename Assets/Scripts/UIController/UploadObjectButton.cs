using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneProvider;
using Commands;

namespace UIController
{
    public class UploadObjectButton : MonoBehaviour
    {
        public void UploadObject()
        {
            SceneData.ExecutionQueue.Enqueue(new UploadObjectCommand());
        }
    }
}
