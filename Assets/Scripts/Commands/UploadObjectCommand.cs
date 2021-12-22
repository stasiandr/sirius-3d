using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneProvider;
using MeshTools;

namespace Commands
{
    public class UploadObjectCommand : ICommand
    {
        public string name;

        public UploadObjectCommand(string _name) {
            name = _name;
        }

        public void Apply()
        {
            var new_mesh = new MyMesh();
            string path = FilePath();
            var obj = new Dummiesman.OBJLoader().Load(path);
            if (obj.GetComponent<MeshFilter>())
            {
                var mesh = obj.GetComponent<MeshFilter>().mesh;
                new_mesh.Vertices = new List<Vector3>();
                foreach (var ver in mesh.vertices)
                    new_mesh.Vertices.Add(ver);
                new_mesh.Triangles = new List<int>();
                foreach (var tr in mesh.triangles)
                    new_mesh.Triangles.Add(tr);
            }
            else
            {
                new_mesh.Vertices = new List<Vector3>();
                new_mesh.Triangles = new List<int>();
                int N = 0;
                foreach (Transform obj2 in obj.transform)
                {
                    var mesh = obj2.gameObject.GetComponent<MeshFilter>().mesh;
                    foreach (var ver in mesh.vertices)
                        new_mesh.Vertices.Add(ver);
                    foreach (var tr in mesh.triangles)
                        new_mesh.Triangles.Add(tr + N);
                    N += mesh.vertices.Length;
                }
            }
            SceneData.UploadedMeshes[name] = new_mesh;
            GameObject.Destroy(obj);
        }

        public string FilePath()
        {
            return "al.obj";
        }

        public void Revert()
        {
            throw new System.NotImplementedException(); ;
        }
    }
}
