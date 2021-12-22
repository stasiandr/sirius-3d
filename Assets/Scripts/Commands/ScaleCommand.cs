using System;
using System.Collections.Generic;
using MeshTools;
using SceneProvider;
using UnityEngine;

namespace Commands
{
    public struct ScaleCommand : ICommand
    {
        Vector3 Trans;
        List<int> Objects;

        public ScaleCommand(List<GameObject> targets, Vector3 trans = default)
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
                SceneData.ObjectsByID[obj].transform.localScale = Vector3.Scale(SceneData.ObjectsByID[obj].transform.localScale, Trans);
            }
        }

        public void Revert()
        {
            foreach (var obj in Objects)
            {
                SceneData.ObjectsByID[obj].transform.localScale = Vector3.Scale(SceneData.ObjectsByID[obj].transform.localScale, new Vector3(1 / Trans.x, 1 / Trans.y, 1 / Trans.z));
            }
        }
    }
}