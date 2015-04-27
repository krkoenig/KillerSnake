using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using scMessage;

public class loginScript : MonoBehaviour
{
    private int
        sPort = 3000, // server port
        pfrPort = 2999; // policy file request port

    private Socket
        cSock; // client socket

    private string
        ipAddress = "127.0.0.1"; // server ip address

    public bool
        connectedToServer = false;

    private List<message>
        incMessages = new List<message>();

    private static loginScript instance;
    public static loginScript Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        instance = this;
    }

    void OnGUI()
    {
        if (!connectedToServer)
        {
            if (GUI.Button(new Rect(0, 0, 100, 25), "Connect"))
            {
                connect();
            }
        }
        else
        {
            GUI.Label(new Rect(0, 0, 250, 25), "Connected to server!");
            if (GUI.Button(new Rect(275, 0, 100, 25), "Disconnect"))
            {
                OnApplicationQuit();
            }
        }
    }

    void connect()
    {
        try
        {
            // get policy if we are on the web or in editor
            if ((Application.platform == RuntimePlatform.WindowsWebPlayer) || (Application.platform == RuntimePlatform.WindowsEditor))
            {
                Security.PrefetchSocketPolicy(ipAddress, pfrPort);
            }

            cSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            cSock.Connect(new IPEndPoint(IPAddress.Parse(ipAddress), sPort));
            clientConnection gsCon = new clientConnection(cSock);
        }
        catch
        {
            Debug.Log("Unable to connect to server.");
        }
    }

    public void onConnect()
    {
        connectedToServer = true;

        // test the connection
        message testMessage = new message("test");
        SendServerMessage(testMessage);
    }

    private void OnApplicationQuit()
    {
        try { cSock.Close(); }
        catch { }
    }

    public void addServerMessageToQue(message msg)
    {
        incMessages.Add(msg);
    }

    void Update()
    {
        if (incMessages.Count > 0)
        {
            doMessages();
        }
    }

    private void doMessages()
    {
        // do messages
        List<message> completedMessages = new List<message>();
        for (int i = 0; i < incMessages.Count; i++)
        {
            try
            {
                handleData(incMessages[i]);
                completedMessages.Add(incMessages[i]);
            }
            catch { }
        }
        
        // delete completed messages
        for (int i = 0; i < completedMessages.Count; i++)
        {
            try
            {
                incMessages.Remove(completedMessages[i]);
            }
            catch { }
        }
    }

    private void handleData(message mess)
    {
        Debug.Log("The server sent a message: " + mess.messageText);
    }

    public void SendServerMessage(message mes)
    {
        if (connectedToServer)
        {
            try
            {
                // convert message into a byte array, wrap the message with framing
                byte[] messageObject = conversionTools.convertObjectToBytes(mes);
                byte[] readyMessage = conversionTools.wrapMessage(messageObject);

                // send completed message
                cSock.Send(readyMessage);
            }
            catch
            {
                Debug.Log("There was an error sending server message " + mes.messageText);
            }
        }
    }
}