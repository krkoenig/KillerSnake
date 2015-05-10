using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using scMessage;

public class GameManager : MonoBehaviour
{		
	PlayerList playerList;
	
	// Use this for initialization
	void Start ()
	{
		playerList = GameObject.Find ("PlayerList").GetComponent<PlayerList> ();
		
		foreach (Player p in playerList.players) {
			p.startSnake ();
		}
		
		InvokeRepeating ("sendUpdates", 0.0f, 0.1f);
	}
	
	// Update is called once per frame
	void Update ()
	{

	}
	
	// Send only the snake of the user.
	private void sendUpdates ()
	{
		message m = new message ("game");
		
		m.addSCObject (GameObject.Find ("UserSnake").GetComponent<UserSnake> ().snakeToSCObject ());
						
		Client.Instance.SendServerMessage (m);
	}
	
	public void receiveUpdates (message m)
	{
		scObject information = m.getSCObject ("header");
		
		int numSnakes = information.getInt ("num_snake");		
		
		for (int i = 0; i < numSnakes - 1; i++) {
			playerList.players [i].snake.scObjectToSnake (m.getSCObject (playerList.players [i].username + "_snake"));
		}
	}
}
