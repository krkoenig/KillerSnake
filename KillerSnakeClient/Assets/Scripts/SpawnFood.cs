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
		if (type == 1 || type == 2 || type == 3) {
			Instantiate (apple, new Vector2 (x, y),
			             Quaternion.identity);
		} else if (type == 4) {
			Instantiate (onion, new Vector2 (x, y),
			             Quaternion.identity);
		} else if (type == 5){
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

	public void receiveDestroy (message m)
	{
		scObject foodInfo = m.getSCObject ("foodInfo");
		float x = foodInfo.getFloat ("xPos");
		float y = foodInfo.getFloat ("yPos");

		Debug.Log ("get the destroy message");

		GameObject[] fs = GameObject.FindGameObjectsWithTag ("food");

		foreach (GameObject f in fs) {
			if(f.transform.position.x == x && f.transform.position.y == y)
				Destroy(f);
		}
	}

}
