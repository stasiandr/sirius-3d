using UnityEngine;

namespace VRController
{
    public class VRController : MonoBehaviour
    {
        public GameObject camera;
        public const float WalkSpeed = 2.5f;
        public const float FlySpeed = 2.5f;
        public const float RotationAngle = 45f;
        private void Walk(Vector2 input)
        {
            
            var movement = camera.transform.TransformDirection(input.x, 0, input.y);
            movement = movement.magnitude == 0 ? Vector3.zero : movement / movement.magnitude;
            
            float speedProduct = Time.deltaTime * WalkSpeed;
            movement *= speedProduct;
            this.transform.localPosition += movement;
        }
    
        private void Rotate(Vector2 input)
        {
            transform.Rotate(Vector3.up, input.x * RotationAngle * Time.deltaTime);
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
            var inputRightThumbstick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
            
            Walk(inputLeftThumbstick);
            Rotate(inputRightThumbstick);
            Fly();
        }
    }
}
