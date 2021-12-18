using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneProvider;

namespace UIController
{
    public class TRSTransform : MonoBehaviour
    {
        public void TransformTargetsX(string s)
        {
            if (!float.TryParse(s, out float newpos) || SceneData.Targets.Count == 0 || SceneData.Targets[0] == null)
            {
                return;
            }
            SceneData.ExecutionQueue.Enqueue(new Commands.TransformCommand(SceneData.Targets, new Vector3(newpos -
                SceneData.Targets[0].transform.position.x, 0, 0)));
        }

        public void TransformTargetsY(string s)
        {
            if (!float.TryParse(s, out float newpos) || SceneData.Targets.Count == 0 || SceneData.Targets[0] == null)
            {
                return;
            }
            SceneData.ExecutionQueue.Enqueue(new Commands.TransformCommand(SceneData.Targets, new Vector3(0, newpos -
                SceneData.Targets[0].transform.position.y, 0)));
        }

        public void TransformTargetsZ(string s)
        {
            if (!float.TryParse(s, out float newpos) || SceneData.Targets.Count == 0 || SceneData.Targets[0] == null)
            {
                return;
            }
            SceneData.ExecutionQueue.Enqueue(new Commands.TransformCommand(SceneData.Targets, new Vector3(0, 0, newpos -
                SceneData.Targets[0].transform.position.z)));
        }


    }
}
