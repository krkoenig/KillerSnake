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
	}
	
	// Update is called once per frame
	void Update ()
	{

	}
	
	public message receiveUpdates (message m)
	{
		scObject snake = m.getSCObject ("snake");
				
		foreach (Player p in playerList.players) {
			if (p.username.Equals (snake.getString ("username"))) {
				p.snake.scObjectToSnake (snake);
			}
		}
		return buildMessage ();
	}
	
	private message buildMessage ()
	{
		message m = new message ("game");
		scObject information = new scObject ("header");
		information.addInt ("num_snake", playerList.players.Count);
		m.addSCObject (information);
		
		for (int i = 0; i < playerList.players.Count; i++) {
			m.addSCObject (playerList.players [i].snake.snakeToSCObject ());
		}
		return m;
	}
}
