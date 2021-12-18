using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneProvider;

namespace UIController
{
    public class TRSScale : MonoBehaviour
    {
        public void ScaleTargetsX(string s)
        {
            if (!float.TryParse(s, out float newpos) || SceneData.Targets.Count == 0 || SceneData.Targets[0] == null)
            {
                return;
            }
            SceneData.ExecutionQueue.Enqueue(new Commands.ScaleCommand(SceneData.Targets, new Vector3(newpos,
                SceneData.Targets[0].transform.localScale.y, SceneData.Targets[0].transform.localScale.z)));
        }

        public void ScaleTargetsY(string s)
        {
            if (!float.TryParse(s, out float newpos) || SceneData.Targets.Count == 0 || SceneData.Targets[0] == null)
            {
                return;
            }
            SceneData.ExecutionQueue.Enqueue(new Commands.ScaleCommand(SceneData.Targets, new Vector3(SceneData.Targets[0].transform.localScale.x,
                newpos, SceneData.Targets[0].transform.localScale.z)));
        }

        public void ScaleTargetsZ(string s)
        {
            if (!float.TryParse(s, out float newpos) || SceneData.Targets.Count == 0 || SceneData.Targets[0] == null)
            {
                return;
            }
            SceneData.ExecutionQueue.Enqueue(new Commands.ScaleCommand(SceneData.Targets, new Vector3(SceneData.Targets[0].transform.localScale.x,
                SceneData.Targets[0].transform.localScale.y, newpos)));
        }
    }
}
