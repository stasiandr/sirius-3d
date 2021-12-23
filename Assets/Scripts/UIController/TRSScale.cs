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
            if (!float.TryParse(s, out float newpos) || Targets.Count == 0 || newpos == 0)
            {
                return;
            }
            SceneData.RequestQueue.Enqueue(new Commands.ScaleCommand(Targets, 
                new Vector3(newpos / Targets[0].transform.localScale.x, 1, 1)));
        }

        public void ScaleTargetsY(string s)
        {
            if (!float.TryParse(s, out float newpos) || Targets.Count == 0 || newpos == 0)
            {
                return;
            }
            SceneData.RequestQueue.Enqueue(new Commands.ScaleCommand(Targets,
                new Vector3(1, newpos / Targets[0].transform.localScale.y, 1)));
        }

        public void ScaleTargetsZ(string s)
        {
            if (!float.TryParse(s, out float newpos) || Targets.Count == 0 || newpos == 0)
            {
                return;
            }
            SceneData.RequestQueue.Enqueue(new Commands.ScaleCommand(Targets,
                new Vector3(1, 1, newpos / Targets[0].transform.localScale.z)));
        }
    }
}
