using System.Collections.Generic;
using UnityEngine;

namespace MyMesh
{
    public struct MyMesh
    {
        public List<Vector3> Vertices;
        public List<int> Triangles;

        public Mesh ToUnityMesh()
        {
            return new Mesh {vertices = Vertices.ToArray(), triangles = Triangles.ToArray()};
        }
    }
}
