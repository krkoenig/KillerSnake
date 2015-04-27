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
	
	Client client;

	// Use this for initialization
	void Start ()
	{
		username = "";
		password = "";
		Client.Instance.connect ();
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
			Client.Instance.connect ();
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
}
