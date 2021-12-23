using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRController;

namespace VRCameraClickController
{
    public class VRCameraSelectControllerRight : MonoBehaviour
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
            if (VRController.VRController.CurrentDragger == null && OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
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
