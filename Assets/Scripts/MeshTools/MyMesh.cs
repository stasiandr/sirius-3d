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

        public void AddTriangle(int i0, int i1, int i2)
        {
            this.Triangles.Add(i0);
            this.Triangles.Add(i1);
            this.Triangles.Add(i2);
        }
    }
}
