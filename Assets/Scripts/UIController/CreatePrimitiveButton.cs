using System;
using Commands;
using SceneProvider;
using UnityEngine;

namespace UIController
{
    public class CreatePrimitiveButton : MonoBehaviour
    {

        public void CreatePrimitiveByID(int id)
        {
            switch(id)
            {
                case 0:
                    SceneData.ExecutionQueue.Enqueue(new CreatePrimitiveCommand("Cube"));
                    break;
                case 1:
                    SceneData.ExecutionQueue.Enqueue(new CreatePrimitiveCommand("Sphere"));
                    break;
                case 2:
                    SceneData.ExecutionQueue.Enqueue(new CreatePrimitiveCommand("Cone"));
                    break;
                case 3:
                    SceneData.ExecutionQueue.Enqueue(new CreatePrimitiveCommand("Cylinder"));
                    break;
                case 4:
                    SceneData.ExecutionQueue.Enqueue(new CreatePrimitiveCommand("Torus"));
                    break;
                default:
                    throw new NotImplementedException();
            }

        }
        
        public void CreateCube()
        {
            
        }
    }
}
