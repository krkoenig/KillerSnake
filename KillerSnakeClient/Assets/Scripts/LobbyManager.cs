using UnityEngine;
using System.Collections;
using scMessage;

public class LobbyManager : MonoBehaviour
{

	private bool readied = false;
	private bool start = false;
	
	PlayerList playerList;

	// Use this for initialization
	void Start ()
	{
		playerList = GameObject.Find ("PlayerList").GetComponent<PlayerList> ();
		
		InvokeRepeating ("sendUpdates", 0.0f, 0.1f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (start) {
			Application.LoadLevel ("GameScene");
		}
	}
	
	void OnGUI ()
	{
		string btnText = "Ready";
		
		if (readied) {
			btnText = "Unready";
		}
		
		if (GUI.Button (new Rect (0, 0, 150, 50), btnText)) {
			readied = !readied;
		}
	
	}
	
	// Send only the snake of the user.
	private void sendUpdates ()
	{
		message m = new message ("lobby");
		
		scObject lobby = new scObject ("lobby");
		lobby.addString ("username", Client.Instance.username);
		lobby.addBool ("ready", readied);
		m.addSCObject (lobby);
		
		Client.Instance.SendServerMessage (m);
	}
	
	public void receiveUpdates (message m)
	{	
		scObject lobby = m.getSCObject ("lobby");
		
		start = lobby.getBool ("start");
		
		int numPlayers = lobby.getInt ("num_player");
		Debug.Log (numPlayers);
		// Check if there is a new player
		for (int i = 0; i < numPlayers; i++) {
			if (!lobby.getString (i + "_username").Equals (Client.Instance.username)) {
				bool newPlayer = true;
				Debug.Log ("Doing stuff");
				// Check if the player exists
				foreach (Player s in playerList.players) {
					if (lobby.getString (i + "_username").Equals (s.username)) {
						newPlayer = false;
					}
				}
				
				if (newPlayer) {
					Debug.Log ("adding");
					playerList.addPlayer (lobby.getString (i + "_username"));
				}
			}
		}

		// TODO: Worry about players leaving

	}
}
