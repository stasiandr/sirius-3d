using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneProvider;
using MeshTools;
using Newtonsoft.Json.Linq;
using UIController;
using UnityEngine.UI;

namespace Commands
{
    public class UploadObjectCommand : ICommand
    {
        public string name;
        public string path;
        public MyMesh uploadedMesh;


        public UploadObjectCommand(string _name, string _path = "") {
            name = _name;
            if (_path == "") {
                return;
            } else {
                path = _path;
            }
            var new_mesh = new MyMesh();
            var obj = new Dummiesman.OBJLoader().Load(path);
            if (obj.GetComponent<MeshFilter>())
            {
                var mesh = obj.GetComponent<MeshFilter>().mesh;
                new_mesh.Vertices = new List<Vector3>();
                foreach (var ver in mesh.vertices)
                {
                    new_mesh.Vertices.Add(ver);
                }
                new_mesh.Triangles = new List<int>();
                foreach (var tr in mesh.triangles)
                    new_mesh.Triangles.Add(tr);
            }
            else
            {
                new_mesh.Vertices = new List<Vector3>();
                new_mesh.Triangles = new List<int>();
                int N = 0;
                foreach (Transform obj2 in obj.transform)
                {
                    var mesh = obj2.gameObject.GetComponent<MeshFilter>().mesh;
                    foreach (var ver in mesh.vertices)
                        new_mesh.Vertices.Add(ver);
                    foreach (var tr in mesh.triangles)
                        new_mesh.Triangles.Add(tr + N);
                    N += mesh.vertices.Length;
                }
            }
            Vector3 lb = new Vector3(100000, 100000, 100000);
            Vector3 rt = new Vector3(-100000, -100000, -100000);
            foreach (var ver in new_mesh.Vertices)
            {
                if (lb.x > ver.x)
                    lb.x = ver.x;
                if (lb.y > ver.y)
                    lb.y = ver.y;
                if (lb.z > ver.z)
                    lb.z = ver.z;
                if (rt.x < ver.x)
                    rt.x = ver.x;
                if (rt.y < ver.y)
                    rt.y = ver.y;
                if (rt.z < ver.z)
                    rt.z = ver.z;
            }
            for (int i = 0; i < new_mesh.Vertices.Count; ++i)
            {
                new_mesh.Vertices[i] -= lb;
            }
            if ((lb - rt).sqrMagnitude > 20000)
            {
                for (int i = 0; i < new_mesh.Vertices.Count; ++i)
                {
                    new_mesh.Vertices[i] /= 100;
                }
            }
            uploadedMesh = new_mesh;
            GameObject.Destroy(obj);
        }

        public void Apply()
        {
            SceneData.UploadedMeshes[name] = uploadedMesh;

            var new_button = GameObject.Instantiate(SceneData.create_button_prefab);
            new_button.transform.parent = SceneData.buttons_scrollview_transform;
            new_button.transform.localScale = Vector3.one;
            new_button.GetComponent<CreateUploadedButton>().Name = name;
            new_button.transform.GetChild(0).GetComponent<Text>().text = name;
        }

        public void Revert()
        {
            throw new System.NotImplementedException(); ;
        }

        public string Serialize()
        {
            JArray vert = new JArray();
            foreach (var el in uploadedMesh.Vertices)
                vert.Add(new JObject(new JProperty("x", el.x), new JProperty("y", el.y), new JProperty("z", el.z)));
            JObject json = new JObject(new JProperty("CommandType", "UploadObject"),
                new JProperty("path", path), new JProperty("name", name),
                new JProperty("Vertices", vert),
                new JProperty("Triangles", new JArray(uploadedMesh.Triangles)));
            return json.ToString();
        }

        public static UploadObjectCommand Deserialize(string str)
        {
            JObject json = JObject.Parse(str);
            UploadObjectCommand command = new UploadObjectCommand(json["name"].Value<string>(), "");
            command.uploadedMesh = new MyMesh();
            JArray vert = json["Vertices"].Value<JArray>();
            command.uploadedMesh.Vertices = new List<Vector3>();
            foreach (var el in vert)
                command.uploadedMesh.Vertices.Add(new Vector3(el["x"].Value<float>(), el["y"].Value<float>(), el["z"].Value<float>()));
            command.uploadedMesh.Triangles = json["Triangles"].Value<JArray>().ToObject<List<int>>();
            return command;
        }
    }
}
