using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Mono.Data.Sqlite; 
using System.Data;
using scMessage;
public class User : MonoBehaviour {

	private const string CONNECTION_STRING = "Data Source=db/killer_snake.s3db;";
	
	public static message login(message inc)
	{
		string username = inc.getSCObject("head").getString("username");
		string password = inc.getSCObject("head").getString("password");
		
		string hash = getHashed(password);
		
		bool exists = isInDatabase(username, hash);
		
		// Build the message
		message m = new message("login response");
		scObject head = new scObject("head");
		head.addString("command", "login");
		head.addBool("success", exists);
		m.addSCObject(head);
		return m;
	}
	
	public static message register(message inc)
	{
		string username = inc.getSCObject("head").getString("username");
		string password = inc.getSCObject("head").getString("password");
		
		string hash = getHashed(password);
		
		bool success = isNewUser(username, hash);
		
		// Build the message
		message m = new message("register response");
		scObject head = new scObject("head");
		head.addString("command", "register");
		head.addBool("success", success);
		m.addSCObject(head);
		return m;
	}
	
	private static string getHashed(string pass)
	{
		MD5 encrypt = new MD5CryptoServiceProvider();
		encrypt.ComputeHash(ASCIIEncoding.ASCII.GetBytes(pass));
		byte[] hash = encrypt.Hash;
		StringBuilder strBuilder = new StringBuilder();
		for(int i = 0; i < hash.Length; i++)
		{
			strBuilder.Append(hash[i].ToString("x2"));
		}
		
		return strBuilder.ToString();
	}
	
	private static bool isInDatabase(string username, string hash)
	{
		Mono.Data.Sqlite.SqliteConnection myConnection = new Mono.Data.Sqlite.SqliteConnection();
		myConnection.ConnectionString = CONNECTION_STRING;
		myConnection.Open();
		string query = "SELECT * FROM users WHERE username = '" + username.ToLower() + "';";
		Mono.Data.Sqlite.SqliteCommand cmd = new Mono.Data.Sqlite.SqliteCommand(query, myConnection);
		Mono.Data.Sqlite.SqliteDataReader rdr = cmd.ExecuteReader();
		
		string dbHash = "";
		if (rdr.Read())
		{
			dbHash = rdr.GetString(1);
		}
		
		rdr.Close();
		myConnection.Close();
		
		return hash.Equals(dbHash);
	}
	
	private static bool isNewUser(string username, string hash)
	{
		// Open a new connection
		Mono.Data.Sqlite.SqliteConnection myConnection = new Mono.Data.Sqlite.SqliteConnection();
		myConnection.ConnectionString = CONNECTION_STRING;
		myConnection.Open();
		
		// Grab users that have the same username as the one provided
		string query = "SELECT * FROM users WHERE username = '" + username.ToLower() + "'";
		Mono.Data.Sqlite.SqliteCommand cmd = new Mono.Data.Sqlite.SqliteCommand(query, myConnection);
		Mono.Data.Sqlite.SqliteDataReader rdr = cmd.ExecuteReader();
		
		// The user is old if the query brought back a reply
		if(rdr.HasRows)
		{
			rdr.Close();
			return false;
		}
		
		rdr.Close();
		
		query = "INSERT INTO users VALUES('" + username.ToLower() + "','" + hash + "');";
		cmd.CommandText = query;
		cmd.ExecuteNonQuery();
		
		myConnection.Close();
		return true;
		
	}
}
