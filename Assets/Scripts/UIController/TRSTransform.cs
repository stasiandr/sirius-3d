using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SceneProvider;
using TMPro;

namespace UIController
{
    public class TRSTransform : MonoBehaviour
    {
        public TMP_InputField posX, posY, posZ;
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
            posX.SetTextWithoutNotify(target.transform.position.x.ToString());
            posY.SetTextWithoutNotify(target.transform.position.y.ToString());
            posZ.SetTextWithoutNotify(target.transform.position.z.ToString());
        }

        public void TransformTargetsX(string s)
        {
            if (!float.TryParse(s, out float newpos) || Targets.Count == 0)
            {
                return;
            }
            SceneData.RequestQueue.Enqueue(new Commands.TransformCommand(Targets, new Vector3(newpos -
                Targets[0].transform.position.x, 0, 0)));
        }

        public void TransformTargetsY(string s)
        {
            if (!float.TryParse(s, out float newpos) || Targets.Count == 0)
            {
                return;
            }
            SceneData.RequestQueue.Enqueue(new Commands.TransformCommand(Targets, new Vector3(0, newpos -
                Targets[0].transform.position.y, 0)));
        }

        public void TransformTargetsZ(string s)
        {
            if (!float.TryParse(s, out float newpos) || Targets.Count == 0)
            {
                return;
            }
            SceneData.RequestQueue.Enqueue(new Commands.TransformCommand(Targets, new Vector3(0, 0, newpos -
                Targets[0].transform.position.z)));
        }


    }
}
