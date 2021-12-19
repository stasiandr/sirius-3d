using System;
using System.Collections.Generic;
using MeshTools;
using SceneProvider;
using UnityEngine;

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
                SceneData.ObjectsByID[id] = null;
            }
            SceneData.Targets = new List<GameObject>();
        }

        public void Revert()
        {
            throw new NotImplementedException();
            //to do откат удаления
            //SceneData.ExecutionQueue.Enequeue(new CreatePrimitiveCommand(MeshTypes[i], Positions[i], ...));
        }
    }
}