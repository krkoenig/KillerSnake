using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using scMessage;

public class clientConnection
{
    public Socket sSock;
    private int MAX_INC_DATA = 512000;

    public clientConnection(Socket s)
    {
        sSock = s;
        ThreadPool.QueueUserWorkItem(new WaitCallback(HandleConnection));
    }

    public void HandleConnection(object state)
    {
        Debug.Log("Connected to server.");
        loginScript.Instance.onConnect();

        try
        {
            while (sSock.Connected)
            {
                byte[] sizeinfo = new byte[4];

                int bytesRead = 0, currentread = 0;

                currentread = bytesRead = sSock.Receive(sizeinfo);

                while (bytesRead < sizeinfo.Length && currentread > 0)
                {
                    currentread = sSock.Receive(sizeinfo, bytesRead, sizeinfo.Length - bytesRead, SocketFlags.None);
                    bytesRead += currentread;
                }

                int messagesize = BitConverter.ToInt32(sizeinfo, 0);
                byte[] message = new byte[messagesize];

                bytesRead = 0;
                currentread = bytesRead = sSock.Receive(message, bytesRead, message.Length - bytesRead, SocketFlags.None);

                while (bytesRead < messagesize && currentread > 0)
                {
                    currentread = sSock.Receive(message, bytesRead, message.Length - bytesRead, SocketFlags.None);
                    bytesRead += currentread;
                }

                try
                {
                    message incObject = (message)conversionTools.convertBytesToObject(message);

                    if (incObject != null)
                    {
                        loginScript.Instance.addServerMessageToQue(incObject);
                    }
                }
                catch (Exception er)
                {
                    Debug.Log("There was an error trying to get data: " + er.ToString());
                }
            }
        }
        catch { }

        Debug.Log("Disconnected from server.");
        loginScript.Instance.connectedToServer = false;
        sSock.Close();
    }
}