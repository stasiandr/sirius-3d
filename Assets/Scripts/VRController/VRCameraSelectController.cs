using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VRCameraClickController
{
    public class VRCameraSelectController : MonoBehaviour
    {
        public static event Action<List<Collider>> ObjectsSelected;
        private List<Collider> _selected;
        public string handlesLayerName = "Handles";

        private void OnEnable()
        {
            _selected = new List<Collider>();
        }

        private void Update()
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                SphereCast();
            }
        }

        private void SphereCast()
        {
            RaycastHit[] sphere = Physics.SphereCastAll
            (
                this.transform.position,
                0.05f,
                Vector3.forward,
                0.0001f
            );

            _selected.Clear();

            if (sphere.Length == 0)
            {
                ObjectsSelected?.Invoke(null);
                return;
            }
            
            _selected.Add(sphere.First().collider);
            ObjectsSelected?.Invoke(_selected);
        }
    }
}
