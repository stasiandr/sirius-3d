using System;
using MeshTools;
using SceneProvider;
using UnityEngine;

namespace Commands
{
    public struct CreatePrimitiveCommand : ICommand
    {
        public string MeshType;
        public Vector3 Pos;
        public Vector3 Scale;
        public int Details;
        public int Details2;
        public CreatePrimitiveCommand(string meshType, Vector3 pos = default, Vector3 scale = default)
        {
            MeshType = meshType;
            Pos = pos;
            if (scale == default)
                Scale = Vector3.one;
            else
                Scale = scale;
            Details = 10;
            Details2 = 10;
        }

        public void Apply()
        {
            MyMesh mesh;
            switch (MeshType) {
                case "Cube":
                    mesh = MeshGenerator.GenerateCube(/*Pos, Scale*/);
                    break;
                case "Sphere":
                    mesh = SphereGenerator.GenerateSphere(100, 100);
                    break;
                case "Cone":
                    throw new NotImplementedException();
                    //mesh = MeshGenerator.GenerateCone(Pos, Scale.x, Scale.y, Details);
                    //break;
                case "Cylinder":
                    throw new NotImplementedException();
                    //mesh = MeshGenerator.GenerateCylinder(Pos, Scale.x, Scale.y, Details);
                    //break;
                case "Torus":
                    mesh = MeshGenerator.GenerateTorus(/*Pos, Scale.x, Scale.y, Details, Details2*/);
                    break;
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