using UnityEngine;
using System;

public class Player
{
	private GameObject snakeObject;
	public Snake snake;
	
	public string username;
	
	public Player (string u)
	{
		username = u;
	}
	
	public void startSnake ()
	{
		snakeObject = (GameObject)GameObject.Instantiate (Resources.Load ("Snake"));
		snake = snakeObject.GetComponent<Snake> ();
	}
}

