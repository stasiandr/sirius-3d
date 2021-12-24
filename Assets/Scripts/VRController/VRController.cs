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
        private Dictionary<GameObject, Quaternion> startRotations = new Dictionary<GameObject, Quaternion>();
        private List<GameObject> Target;
        
        public static GameObject CurrentDragger = null;
        public GameObject camera;
        public GameObject leftControllerObject;
        public GameObject rightControllerObject;

        public const float WalkSpeed = 2.5f;
        public const float FlySpeed = 2.5f;

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
            float changeUp = ConvertGoUp(OVRInput.Get(OVRInput.Button.Two));
            float changeDown = ConvertGoDown(OVRInput.Get(OVRInput.Button.One));
            var movement = camera.transform.TransformDirection(0, changeUp + changeDown, 0);
            movement = movement.magnitude == 0 ? Vector3.zero : movement / movement.magnitude;
    
            float speedProduct = Time.deltaTime * FlySpeed;
            movement *= speedProduct;
            this.transform.Translate(movement);
        }

        public bool kek;
        
        private void Drag(List<GameObject> targets)
        {
            if (CurrentDragger == null)
            {
                if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
                {
                    startPositions = new Dictionary<GameObject, Vector3>();
                    startRotations = new Dictionary<GameObject, Quaternion>();
                    Grab(targets, leftControllerObject);
                }
                else if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
                {
                    startPositions = new Dictionary<GameObject, Vector3>();
                    startRotations = new Dictionary<GameObject, Quaternion>();
                    Grab(targets, rightControllerObject);
                }
            }

            if (CurrentDragger != null)
            {
                if (CurrentDragger == leftControllerObject && OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger))
                {
                    Ungrab(targets);
                    CurrentDragger = null;
                }
                else if (CurrentDragger == rightControllerObject && OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger))
                {
                    Ungrab(targets);
                    CurrentDragger = null;
                }
            }
        }

        public Material mat;
        
        private void Grab(List<GameObject> targets, GameObject parent)
        {
            CurrentDragger = parent;
            
            foreach (var obj in targets)
            {
                if (Vector3.Distance(parent.transform.position, obj.transform.position) < 1f)
                {
                    startPositions.Add(obj, obj.transform.position);
                    startRotations.Add(obj, obj.transform.rotation);
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

                var tempPosition = obj.transform.position;
                var tempRotation = obj.transform.rotation;

                SceneData.RequestQueue.Enqueue
                    (
                    new Commands.TransformCommand
                        (
                            temp,
                            tempPosition - startPositions[obj]
                        )
                    );

                SceneData.RequestQueue.Enqueue
                    (
                        new Commands.RotateCommand
                            (
                                temp,
                                tempRotation.eulerAngles - startRotations[obj].eulerAngles
                            )    
                    );
                
                obj.transform.position = startPositions[obj];
                obj.transform.rotation = startRotations[obj];
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
