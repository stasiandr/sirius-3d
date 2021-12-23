using System;
using System.Text;
using System.Collections.Generic;
using HybridWebSocket;
using UnityEngine;

namespace Core.Scripts.Networking
{
    public class Client
    {
        public event Action<string> MessageReceived;
        public event Action WebSocketsConnected;
        public event Action WebSocketsDisconnectedAbnormally;
        public bool IsOpen => _webSocket != null && _webSocket.GetState() == WebSocketState.Open;

        private WebSocket _webSocket;
        private bool _endFlag;
        private bool _onCloseAbnormally = false;

        private readonly Queue<string> _queue = new Queue<string>();

        private bool _initializedFlag;

        public void Init(string IP)
        {
            if (_initializedFlag)
                return;

            StartClient(IP);

            _initializedFlag = true;
        }

        private void StartClient(string IP)
        {
            _webSocket = WebSocketFactory.CreateInstance(IP + ":8123");

            _webSocket.OnMessage += WebSocketOnMessage;
            _webSocket.OnOpen += () => WebSocketsConnected?.Invoke();

            _webSocket.OnError += Debug.LogError;
            _webSocket.OnClose += WebSocketOnClose;
            _webSocket.Connect();
        }


        private void WebSocketOnClose(WebSocketCloseCode code)
        {
            if (code == WebSocketCloseCode.Normal)
                Debug.Log("Websockets closed normally");
            else
            {
                Debug.LogError($"Websockets closed with code {code}");
                _onCloseAbnormally = true;
            }
        }

        private void WebSocketOnMessage(byte[] msg)
        {
            string jsonAnswer = Encoding.UTF8.GetString(msg);
            _queue.Enqueue(jsonAnswer);

         //   Debug.Log(jsonAnswer);
        }

        public void SendRequest(string request)
        {
            if (_webSocket.GetState() != WebSocketState.Open)
                throw new Exception("WebSocket is not open");

            _webSocket.Send(Encoding.UTF8.GetBytes(request));

     //       Debug.Log(request);
        }

        public void Quit()
        {
            _webSocket.OnMessage -= WebSocketOnMessage;
            _webSocket.OnClose -= WebSocketOnClose;
            _initializedFlag = false;
            _webSocket.Close();
        }

        public void UpdateState()
        {
            if (_queue.Count > 0) 
            {
                string str = _queue.Dequeue();
               // Debug.Log(str);
                MessageReceived?.Invoke(str); 
            }

            if (_onCloseAbnormally)
                WebSocketsDisconnectedAbnormally?.Invoke();
        }
    }
}
