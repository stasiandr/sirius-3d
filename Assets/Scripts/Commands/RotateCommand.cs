using System;
using System.Collections.Generic;
using MeshTools;
using SceneProvider;
using UnityEngine;
using Newtonsoft.Json.Linq;

namespace Commands
{
    public struct RotateCommand : ICommand
    {
        Vector3 Trans;
        List<int> Objects;

        public RotateCommand(List<GameObject> targets, Vector3 trans = default)
        {
            Objects = new List<int>();
            foreach (var obj in targets)
                Objects.Add(Convert.ToInt32(obj.name));
            Trans = trans;
        }

        public void Apply()
        {
            foreach (var obj in Objects)
            {
                SceneData.ObjectsByID[obj].transform.eulerAngles += Trans;
            }
        }

        public void Revert()
        {
            foreach (var obj in Objects)
            {
                SceneData.ObjectsByID[obj].transform.eulerAngles -= Trans;
            }
        }

        public static string Serialize(RotateCommand command)
        {
            JObject json = new JObject(new JProperty("CommandType", "Rotate"),
                new JProperty("Vector3", new JObject(new JProperty("x", command.Trans.x),
                new JProperty("y", command.Trans.y), new JProperty("z", command.Trans.z))),
                new JProperty("Objects", new JArray(command.Objects)));
            return json.ToString();
        }

        public static RotateCommand Deserialize(string str)
        {
            JObject json = JObject.Parse(str);
            RotateCommand command = new RotateCommand();
            command.Trans.x = json["Vector3"]["x"].Value<float>();
            command.Trans.y = json["Vector3"]["y"].Value<float>();
            command.Trans.z = json["Vector3"]["z"].Value<float>();
            command.Objects = json["Objects"].Value<JArray>().ToObject<List<int>>();
            return command;
        }
    }
}