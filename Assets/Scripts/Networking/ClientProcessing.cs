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
        }
    }

    IEnumerator Start()
    {
        client = new Client();
        client.MessageReceived += ProcessMessage;
        client.Init();

        yield return null;

        while (!client.IsOpen)
            yield return null;
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
