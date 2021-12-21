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
        List<GameObject> Objects;

        public RotateCommand(List<GameObject> targets, Vector3 trans = default)
        {
            Objects = targets;
            Trans = trans;
        }

        public void Apply()
        {
            foreach (var obj in Objects)
            {
                obj.transform.eulerAngles += Trans;
            }
        }

        public void Revert()
        {
            foreach (var obj in Objects)
            {
                obj.transform.Rotate(-Trans, Space.World);
            }
        }
    }
}