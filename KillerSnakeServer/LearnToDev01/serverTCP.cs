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
            clientPort = 3000;

        private static Socket
            clientListenSocket;

        private static List<Connection>
            clients = new List<Connection>();

        public serverTCP()
        {
            try
            {
                // listen for clients
                clientListenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientListenSocket.Bind(new IPEndPoint(IPAddress.Any, clientPort));
                clientListenSocket.Listen(int.MaxValue);
                ThreadPool.QueueUserWorkItem(new WaitCallback(listenForClients));

                output.outToScreen("Waiting for clients on port " + clientPort);
            }
            catch { }
        }

        private void listenForClients(object x)
        {
            while (serverMain.keepAlive)
            {
                Socket cSocket = clientListenSocket.Accept();
                Connection newCon = new Connection(cSocket, this);
            }
        }

        public void handleClientData(Socket cSock, message incObject)
        {
            output.outToScreen(incObject.messageText);

            string command = incObject.getSCObject("head").getString("command");
            message m;
            if (command.Equals("login"))
            {
                m = User.login(incObject);
            }
            else if (command.Equals("register"))
            {
                m = User.register(incObject);
            } else {
                m = new message("invalid");
            }
            sendClientMessage(cSock, m);
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
