using System;
using System.Collections.Generic;
using MeshTools;
using SceneProvider;
using UnityEngine;

namespace Commands
{
    public struct TransformCommand : ICommand
    {
        Vector3 Trans;
        List<GameObject> Objects;

        public TransformCommand(List<GameObject> targets, Vector3 trans = default)
        {
            Objects = targets;
            Trans = trans;
        }

        public void Apply()
        {
            foreach(var obj in Objects)
            {
                obj.transform.Translate(Trans);
            }
        }

        public void Revert()
        {
            foreach (var obj in Objects)
            {
                obj.transform.Translate(-Trans);
            }
        }
    }
}