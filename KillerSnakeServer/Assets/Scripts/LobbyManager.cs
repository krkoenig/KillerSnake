using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using scMessage;

public class LobbyManager : MonoBehaviour
{

	private bool start = false;
	
	PlayerList playerList;
	
	public List<bool> ready = new List<bool> ();

	// Use this for initialization
	void Start ()
	{
		playerList = GameObject.Find ("PlayerList").GetComponent<PlayerList> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (startGame ()) {
			sendStart ();
			Application.LoadLevel ("GameScene");
		}
	}
	
	private bool startGame ()
	{
		foreach (bool r in ready) {
			if (!r) {
				return false;
			}
		}
		if (ready.Count == 0) {
			return false;
		} else {
			return true;
		}
	}

	private void sendStart ()
	{
		List<Connection> clients = Server.Instance.getClients ();
		for (int i = 0; i < clients.Count; i++) {
			Server.Instance.sendClientMessage (clients [i], buildLobbyMessage (true, i));
		}
	}
	
	public message receiveUpdates (message m)
	{	
		scObject lobby = m.getSCObject ("lobby");
		
		bool readied = lobby.getBool ("ready");
							
		Debug.Log (playerList.players.Count);

		int userID = 0;

		// Check if the player exists
		for (int i = 0; i < playerList.players.Count; i++) {
			if (lobby.getString ("username").Equals (playerList.players [i].username)) {
				ready [i] = readied;
				userID = i;
			}
		}

		// TODO: Worry about players leaving
		return buildLobbyMessage (false, userID);
	}
	
	private message buildLobbyMessage (bool start, int id)
	{
		message m = new message ("lobby");
		scObject lobby = new scObject ("lobby");
		lobby.addInt ("num_player", playerList.players.Count);
		
		lobby.addBool ("start", start);
		lobby.addInt ("start_id", id);
<<<<<<< HEAD
=======

		GameObject.Find ("PlayerList").GetComponent<PlayerList> ().startId = id;
>>>>>>> pr/6
		
		for (int i = 0; i < playerList.players.Count; i++) {
			lobby.addString (i + "_username", playerList.players [i].username);
		}
		
		m.addSCObject (lobby);
		return m;
	}
}
