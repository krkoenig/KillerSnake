using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using scMessage;

class Server : MonoBehaviour
{
	private const int PORT = 10093;

	private Socket listener;

	public static Server Instance { get; private set; }
	
	private Queue<Pair<Connection,message>> incMessages = new Queue<Pair<Connection,message>> ();
				
	private List<Connection> clients = new List<Connection> ();
				
	private Database db = new Database ();
			
	private string output = "";
	
	private bool listening = true;
						
	void Awake ()
	{
		Instance = this;
		DontDestroyOnLoad (this);
	}

	void Start ()
	{
		listener = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		listener.Bind (new IPEndPoint (IPAddress.Any, PORT));
		listener.Listen (int.MaxValue);
		
		ThreadStart ts = new ThreadStart (Listen);
		Thread t = new Thread (ts);
		t.Start ();
	}
	
	private void Listen ()
	{
		while (listening) {
			clients.Add (new Connection (listener.Accept ()));
		}
	}
	
	void Update ()
	{
		for (int i = 0; i < incMessages.Count; i++) {
			Pair<Connection, message> p = incMessages.Dequeue ();
			sendClientMessage (p.First, handleData (p.Second));
		}
	}
	
	void OnApplicationQuit ()
	{
		listener.Close ();
	}
	
	void OnGUI ()
	{
		GUI.Box (new Rect (0, 0, Screen.width, Screen.height), output);
	}
	
	public void print (string s)
	{
		output = s;
	}
	
	public void addClientMessagetoQueue (Connection c, message msg)
	{
		incMessages.Enqueue (new Pair<Connection,message> (c, msg));
	}
	
	public message handleData (message msg)
	{
		print (msg.messageText);

		message m;
				
		if (msg.messageText.Equals ("login")) {
			m = db.login (msg);
		} else if (msg.messageText.Equals ("register")) {
			m = db.register (msg);
		} else if (msg.messageText.Equals ("lobby")) {
			m = GameObject.Find ("LobbyManager").GetComponent<LobbyManager> ().receiveUpdates (msg);
		} else if (msg.messageText.Equals ("game")) {
<<<<<<< HEAD
			m = GameObject.Find ("GameManager").GetComponent<GameManager> ().receiveUpdates (msg);
			
		} else {
=======
			m = GameObject.Find ("GameManager").GetComponent<GameManager> ().receiveUpdates (msg);			
		} else if (msg.messageText.Equals ("foodDestroy")) {
			m = GameObject.Find ("Main Camera").GetComponent<SpawnFood> ().receiveDestroy (msg); 
		}
		else {
>>>>>>> pr/6
			m = new message ("invalid");
		}
			
		return m;
	}

	public void sendClientMessage (Connection client, message mes)
	{
		try {
			// convert message into a byte array, wrap the message, then send it
			byte[] messageObject = conversionTools.convertObjectToBytes (mes);
			byte[] readyToSend = conversionTools.wrapMessage (messageObject);
			client.socket.Send (readyToSend);
		} catch {
		}
	}
	
	public List<Connection> getClients ()
	{
		return clients;
	}
}
