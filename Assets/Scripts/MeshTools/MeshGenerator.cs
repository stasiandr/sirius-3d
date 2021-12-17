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

            mesh.Triangles = new List<int>{ 0, 2, 4, 2, 6, 4, 1, 5, 3, 5, 7, 3, 4, 6, 5, 6, 7, 5, 0, 2, 1, 2, 3, 1, 2, 3, 6, 3, 7, 6, 0, 1, 4, 1, 5, 4 }; 
            
            return mesh;
        }
        public static MyMesh GenerateTorus(Vector3 pos = default, float r1 = 0.5f, float r2 = 1.5f, int vert1 = 20, int vert2 = 10)
        {
            var mesh = new MyMesh
            {
                Vertices = new List<Vector3>(),
                Triangles = new List<int>()
            };
            List<List<int>> Circles = new List<List<int>>();
            for (int i = 0; i < vert1; ++i)
            {
                List<int> circle = new List<int>();
                float step = Mathf.PI * 2 / vert1;
                float x = Mathf.Cos(step * i) * (r1 + r2) / 2f;
                float y = Mathf.Sin(step * i) * (r1 + r2) / 2f;
                for (int j = 0; j < vert2; ++j)
                {
                    float step2 = Mathf.PI * 2 / vert2;
                    float z = Mathf.Cos(step2 * j) * (r2 - r1) / 2f;
                    float x2 = x + x * (r2 - r1) / (r1 + r2) * Mathf.Sin(step2 * j);
                    float y2 = y + y * (r2 - r1) / (r1 + r2) * Mathf.Sin(step2 * j);
                    circle.Add(mesh.Vertices.Count);
                    mesh.Vertices.Add(new Vector3(x2, y2, z) + pos);
                }
                Circles.Add(circle);
            }
            for (int i = 0; i < vert1; ++i)
            {
                for (int j = 0; j < vert2; ++j)
                {
                    mesh.Triangles.Add(Circles[(i + 1) % vert1][j]);
                    mesh.Triangles.Add(Circles[i][j]);
                    mesh.Triangles.Add(Circles[i][(j + 1) % vert2]);
                    mesh.Triangles.Add(Circles[(i + vert1 - 1) % vert1][j]);
                    mesh.Triangles.Add(Circles[i][j]);
                    mesh.Triangles.Add(Circles[i][(j + vert2 - 1) % vert2]);
                }
            }
            return mesh;
        }
    }
}

