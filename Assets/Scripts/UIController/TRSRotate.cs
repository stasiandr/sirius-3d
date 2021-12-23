using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SceneProvider;
using TMPro;

namespace UIController
{
    public class TRSRotate : MonoBehaviour
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
            print(target);
            posX.SetTextWithoutNotify(target.transform.rotation.eulerAngles.x.ToString());
            posY.SetTextWithoutNotify(target.transform.rotation.eulerAngles.y.ToString());
            posZ.SetTextWithoutNotify(target.transform.rotation.eulerAngles.z.ToString());
        }

        public void RotateTargetsX(string s)
        {
            if (!float.TryParse(s, out float newpos) || Targets.Count == 0)
            {
                return;
            }
            var angle = Targets[0].transform.rotation.eulerAngles;
            SceneData.RequestQueue.Enqueue(new Commands.RotateCommand(Targets,
                new Vector3(newpos - angle.x, 0, 0)));
        }

        public void RotateTargetsY(string s)
        {
            if (!float.TryParse(s, out float newpos) || Targets.Count == 0)
            {
                return;
            }
            var angle = Targets[0].transform.rotation.eulerAngles;
            SceneData.RequestQueue.Enqueue(new Commands.RotateCommand(Targets,
                new Vector3(0, newpos - angle.y, 0)));
        }

        public void RotateTargetsZ(string s)
        {
            if (!float.TryParse(s, out float newpos) || Targets.Count == 0)
            {
                return;
            }
            var angle = Targets[0].transform.rotation.eulerAngles;
            SceneData.RequestQueue.Enqueue(new Commands.RotateCommand(Targets,
                new Vector3(0, 0, newpos - angle.z)));
        }
    }
}
