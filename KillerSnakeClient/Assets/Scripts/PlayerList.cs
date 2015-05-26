using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerList : MonoBehaviour
{
	public List<Player> players = new List<Player> ();

	public int startId;

	void Awake ()
	{
		DontDestroyOnLoad (this);
	}

	public void addPlayer (string u)
	{
		players.Add (new Player (u));
	}
	
	public void removePlayer (string u)
	{
		for (int i = 0; i < players.Count; i++) {
			if (players [i].username.Equals (u)) {
				players.RemoveAt (i);
			}
		}
	}
}
