using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeshTools
{
    public static class SphereGenerator
    {
        public static MyMesh GenerateSphere(int parallels, int meridians)
        {
            var mesh = new MyMesh
            {
                Vertices = new List<Vector3>(),
                Triangles = new List<int>()
            };

            mesh.Vertices.Add(new Vector3(0, 1, 0));
            for (int i = 1; i < parallels; ++i)
            {
                var phi = System.Math.PI * i / parallels;
                for (int j = 0; j < meridians; ++j)
                {
                    var theta = 2 * System.Math.PI * j / meridians;
                    var x = (float)(System.Math.Sin(phi) * System.Math.Cos(theta));
                    var y = (float)(System.Math.Cos(phi));
                    var z = (float)(System.Math.Sin(phi) * System.Math.Sin(theta));
                    mesh.Vertices.Add(new Vector3(x, y, z));
                }
            }
            mesh.Vertices.Add(new Vector3(0, -1, 0));

            var topVertice = 0;
            var bottomVertice = 1 + (parallels - 1) * meridians;
            for (int i = 0; i < meridians; ++i)
            {
                var i0 = i + 1;
                var i1 = (i + 1) % meridians + 1;
                mesh.AddTriangle(topVertice, i1, i0);
                i0 = i + meridians * (parallels - 2) + 1;
                i1 = (i + 1) % meridians + meridians * (parallels - 2) + 1;
                mesh.AddTriangle(bottomVertice, i0, i1);
            }

            for (int j = 0; j < parallels - 2; ++j)
            {
                var j0 = j * meridians + 1;
                var j1 = (j + 1) * meridians + 1;
                for (int i = 0; i < meridians; ++i)
                {
                    var i0 = j0 + i;
                    var i1 = j0 + (i + 1) % meridians;
                    var i2 = j1 + (i + 1) % meridians;
                    var i3 = j1 + i;
                    mesh.AddTriangle(i0, i1, i3);
                    mesh.AddTriangle(i1, i2, i3);
                }
            }

            return mesh;
        }
    }
}
