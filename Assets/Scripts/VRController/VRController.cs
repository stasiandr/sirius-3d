using UnityEngine;

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
        public void Update()
        {
            var inputLeftThumbstick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

            Move(inputLeftThumbstick);
            Fly();
        }
    }
}
