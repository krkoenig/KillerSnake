using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using scMessage;

// For password security
using System.Security.Cryptography;
using System.Text;

// For connecting to the database
using Mono.Data.SqliteClient;
using System.Data;

public class Login : MonoBehaviour
{

	private string username;
	private string password;

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
		incMessages = new List<message> ();

	private static Login instance;
	public static Login Instance {
		get {
			return instance;
		}
	}

	void Awake ()
	{
		instance = this;
	}

	// Use this for initialization
	void Start ()
	{
		username = "";
		password = "";
		connect ();

	}

	void OnGUI ()
	{
		float centerX = Screen.width / 2;
		float centerY = Screen.height / 2;

		// Make a background box
		GUI.Box (new Rect (centerX - 150, centerY - 100, 300, 200), "Login");

		GUI.Box (new Rect (centerX - 125, centerY - 15, 100, 25), "Username:");
		username = GUI.TextField (new Rect (centerX - 25, centerY - 15, 150, 25), username, 20);
		GUI.Box (new Rect (centerX - 125, centerY + 15, 100, 25), "Password:");
		password = GUI.PasswordField (new Rect (centerX - 25, centerY + 15, 150, 25), password, '*', 20);

		if (GUI.Button (new Rect (centerX - 125, centerY + 60, 100, 25), "Login") && !username.Equals ("") && !password.Equals ("")) {
			checkLoginWithDatabase ();
		}
		if (GUI.Button (new Rect (centerX + 25, centerY + 60, 100, 25), "Register")) {
			// Load the register scene			
			Application.LoadLevel (1);
		}
	}

	private void checkLoginWithDatabase ()
	{
		SqliteConnection myConnection = new SqliteConnection ();
		myConnection.ConnectionString = "URI=file:" + Application.dataPath + "/killer_snake.db";
		myConnection.Open ();

		string query = "SELECT * FROM users WHERE username = '" + username.ToUpper () + "';";
		SqliteCommand cmd = new SqliteCommand (query, myConnection);
		SqliteDataReader rdr = cmd.ExecuteReader ();

		string dbHash = "";
		string salt = "";
		if (rdr.Read ()) {
			dbHash = rdr.GetString (1);
			salt = rdr.GetString (2);
		}
				


		if (dbHash.Equals (password)) {
			Application.LoadLevel (2);
		}

		rdr.Close ();
		myConnection.Close ();
	}

	private void connect ()
	{
		try {
			// get policy if we are on the web or in editor
			if ((Application.platform == RuntimePlatform.WindowsWebPlayer) || (Application.platform == RuntimePlatform.WindowsEditor)) {
				Security.PrefetchSocketPolicy (ipAddress, pfrPort);
			}
			
			cSock = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			cSock.Connect (new IPEndPoint (IPAddress.Parse (ipAddress), sPort));
			clientConnection gsCon = new clientConnection (cSock);
		} catch {
			Debug.Log ("Unable to connect to server.");
		}
	}

	public void onConnect ()
	{
		connectedToServer = true;
		
		// test the connection
		message testMessage = new message ("LOGIN");
		SendServerMessage (testMessage);
	}
	
	private void OnApplicationQuit ()
	{
		try {
			cSock.Close ();
		} catch {
		}
	}
	
	public void addServerMessageToQue (message msg)
	{
		incMessages.Add (msg);
	}
	
	void Update ()
	{
		if (incMessages.Count > 0) {
			doMessages ();
		}
	}
	
	private void doMessages ()
	{
		// do messages
		List<message> completedMessages = new List<message> ();
		for (int i = 0; i < incMessages.Count; i++) {
			try {
				handleData (incMessages [i]);
				completedMessages.Add (incMessages [i]);
			} catch {
			}
		}
		
		// delete completed messages
		for (int i = 0; i < completedMessages.Count; i++) {
			try {
				incMessages.Remove (completedMessages [i]);
			} catch {
			}
		}
	}
	
	private void handleData (message mess)
	{
		Debug.Log ("The server sent a message: " + mess.messageText);
	}
	
	public void SendServerMessage (message mes)
	{
		if (connectedToServer) {
			try {
				// convert message into a byte array, wrap the message with framing
				byte[] messageObject = conversionTools.convertObjectToBytes (mes);
				byte[] readyMessage = conversionTools.wrapMessage (messageObject);
				
				// send completed message
				cSock.Send (readyMessage);
			} catch {
				Debug.Log ("There was an error sending server message " + mes.messageText);
			}
		}
	}
}
