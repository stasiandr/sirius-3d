using System;
using MeshTools;
using SceneProvider;
using UnityEngine;
using Newtonsoft.Json.Linq;

namespace Commands
{
    public struct CreatePrimitiveCommand : ICommand
    {
        public string MeshType;
        public Vector3 Pos;
        public Vector3 Scale;
        public int Details;
        public int Details2;
        int MyObjID;
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
            MyObjID = 0;
        }

        public void Apply()
        {
            MyMesh mesh;
            switch (MeshType) {
                case "Cube":
                    mesh = MeshGenerator.GenerateCube(/*Pos, Scale*/);
                    break;
                case "Sphere":
                    mesh = SphereGenerator.GenerateSphere(10, 10);
                    break;
                case "Cone":
                    mesh = MeshGenerator.GenerateCone(/*Height, Radius, VertexCount*/);
                    break;
                case "Cylinder":
                    mesh = MeshGenerator.GenerateCylinder(/*Height, Radius, VertexCount*/);
                    break;
                case "Torus":
                    mesh = MeshGenerator.GenerateTorus(/*Pos, Scale.x, Scale.y, Details, Details2*/);
                    break;
                case "Plane":
                    mesh = MeshGenerator.GeneratePlane();
                    break;
                default:
                    throw new NotImplementedException();
            }
            MyObjID = SceneData.CreateMesh(mesh);
        }

        public void Revert()
        {
            GameObject.Destroy(SceneData.ObjectsByID[MyObjID]);
            SceneData.ObjectsByID.Remove(MyObjID);
        }

        public static string Serialize(CreatePrimitiveCommand command)
        {
            JObject json = new JObject(new JProperty("CommandType", "CreatePrimitive"),
                new JProperty("MeshType", command.MeshType));
            return json.ToString();
        }

        public static CreatePrimitiveCommand Deserialize(string str)
        {
            JObject json = JObject.Parse(str);
            CreatePrimitiveCommand command = new CreatePrimitiveCommand();
            command.MeshType = json["MeshType"].Value<string>();
            return command;
        }
    }
}