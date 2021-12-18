using System;
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

        private static int GetNextPosition(int i, int modulo)
        {
            ++i;
            if (i >= modulo) i -= modulo;
            return i;
        }

        private static MyMesh GenerateCircle(bool isDown, float yPosition, int vertAmount = 100, double radius = 1)
        {
            var mesh = new MyMesh
            {
                Vertices = new List<Vector3>(),
                Triangles = new List<int>()
            };

            for (int i = 0; i < vertAmount; ++i)
            {
                double step = 2 * Math.PI / vertAmount;
                float x = (float)(Math.Cos(step * i) * radius);
                float z = (float)(Math.Sin(step * i) * radius);
                mesh.Vertices.Add(new Vector3(x, yPosition, z));
            }

            mesh.Vertices.Add(new Vector3(0, yPosition, 0));
            int centerIndex = vertAmount;

            if (isDown)
            {
                for (int i = 0; i < vertAmount; ++i)
                {
                    mesh.AddTriangle(centerIndex, i, GetNextPosition(i, vertAmount));
                }
            }
            else
            {
                for (int i = 0; i < vertAmount; ++i)
                {
                    mesh.AddTriangle(centerIndex, GetNextPosition(i, vertAmount), i);
                }
            }

            return mesh;
        }

        private static List<Vector3> MergeVertices(List<Vector3> verticesA, List<Vector3> verticesB)
        {
            var result = new List<Vector3>();
            
            foreach (var vertex in verticesA)
            {
                result.Add(vertex);
            }
            
            foreach (var vertex in verticesB)
            {
                result.Add(vertex);
            }

            return result;
        }

        private static List<int> MergeTriangles(List<int> trianglesA, List<int> trianglesB, int add)
        {
            var result = new List<int>();

            foreach (var triangle in trianglesA)
            {
                result.Add(triangle);
            }
            
            foreach (var triangle in trianglesB)
            {
                result.Add(triangle + add);
            }

            return result;
        }
        
        public static MyMesh GenerateCylinder(float height = 3, double radius = 1, int vertexCount = 100)
        {
            MyMesh bottomCircle = GenerateCircle(true, 0, vertexCount, radius),
                topCircle = GenerateCircle(false, height, vertexCount, radius);

            List<Vector3> vertices = MergeVertices(bottomCircle.Vertices, topCircle.Vertices);
            List<int> triangles = MergeTriangles(bottomCircle.Triangles, topCircle.Triangles, vertexCount + 1);

            var mesh = new MyMesh
            {
                Vertices = vertices,
                Triangles = triangles
            };
            
            for (int firstVertex = 0; firstVertex < vertexCount; ++firstVertex)
            {
                int secondVertex = firstVertex + vertexCount + 1;
                int thirdVertex = GetNextPosition(firstVertex, vertexCount) + vertexCount + 1;
                
                mesh.AddTriangle(firstVertex, secondVertex, thirdVertex);

                secondVertex = thirdVertex;
                thirdVertex = GetNextPosition(firstVertex, vertexCount);
                
                mesh.AddTriangle(firstVertex, secondVertex, thirdVertex);
            }

            return mesh;
        }

        public static MyMesh GenerateCone(float height = 2, double radius = 1, int vertexCount = 100)
        {
            var foundation = GenerateCircle(true, 0, vertexCount, radius);
            var topVertex = new Vector3(0, height, 0);

            var mesh = new MyMesh
            {
                Vertices = foundation.Vertices,
                Triangles = foundation.Triangles
            };

            int topVertexIndex = mesh.Vertices.Count;
            mesh.Vertices.Add(topVertex);

            for (int firstVertex = 0; firstVertex < vertexCount; ++firstVertex)
            {
                int secondVertex = GetNextPosition(firstVertex, vertexCount);
                mesh.AddTriangle(topVertexIndex, secondVertex, firstVertex);
            }

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

