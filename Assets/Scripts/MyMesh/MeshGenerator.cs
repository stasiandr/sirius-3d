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


            m.Triangles.Add(0);
            m.Triangles.Add(1);
            m.Triangles.Add(2);


            return m;
        }
    }
}

