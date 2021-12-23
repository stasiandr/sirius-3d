using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Scripts.Networking;
using SceneProvider;
using Commands;
using Newtonsoft.Json.Linq;

public class ClientProcessing : MonoBehaviour
{
    public static Client client = new Client();

    public void ProcessMessage(string message)
    {
        Debug.Log(message);
        string commandType = JObject.Parse(message)["CommandType"].Value<string>();
        switch (commandType) 
        {
            case "Transform":
                SceneData.ExecutionQueue.Enqueue(TransformCommand.Deserialize(message));
                break;
            case "Rotate":
                SceneData.ExecutionQueue.Enqueue(RotateCommand.Deserialize(message));
                break;
            case "Scale":
                SceneData.ExecutionQueue.Enqueue(ScaleCommand.Deserialize(message));
                break;
            case "Delete":
                SceneData.ExecutionQueue.Enqueue(DeleteCommand.Deserialize(message));
                break;
            case "CreatePrimitive":
                SceneData.ExecutionQueue.Enqueue(CreatePrimitiveCommand.Deserialize(message));
                break;
            case "UploadObject":
                SceneData.ExecutionQueue.Enqueue(UploadObjectCommand.Deserialize(message));
                break;
        }
    }

    public void Enter(string str)
    {
        StartCoroutine(Activate(str));
        gameObject.transform.position = new Vector3(1000000, 10000000, 1000000);
    }

    public IEnumerator Activate(string str)
    {
        SceneData.HasStarted = true;
        if (str == "SinglePlayer")
        {
            SceneData.SinglePlayer = true;
            yield return null;
        }
        else
        {
            client.MessageReceived += ProcessMessage;
            client.Init(str);

            yield return null;

            while (!client.IsOpen)
                yield return null;
        }
    }

    void Update()
    {
        client.UpdateState();
    }

    private void OnDestroy()
    {
        client.Quit();
    }
}
