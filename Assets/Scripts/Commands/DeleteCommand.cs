using System;
using System.Collections.Generic;
using MeshTools;
using SceneProvider;
using UnityEngine;
using Newtonsoft.Json.Linq;

namespace Commands
{
    public struct DeleteCommand : ICommand
    {
        List<int> Objects;
        List<string> MeshTypes;
        List<Vector3> Positions;
        List<Vector3> Scales;
        List<float> Details1;
        List<float> Details2;

        public DeleteCommand(List<GameObject> objs)
        {
            Objects = new List<int>();
            foreach (var obj in objs)
            {
                Objects.Add(Convert.ToInt32(obj.name));
            }
            MeshTypes = new List<string>();
            Positions = new List<Vector3>();
            Scales = new List<Vector3>();
            Details1 = new List<float>();
            Details2 = new List<float>();
        }

        public void Apply()
        {
            foreach (int id in Objects)
            {
                var obj = SceneData.ObjectsByID[id];
                GameObject.Destroy(obj);
                //to do запонимать все свойства удалённого объекта
                SceneData.ObjectsByID.Remove(id);
            }
            SceneData.Targets = new List<GameObject>();
        }

        public void Revert()
        {
            throw new NotImplementedException();
            //to do откат удаления
            //SceneData.ExecutionQueue.Enequeue(new CreatePrimitiveCommand(MeshTypes[i], Positions[i], ...));
        }

        public string Serialize()
        {
            JObject json = new JObject(new JProperty("CommandType", "Delete"),
                new JProperty("Objects", new JArray(this.Objects)));
            return json.ToString();
        }

        public static DeleteCommand Deserialize(string str)
        {
            JObject json = JObject.Parse(str);
            DeleteCommand command = new DeleteCommand();
            command.Objects = json["Objects"].Value<JArray>().ToObject<List<int>>();
            return command;
        }
    }
}