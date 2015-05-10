using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Mono.Data.Sqlite;
using scMessage;

class Database
{
	private const string CONNECTION_STRING = "URI=file:Assets/Database/killer_snake.s3db;";

	private SqliteConnection dbConnection;

	public Database ()
	{
		dbConnection = new SqliteConnection ();
		dbConnection.ConnectionString = CONNECTION_STRING;
		dbConnection.Open ();
	}
	
	~Database ()
	{
		dbConnection.Close ();
	}

	public message login (message inc)
	{
		string username = inc.getSCObject ("login").getString ("username");
		string password = inc.getSCObject ("login").getString ("password");
           
		string hash = getHashed (password);

		bool exists = isInDatabase (username, hash);
		
		if (exists) {
			GameObject.Find ("PlayerList").GetComponent<PlayerList> ().addPlayer (username);
			GameObject.Find ("LobbyManager").GetComponent<LobbyManager> ().ready.Add (false);
		}

		// Build the message
		message m = new message ("login");
		scObject head = new scObject ("login");
		head.addBool ("success", exists);
		m.addSCObject (head);
		return m;
	}

	public message register (message inc)
	{
		string username = inc.getSCObject ("register").getString ("username");
		string password = inc.getSCObject ("register").getString ("password");

		string hash = getHashed (password);

		bool success = isNewUser (username, hash);

		// Build the message
		message m = new message ("register");
		scObject head = new scObject ("register");
		head.addBool ("success", success);
		m.addSCObject (head);
		return m;
	}

	private static string getHashed (string pass)
	{
		MD5 encrypt = new MD5CryptoServiceProvider ();
		encrypt.ComputeHash (ASCIIEncoding.ASCII.GetBytes (pass));
		byte[] hash = encrypt.Hash;
		StringBuilder strBuilder = new StringBuilder ();
		for (int i = 0; i < hash.Length; i++) {
			strBuilder.Append (hash [i].ToString ("x2"));
		}

		return strBuilder.ToString ();
	}

	private bool isInDatabase (string username, string hash)
	{
		string query = "SELECT * FROM users WHERE username = '" + username.ToLower () + "';";
		SqliteCommand cmd = new SqliteCommand (query, dbConnection);
		SqliteDataReader rdr = cmd.ExecuteReader ();

		string dbHash = "";
		if (rdr.Read ()) {
			dbHash = rdr.GetString (1);
		}

		rdr.Close ();

		return hash.Equals (dbHash);
	}

	private bool isNewUser (string username, string hash)
	{
		// Grab users that have the same username as the one provided
		string query = "SELECT * FROM users WHERE username = '" + username.ToLower () + "'";
		SqliteCommand cmd = new SqliteCommand (query, dbConnection);
		SqliteDataReader rdr = cmd.ExecuteReader ();

		// The user is old if the query brought back a reply
		if (rdr.HasRows) {
			rdr.Close ();
			return false;
		}

		rdr.Close ();

		query = "INSERT INTO users VALUES('" + username.ToLower () + "','" + hash + "');";
		cmd.CommandText = query;
		cmd.ExecuteNonQuery ();

		return true;

	}

}
