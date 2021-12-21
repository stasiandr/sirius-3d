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
            var mesh = obj.GetComponent<MeshFilter>().mesh;
            foreach (var ver in mesh.vertices)
                SceneData.UploadedMesh.Vertices.Add(ver);
            foreach (var tr in mesh.triangles)
                SceneData.UploadedMesh.Triangles.Add(tr);
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
