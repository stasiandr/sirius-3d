using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Scripts.Networking;

public class ClientProcessing : MonoBehaviour
{
    public Client client = new Client();

    public void ProcessMessage(string message)
    {
        Debug.Log(message);
    }

    IEnumerator Start()
    {
        client.MessageReceived += ProcessMessage;
        client.Init();

        yield return null;

        while (!client.IsOpen)
            yield return null;


        client.SendRequest("heh");
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
