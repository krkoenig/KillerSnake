using UnityEngine;
using System.Collections;

using scMessage;

public class Register : MonoBehaviour
{
	string username;
	string password;
	string confirm;
	string warning;

	// Use this for initialization
	void Start ()
	{
		username = "";
		password = "";
		confirm = "";
		warning = "";
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnGUI ()
	{
		float centerX = Screen.width / 2;
		float centerY = Screen.height / 2;

		GUIStyle center = new GUIStyle (GUI.skin.GetStyle ("Label"));
		center.alignment = TextAnchor.UpperCenter;
		center.normal.textColor = Color.white;

		GUIStyle centerRed = new GUIStyle ();
		centerRed.alignment = TextAnchor.UpperCenter;
		centerRed.normal.textColor = Color.red;
				
		// Make a background box
		GUI.Box (new Rect (centerX - 150, centerY - 150, 300, 300), "Register");
		GUI.Label (new Rect (centerX - 150, centerY - 125, 290, 120), warning, centerRed);
		GUI.Label (new Rect (centerX - 125, centerY - 15, 100, 25), "Username:", center);
		username = GUI.TextField (new Rect (centerX - 25, centerY - 15, 150, 25), username, 20);
		GUI.Label (new Rect (centerX - 125, centerY + 15, 100, 25), "Password:", center);
		password = GUI.PasswordField (new Rect (centerX - 25, centerY + 15, 150, 25), password, '*', 20);
		GUI.Label (new Rect (centerX - 125, centerY + 45, 100, 40), "Confirm\nPassword:", center);
		confirm = GUI.PasswordField (new Rect (centerX - 25, centerY + 57, 150, 25), confirm, '*', 20);

		if (GUI.Button (new Rect (centerX - 50, centerY + 95, 100, 25), "Register")) {
			if (password.Length < 6) {
				warning = "Password must be 6 character!";
			} else if (!password.Equals (confirm)) {
				warning = "Passwords don't match!";
			} else {
				Client.Instance.connect ();
				sendRegister ();
			}

		}
	}

	private void sendRegister ()
	{
		message m = new message ("register");
		scObject head = new scObject ("register");
		head.addString ("username", username);
		head.addString ("password", password);
		m.addSCObject (head);
		Client.Instance.SendServerMessage (m);
	}

	public void registerResponse (message m)
	{
		if (m.getSCObject ("register").getBool ("success")) {
			Application.LoadLevel ("LoginScene");
		} else {
			warning = "Username is already taken!";
		}
	}
}