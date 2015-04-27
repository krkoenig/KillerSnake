using UnityEngine;
using System;
using System.Collections;

// For password security
using System.Security.Cryptography;
using System.Text;

// For connecting to the database
using MySql.Data.MySqlClient;

public class Login : MonoBehaviour
{

		private string username;
		private string password;

		// Security parameters
		HashAlgorithm HashProvider;
		private const int SALT_LENGTH = 4;

		// Database information


		// Use this for initialization
		void Start ()
		{
				HashProvider = new SHA256Managed ();

				username = "";
				password = "";
		}
	
		// Update is called once per frame
		void Update ()
		{
	
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
						// login...
				}
		}

		private void checkLoginWithDatabase ()
		{
				MySqlConnection myConnection = new MySqlConnection ();
				myConnection.ConnectionString = "Server=localhost;Database=killer_snake;port=3306;User ID=root;";
				myConnection.Open ();

				Debug.Log (myConnection.Database);

				myConnection.Close ();
		}
}
