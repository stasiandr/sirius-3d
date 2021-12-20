using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SceneProvider;

namespace UIController
{
    public class TRSScale : MonoBehaviour
    {
        public InputField posX, posY, posZ;
        private List<GameObject> Targets;

        public void Start()
        {
            SceneData.ObjectsSelected += ObjectSelected;
        }

        public void ObjectSelected(List<GameObject> _Targets)
        {
            Targets = _Targets;
            if (Targets.Count == 0)
            {
                posX.SetTextWithoutNotify("");
                posY.SetTextWithoutNotify("");
                posZ.SetTextWithoutNotify("");
                return;
            }
            var target = Targets[0];
            posX.SetTextWithoutNotify(target.transform.localScale.x.ToString());
            posY.SetTextWithoutNotify(target.transform.localScale.y.ToString());
            posZ.SetTextWithoutNotify(target.transform.localScale.z.ToString());
        }
        public void ScaleTargetsX(string s)
        {
            if (!float.TryParse(s, out float newpos) || Targets.Count == 0 || Targets[0] == null)
            {
                return;
            }
            SceneData.ExecutionQueue.Enqueue(new Commands.ScaleCommand(Targets, new Vector3(newpos,
                Targets[0].transform.localScale.y, Targets[0].transform.localScale.z)));
        }

        public void ScaleTargetsY(string s)
        {
            if (!float.TryParse(s, out float newpos) || Targets.Count == 0 || Targets[0] == null)
            {
                return;
            }
            SceneData.ExecutionQueue.Enqueue(new Commands.ScaleCommand(Targets, new Vector3(Targets[0].transform.localScale.x,
                newpos, Targets[0].transform.localScale.z)));
        }

        public void ScaleTargetsZ(string s)
        {
            if (!float.TryParse(s, out float newpos) || Targets.Count == 0 || Targets[0] == null)
            {
                return;
            }
            SceneData.ExecutionQueue.Enqueue(new Commands.ScaleCommand(Targets, new Vector3(Targets[0].transform.localScale.x,
                Targets[0].transform.localScale.y, newpos)));
        }
    }
}
