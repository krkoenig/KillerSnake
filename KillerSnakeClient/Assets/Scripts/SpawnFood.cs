using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using scMessage;

public class SpawnFood : MonoBehaviour {
	//food prefab
	public GameObject apple;
	public GameObject onion;
	public GameObject rat;

	PlayerList playerList;
	// Use this for initialization
	void Start () {

	//	playerList = GameObject.Find ("PlayerList").GetComponent<PlayerList> ();
	//	InvokeRepeating ("Spawn", 3, 4);
	
	}

	//spawn the food
	void Spawn(int type, int x, int y) {
		if (type == 1) {
			Instantiate (apple, new Vector2 (x, y),
			             Quaternion.identity);
		} else if (type == 2) {
			Instantiate (onion, new Vector2 (x, y),
			             Quaternion.identity);
		} else if (type == 3){
			Instantiate (rat, new Vector2 (x, y),
			             Quaternion.identity);
		}

	}

	public void receiveUpdates (message m)
	{
		scObject foodInfo = m.getSCObject ("foodInfo");
		int type = foodInfo.getInt ("type");
		int x = foodInfo.getInt ("xPos");
		int y = foodInfo.getInt ("yPos");
		Spawn (type, x, y);
	}

}
