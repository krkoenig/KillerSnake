using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using scMessage;

public class Connection
{
	public Socket socket;
	private int MAX_INC_DATA = 512000;

	public Connection (Socket s)
	{
		socket = s;
		ThreadPool.QueueUserWorkItem (new WaitCallback (handleConnection));
	}

	public void handleConnection (object x)
	{
		Debug.Log ("Connected to server.");

		while (true) {
			byte[] sizeInfo = new byte[4];

			int bytesRead = 0, currentRead = 0;
			try {
				currentRead = bytesRead = socket.Receive (sizeInfo);
			} catch {
				break;
			}
									
			while (bytesRead < sizeInfo.Length && currentRead > 0) {
				currentRead = socket.Receive (sizeInfo, bytesRead, sizeInfo.Length - bytesRead, SocketFlags.None);
				bytesRead += currentRead;
			}

			int messagesize = BitConverter.ToInt32 (sizeInfo, 0);
			byte[] message = new byte[messagesize];			

			bytesRead = 0;
			currentRead = bytesRead = socket.Receive (message, bytesRead, message.Length - bytesRead, SocketFlags.None);

			while (bytesRead < messagesize && currentRead > 0) {
				currentRead = socket.Receive (message, bytesRead, message.Length - bytesRead, SocketFlags.None);
				bytesRead += currentRead;
			}
						
			message incObject = (message)conversionTools.convertBytesToObject (message);
			
			if (incObject != null) {
				
				Client.Instance.addServerMessageToQueue (incObject);
			}
		}
		
		Debug.Log ("Disconnected from server.");
		Client.Instance.connectedToServer = false;
		socket.Close ();
	}
}