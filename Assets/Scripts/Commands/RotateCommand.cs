using System;
using System.Collections.Generic;
using MeshTools;
using SceneProvider;
using UnityEngine;

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
    }
}