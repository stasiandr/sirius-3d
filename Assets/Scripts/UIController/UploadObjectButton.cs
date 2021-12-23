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
        public GameObject Button;
        public Transform scrollview_transform;
        public void UploadObject(string name, string path = "")
        {
            var com = new UploadObjectCommand(name, path);
            SceneData.RequestQueue.Enqueue(com);
            com.Button = Button;
            com.scrollview_transform = scrollview_transform;
        }
    }
}
