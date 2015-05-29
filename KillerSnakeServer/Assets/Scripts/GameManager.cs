using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using scMessage;

public class GameManager : MonoBehaviour
{		
	PlayerList playerList;
	List<Pair<string,int>> scoreboard = new List<Pair<string, int>>();
	
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
		string names = "";
		string scs = "";
		foreach (Pair<string, int> sb in scoreboard) {
			names = names + sb.First + ":\n";
			scs = scs + sb.Second + "\n";
		}

		message sc = new message ("scoreboard");
		scObject info = new scObject ("info");
		info.addString("names", names);
		info.addString ("scs", scs);
		sc.addSCObject (info);

		List<Connection> clients = Server.Instance.getClients ();
		for (int i = 0; i < clients.Count; i++) {
			Server.Instance.sendClientMessage (clients[i],sc);
		}
	}

	static int SortByScore(Pair<string, int> p1, Pair<string, int> p2)
	{
		return p2.Second.CompareTo (p1.Second);
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

		//get the scoreboard list
		List<Pair<string,int>> tempboard = new List<Pair<string, int>>();

		foreach (Player p in playerList.players) {
			string name = p.username;
			int score = p.snake.length();
			tempboard.Add(new Pair<string, int> (name,score));
		}
		tempboard.Sort (SortByScore);
		scoreboard = tempboard;

		return m;
	}

}
