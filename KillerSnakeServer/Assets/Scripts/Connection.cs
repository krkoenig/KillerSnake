using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using scMessage;

class Connection
{
	public Socket socket;
	public bool isclose = true;
	public string clientName;
	private int MAX_INC_DATA = 512000; // half a megabyte
	
	public Connection (Socket s)
	{
		socket = s;
		ThreadPool.QueueUserWorkItem (new WaitCallback (handleConnection));
	}

	public void setName(string n)
	{
		clientName = n;
	}

	public void handleConnection (object x)
	{
		// broadcast new connection
		Server.Instance.print ("A client connected from the IP address: " + socket.RemoteEndPoint.ToString ());
		isclose = false;
		while (true) {
			byte[] sizeInfo = new byte[4];

			int bytesRead = 0,
			currentRead = 0;
			
			try {
				currentRead = bytesRead = socket.Receive (sizeInfo);
			} catch {
				break;
			}
			
			while (bytesRead < sizeInfo.Length && currentRead > 0) {
				currentRead =
                            socket.Receive
                            (
                                sizeInfo, // message frame, size of incoming message
                                bytesRead, // offset into the buffer
                                sizeInfo.Length - bytesRead, // max amount to read
                                SocketFlags.None // no socket flags
				);

				bytesRead += currentRead;
			}

			// get the message size of incoming message
			int messageSize = BitConverter.ToInt32 (sizeInfo, 0);

			// create a byte array with the correct message size
			byte[] incMessage = new byte[messageSize];

			// begin reading message
			bytesRead = 0; // reset to ensure proper byte read count

			currentRead =
                        bytesRead =
                        socket.Receive
                        (
                            incMessage, // incoming message
                            bytesRead,
                            incMessage.Length - bytesRead,
                            SocketFlags.None
			);

			// check to see if we received all data
			while (bytesRead < messageSize && currentRead > 0) {
				currentRead =
                            socket.Receive
                            (
                                incMessage, // incoming message
                                bytesRead,
                                incMessage.Length - bytesRead,
                                SocketFlags.None
				);
				bytesRead += currentRead;
			}

			// all data received, continue
			message incObject = (message)conversionTools.convertBytesToObject (incMessage);

			if (incObject != null) {
				// send data to handler
				Server.Instance.addClientMessagetoQueue (this, incObject);
			}
		}

		Server.Instance.print ("The client disconnected from IP address: " + socket.RemoteEndPoint.ToString ());
		socket.Close ();
		isclose = true;
	}
}