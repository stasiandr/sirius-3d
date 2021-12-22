using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneProvider;
using MeshTools;

namespace Commands
{
    public class UploadObjectCommand : ICommand
    {
        public void Apply()
        {
            SceneData.UploadedMesh = new MyMesh();
            string path = FilePath();
            var obj = new Dummiesman.OBJLoader().Load(path);
            if (obj.GetComponent<MeshFilter>())
            {
                var mesh = obj.GetComponent<MeshFilter>().mesh;
                SceneData.UploadedMesh.Vertices = new List<Vector3>();
                foreach (var ver in mesh.vertices)
                    SceneData.UploadedMesh.Vertices.Add(ver);
                SceneData.UploadedMesh.Triangles = new List<int>();
                foreach (var tr in mesh.triangles)
                    SceneData.UploadedMesh.Triangles.Add(tr);
            }
            else
            {
                SceneData.UploadedMesh.Vertices = new List<Vector3>();
                SceneData.UploadedMesh.Triangles = new List<int>();
                int N = 0;
                foreach (Transform obj2 in obj.transform)
                {
                    var mesh = obj2.gameObject.GetComponent<MeshFilter>().mesh;
                    foreach (var ver in mesh.vertices)
                        SceneData.UploadedMesh.Vertices.Add(ver);
                    foreach (var tr in mesh.triangles)
                        SceneData.UploadedMesh.Triangles.Add(tr + N);
                    N += mesh.vertices.Length;
                }
            }
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
