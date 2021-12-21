using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SceneProvider;

namespace VRController
{
    public class VRController : MonoBehaviour
    {
        private OVRCameraRig rig;

        public GameObject camera; 
        public const float WalkSpeed = 2.5f;
        public const float FlySpeed = 2.5f;

        public void Start()
        {
            rig = GetComponent<OVRCameraRig>();
            SceneData.ObjectsSelected += Drag;
        }

        private void Move(Vector2 input)
        {
            Quaternion headYaw = Quaternion.Euler(0, rig.centerEyeAnchor.transform.eulerAngles.y, 0);
            Vector3 direction = headYaw * new Vector3(input.x, 0, input.y);
            camera.transform.Translate(direction * WalkSpeed * Time.deltaTime);
        }
        
        private static float ConvertGoUp(bool isGoingUp)
        {
            return isGoingUp ? 1f : 0f;
        }
    
        private static float ConvertGoDown(bool isGoingDown)
        {
            return isGoingDown ? -1f : 0f;
        }
        
        private void Fly()
        {
            float changeUp = ConvertGoUp(OVRInput.Get(OVRInput.Button.One));
            float changeDown = ConvertGoDown(OVRInput.Get(OVRInput.Button.Two));
            var movement = camera.transform.TransformDirection(0, changeUp + changeDown, 0);
            movement = movement.magnitude == 0 ? Vector3.zero : movement / movement.magnitude;
    
            float speedProduct = Time.deltaTime * FlySpeed;
            movement *= speedProduct;
            this.transform.Translate(movement);
        }

        private void Drag(List<GameObject> targets)
        {
            Vector3 startPos = Vector3.zero, endPos;
            
            if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
            {
                startPos = Grab(targets);
            }

            if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger))
            {
                Debug.Assert(startPos != Vector3.zero, "startPos != Vector3.zero");
                endPos = Ungrab(targets);
                targets.First().transform.position = startPos;
                SceneData.ExecutionQueue.Enqueue
                    (
                        new Commands.TransformCommand
                            (
                                targets,
                                endPos - startPos
                            )
                    );
            }
        }
        
        private Vector3 Grab(List<GameObject> targets)
        {
            var startPos = targets.First().transform;
            var parent = rig.leftHandAnchor.GetChild(1);
            targets.First().transform.SetParent(parent);
            return startPos.position;
        }

        private Vector3 Ungrab(List<GameObject> targets)
        {
            var endPos = targets.First().transform;
            targets.First().transform.SetParent(null);
            return endPos.position;
        }
        
        public void Update()
        {
            var inputLeftThumbstick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

            Move(inputLeftThumbstick);
            Fly();
        }
    }
}
