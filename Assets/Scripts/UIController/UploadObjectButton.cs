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
        public void UploadObject(string name)
        {
            SceneData.ExecutionQueue.Enqueue(new UploadObjectCommand(name));
            var new_button = Instantiate(Button);
            new_button.transform.parent = scrollview_transform;
            new_button.GetComponent<CreateUploadedButton>().Name = name;
            new_button.transform.GetChild(0).GetComponent<Text>().text = "Create " + name; 
        }
    }
}
