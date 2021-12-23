using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Scripts.Networking;
using SceneProvider;
using Commands;
using Newtonsoft.Json.Linq;

public class ClientProcessing
{
    public static Client client = new Client();

    public static void ProcessMessage(string message)
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

    public static IEnumerator Activate(string str)
    {
        Debug.Log("HEH");
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
