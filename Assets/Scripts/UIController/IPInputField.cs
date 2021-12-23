using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIController
{
    public class IPInputField : MonoBehaviour
    {
        public InputField input;
        public void Activate(string s)
        {
            Debug.Log("HEH");
            StartCoroutine(ClientProcessing.Activate(s));
            Destroy(gameObject);
        }
    }
}
