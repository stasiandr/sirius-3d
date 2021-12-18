using System;
using Commands;
using SceneProvider;
using UnityEngine;

namespace UIController
{
    public class RandomMoveCommand : MonoBehaviour
    {

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RandomMove(1f);
            }
        }


        public void RandomMove(float max_delta)
        {
            float new_x = UnityEngine.Random.Range(-max_delta, max_delta);
            float new_y = UnityEngine.Random.Range(-max_delta, max_delta);
            float new_z = UnityEngine.Random.Range(-max_delta, max_delta);
            print("lol");
            print(SceneData.Targets.Count);
            SceneData.ExecutionQueue.Enqueue(new TransformCommand(SceneData.Targets, new Vector3(new_x, new_y, new_z)));
        }

    }
}
