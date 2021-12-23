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
                    SceneData.RequestQueue.Enqueue(new CreatePrimitiveCommand("Cube"));
                    break;
                case 1:
                    SceneData.RequestQueue.Enqueue(new CreatePrimitiveCommand("Sphere"));
                    break;
                case 2:
                    SceneData.RequestQueue.Enqueue(new CreatePrimitiveCommand("Cone"));
                    break;
                case 3:
                    SceneData.RequestQueue.Enqueue(new CreatePrimitiveCommand("Cylinder"));
                    break;
                case 4:
                    SceneData.RequestQueue.Enqueue(new CreatePrimitiveCommand("Torus"));
                    break;
                case 5:
                    SceneData.RequestQueue.Enqueue(new CreatePrimitiveCommand("Plane"));
                    break;
                default:
                    throw new NotImplementedException();
            }

        }
    }
}
