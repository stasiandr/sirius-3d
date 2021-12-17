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

        TransformCommand(Vector3 trans = default)
        {
            Objects = new List<GameObject>(); //SceneData.Targets;
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