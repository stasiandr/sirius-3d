using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CameraClickController
{
    public class CameraSelectController : MonoBehaviour
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
            if (Input.GetMouseButtonDown(0))
            {
                RayCast();
            }
        }

        private void RayCast()
        {
            Debug.Assert(Camera.main != null, "Camera.main != null");
            
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hits = Physics.RaycastAll(ray, 1000, ~LayerMask.NameToLayer(handlesLayerName));

            if (hits.Length == 0)
            {
                _selected.Clear();
                ObjectsSelected?.Invoke(null);
                return;
            }

            var target = hits
                .OrderBy(h => Vector3.Distance(Camera.main.transform.position, h.point))
                .First().collider;

            if (!(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
                _selected.Clear(); 
            
            _selected.Add(target);
            
            ObjectsSelected?.Invoke(_selected);
            
        }
    }
}