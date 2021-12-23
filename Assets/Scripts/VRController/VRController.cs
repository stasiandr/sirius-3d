using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SceneProvider;

namespace VRController
{
    public class VRController : MonoBehaviour
    {
        private OVRCameraRig rig;
        private Dictionary<GameObject, Vector3> startPositions = new Dictionary<GameObject, Vector3>();
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
            if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
            {
                Grab(targets, leftControllerObject);
            }

            if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger))
            {
                Ungrab(targets);
                startPositions = new Dictionary<GameObject, Vector3>();
            }
        }

        public Material mat;
        
        private void Grab(List<GameObject> targets, GameObject parent)
        {
            foreach (var obj in targets)
            {
                if (Vector3.Distance(leftControllerObject.transform.position, obj.transform.position) < 0.1f)
                {
                    startPositions.Add(obj, obj.transform.position);
                    obj.transform.parent = parent.transform;
                }
            }
        }

        private void Ungrab(List<GameObject> targets)
        {
            foreach (var obj in targets.Where(obj => obj.transform.parent != null))
            {
                List<GameObject> temp = new List<GameObject> {obj};
                obj.transform.parent = null;
                
                SceneData.ExecutionQueue.Enqueue
                    (
                    new Commands.TransformCommand
                        (
                            temp,
                            obj.transform.position - startPositions[obj]
                        )
                    );

                obj.transform.position = startPositions[obj];
            }
        }
        
        public void Update()
        {
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
