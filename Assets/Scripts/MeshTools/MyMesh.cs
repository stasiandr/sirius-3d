using System.Collections.Generic;
using UnityEngine;

namespace MeshTools
{
    public struct MyMesh
    {
        public List<Vector3> Vertices;
        public List<int> Triangles;

        public Mesh ToUnityMesh()
        {
            var mesh = new Mesh
            {
                vertices = Vertices.ToArray(), 
                triangles = Triangles.ToArray()
            };
            
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            
            return mesh;
        }
    }
}
