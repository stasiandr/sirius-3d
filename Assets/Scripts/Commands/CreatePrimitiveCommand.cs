using System;
using MeshTools;
using SceneProvider;

namespace Commands
{
    public struct CreatePrimitiveCommand : ICommand
    {
        public string MeshType;

        public CreatePrimitiveCommand(string meshType)
        {
            MeshType = meshType;
        }

        public void Apply()
        {
            MyMesh mesh;
            switch (MeshType) {
                case "Cube":
                    mesh = MeshGenerator.GenerateCube();
                    break;
                case "Sphere":
                    throw new NotImplementedException();
                    //mesh = MeshGenerator.GenerateSphere();
                    //break;
                case "Cone":
                    throw new NotImplementedException();
                    //mesh = MeshGenerator.GenerateCone();
                    //break;
                case "Cylinder":
                    throw new NotImplementedException();
                    //mesh = MeshGenerator.GenerateCylinder();
                    //break;
                case "Torus":
                    throw new NotImplementedException();
                    //mesh = MeshGenerator.GenerateTprus();
                    //break;
                default:
                    throw new NotImplementedException();
            }
            SceneData.CreateMesh(mesh);
        }

        public void Revert()
        {
            throw new NotImplementedException();
        }
    }
}