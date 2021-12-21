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
        List<GameObject> Objects;

        public ScaleCommand(List<GameObject> targets, Vector3 trans = default)
        {
            Objects = targets;
            Trans = trans;
        }

        public void Apply()
        {
            foreach (var obj in Objects)
            {
                obj.transform.localScale = Vector3.Scale(obj.transform.localScale, Trans);
            }
        }

        public void Revert()
        {
            foreach (var obj in Objects)
            {
                obj.transform.localScale = Vector3.Scale(obj.transform.localScale, new Vector3(1 / Trans.x, 1 / Trans.y, 1 / Trans.z));
            }
        }
    }
}