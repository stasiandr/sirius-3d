using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Scripts.Networking;
using SceneProvider;
using Commands;

public class ClientProcessing : MonoBehaviour
{
    public static Client client = new Client();

    public void ProcessMessage(string message)
    {
        Debug.Log(message);
        SceneData.ExecutionQueue.Enqueue(TransformCommand.Deserialize(message));
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
