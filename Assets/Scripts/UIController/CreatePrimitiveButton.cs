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
            CreatePrimitiveCommand com;
            switch(id)
            {
                case 0:
                    com = new CreatePrimitiveCommand("Cube");
                    break;
                case 1:
                    com = new CreatePrimitiveCommand("Sphere");
                    break;
                case 2:
                    com = new CreatePrimitiveCommand("Cone");
                    break;
                case 3:
                    com = new CreatePrimitiveCommand("Cylinder");
                    break;
                case 4:
                    com = new CreatePrimitiveCommand("Torus");
                    break;
                case 5:
                    com = new CreatePrimitiveCommand("Plane");
                    break;
                default:
                    throw new NotImplementedException();
            }
            com.MaterialID = SceneData.CurrentMaterial;
            SceneData.RequestQueue.Enqueue(com);
        }
    }
}
