using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using scMessage;

namespace LearnToDev01
{
    class serverTCP
    {
        private static int
            clientPort = 3000, policyFilePort = 2999;

        private static Socket
            policyFileListenSocket, clientListenSocket;

        private static List<clientConnection>
            clients = new List<clientConnection>();

        public serverTCP()
        {
            try
            {
                // listen for policy requests
                policyFileListenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                policyFileListenSocket.Bind(new IPEndPoint(IPAddress.Any, policyFilePort));
                policyFileListenSocket.Listen(int.MaxValue);
                ThreadPool.QueueUserWorkItem(new WaitCallback(listenForPFR));

                // listen for clients
                clientListenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientListenSocket.Bind(new IPEndPoint(IPAddress.Any, clientPort));
                clientListenSocket.Listen(int.MaxValue);
                ThreadPool.QueueUserWorkItem(new WaitCallback(listenForClients));

                output.outToScreen("Waiting for client policy file requests on port " + policyFilePort + " and clients on port " + clientPort);
            }
            catch { }
        }

        private void listenForPFR(object x)
        {
            while (serverMain.keepAlive)
            {
                Socket pfRequest = policyFileListenSocket.Accept();
                policyFileConnection newRequest = new policyFileConnection(pfRequest);
            }
        }

        private void listenForClients(object x)
        {
            while (serverMain.keepAlive)
            {
                Socket cSocket = clientListenSocket.Accept();
                clientConnection newCon = new clientConnection(cSocket, this);
            }
        }

        public void handleClientData(message incObject)
        {
            output.outToScreen("The client sent a message: " + incObject.messageText);
        }

        public void sendClientMessage(Socket cSock, message mes)
        {
            try
            {
                // convert message into a byte array, wrap the message, then send it
                byte[] messageObject = conversionTools.convertObjectToBytes(mes);
                byte[] readyToSend = conversionTools.wrapMessage(messageObject);
                cSock.Send(readyToSend);
            }
            catch { }
        }
    }
}
