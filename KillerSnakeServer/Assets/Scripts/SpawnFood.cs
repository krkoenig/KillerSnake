using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using scMessage;

public class SpawnFood : MonoBehaviour {
	//food prefab
	public GameObject apple;
	public GameObject onion;
	public GameObject rat;

	// Use this for initialization
	void Start () {
		InvokeRepeating ("Spawn", 3, 4);
	
	}

	//spawn the food
	void Spawn() {
		//x position between left & right border
		int x = (int)Random.Range (-30,30);

		//y position between top & bottom border
		int y = (int)Random.Range (-13,13);

		//instantiate the food
		int type = (int)Random.Range (1, 4);
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

		message m = new message ("food");
		scObject foodInfo = new scObject ("foodInfo");
		foodInfo.addInt ("type", type);
		foodInfo.addInt ("xPos", x);
		foodInfo.addInt ("yPos", y);
		m.addSCObject (foodInfo);

		List<Connection> clients = Server.Instance.getClients ();
		for (int i = 0; i < clients.Count; i++) {
			Server.Instance.sendClientMessage (clients[i],m);
		}
	}
}
