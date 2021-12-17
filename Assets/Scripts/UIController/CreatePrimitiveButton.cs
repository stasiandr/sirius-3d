using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePrimitiveButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnClick()
    {
        SceneProvider.SceneData.ExecutionQueue.Enqueue(new UIController.CreateCubeCommand());
    }
}
