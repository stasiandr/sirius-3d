using System.Collections.Generic;
using UnityEngine;

namespace MeshTools
{
    public static class MeshGenerator
    {
        public static MyMesh GenerateCube()
        {
            var mesh = new MyMesh
            {
                Vertices = new List<Vector3>()
            };

            for (int x = 0; x <= 1; ++x)
                for (int y = 0; y <= 1; ++y)
                    for (int z = 0; z <= 1; ++z)
                        mesh.Vertices.Add(new Vector3(x, y, z));

            mesh.Triangles = new List<int>{ 0, 2, 4, 2, 6, 4, 1, 5, 3, 5, 7, 3, 4, 6, 5, 6, 7, 5, 1, 2, 0, 1, 3, 2, 2, 3, 6, 3, 7, 6, 4, 1, 0, 4, 5, 1 }; 
            
            return mesh;
        }
    }
}

