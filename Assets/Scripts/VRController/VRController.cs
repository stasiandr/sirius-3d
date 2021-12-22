using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SceneProvider;

namespace VRController
{
    public class VRController : MonoBehaviour
    {
        private OVRCameraRig rig;
        private List<Vector3> startPositions = new List<Vector3>();
        private List<GameObject> Target;

        public GameObject camera;
        public GameObject leftControllerObject;
        public GameObject rightControllerObject;

        public const float WalkSpeed = 2.5f;
        public const float FlySpeed = 2.5f;
        public const float RotationAngle = 45f;

        public void Start()
        {
            rig = GetComponent<OVRCameraRig>();
            SceneData.ObjectsSelected += GetTargets;
        }

        public void GetTargets(List<GameObject> targets)
        {
            Target = new List<GameObject>();

            foreach (var obj in targets.Where(obj => obj != null))
            {
                Target.Add(obj);
            }
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
            float changeUp = ConvertGoUp(OVRInput.Get(OVRInput.Button.Four));
            float changeDown = ConvertGoDown(OVRInput.Get(OVRInput.Button.Three));
            var movement = camera.transform.TransformDirection(0, changeUp + changeDown, 0);
            movement = movement.magnitude == 0 ? Vector3.zero : movement / movement.magnitude;
    
            float speedProduct = Time.deltaTime * FlySpeed;
            movement *= speedProduct;
            this.transform.Translate(movement);
        }

        public bool kek;
        
        private void Drag(List<GameObject> targets)
        {
            var grabbedTargets = new List<GameObject>();
            
            if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
            {
                grabbedTargets = Grab(targets, leftControllerObject);
            }

            var inputForRotation = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
            Vector3 rotationVector = new Vector3(inputForRotation.y, inputForRotation.x, 0);

            if (grabbedTargets.Count != 0)
            {
                foreach (var target in grabbedTargets)
                {
                    target.transform.Rotate(rotationVector * RotationAngle * Time.deltaTime);
                }
            }

            if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger))
            {
                var endPositions = Ungrab(targets);

                for (int id = 0; id < endPositions.Count; ++id)
                {
                    targets[id].transform.position = startPositions[id];
                    var target = new List<GameObject> {targets[id]};
                    
                    SceneData.ExecutionQueue.Enqueue
                    (
                        new Commands.TransformCommand
                        (
                            target,
                            endPositions[id] - startPositions[id]
                        )
                    );
                }

                startPositions = new List<Vector3>();
            }
        }

        public Material mat;
        
        private List<GameObject> Grab(List<GameObject> targets, GameObject parent)
        {
            List<GameObject> grabbed = new List<GameObject>();
            
            foreach (var obj in targets)
            {
                if (Vector3.Distance(leftControllerObject.transform.position, obj.transform.position) < 0.1f)
                {
                    startPositions.Add(obj.transform.position);
                    obj.transform.parent = parent.transform;
                    grabbed.Add(obj);
                }
            }

            return grabbed;
        }

        private List<Vector3> Ungrab(List<GameObject> targets)
        {
            List<Vector3> endPositions = new List<Vector3>();

            foreach (var obj in targets.Where(obj => obj.transform.parent != null))
            {
                obj.transform.parent = null;
                endPositions.Add(obj.transform.position);
            }
            
            return endPositions;
        }
        
        public void Update()
        {
            mat.color = Color.magenta;
            var inputLeftThumbstick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

            Move(inputLeftThumbstick);
            Fly();
            
            if (Target.Count != 0) 
            {
                Drag(Target);
            }
        }
    }
}
