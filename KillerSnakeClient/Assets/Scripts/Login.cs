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

public class Login : MonoBehaviour
{

	private string username;
	private string password;
	
	// Use this for initialization
	void Start ()
	{
		username = "";
		password = "";
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
			sendLogin ();
		}
		if (GUI.Button (new Rect (centerX + 25, centerY + 60, 100, 25), "Register")) {
			// Load the register scene			
			Application.LoadLevel ("RegisterScene");
		}
	}

	private void sendLogin ()
	{
		message m = new message ("Login request");
		scObject head = new scObject ("head");
		head.addString ("command", "login");
		head.addString ("username", username);
		head.addString ("password", password);
		m.addSCObject (head);
		Client.Instance.SendServerMessage (m);
	}
	
	public void loginResponse (message m)
	{
		if (m.getSCObject ("head").getBool ("success")) {
			Application.LoadLevel ("GameScene");
		}
	}
}
