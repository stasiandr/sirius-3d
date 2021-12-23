using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneProvider;
using Commands;
using UnityEngine.UI;

namespace UIController
{
    public class UploadObjectButton : MonoBehaviour
    {
        public void UploadObject(string name, string path = "")
        {
            var com = new UploadObjectCommand(name, path);
            SceneData.RequestQueue.Enqueue(com);
        }
    }
}
