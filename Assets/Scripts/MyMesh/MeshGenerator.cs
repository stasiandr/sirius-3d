using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyMesh
{
    public static class MeshGenerator
    {
        public static MyMesh GenerateCube()
        {
            MyMesh m = new MyMesh();
            for (int x = 0; x <= 1; ++x)
                for (int y = 0; y <= 1; ++y)
                    for (int z = 0; z <= 1; ++z)
                    {
                        m.Vertices.Add(new Vector3(x, y, z));
                    }

            m.Triangles = new List<int>{ 0, 2, 4, 2, 6, 4, 1, 5, 3, 5, 7, 3, 4, 6, 5, 6, 7, 5, 0, 2, 1, 2, 3, 1, 2, 3, 6, 3, 7, 6, 0, 1, 4, 1, 5, 4 }; 
            
            return m;
        }
    }
}

