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
            throw new NotImplementedException();
        }
        
        public void CreateCube()
        {
            SceneData.ExecutionQueue.Enqueue(new CreateCubeCommand());
        }

        public void CreateSphere()
        {
            throw new NotImplementedException();
        }
    }
}
