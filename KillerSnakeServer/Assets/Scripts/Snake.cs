using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using scMessage;

public class Snake : MonoBehaviour
{

	// The parts of a snake are made up of prefab parts
	// Last() is the tail
	protected List<GameObject> segments = new List<GameObject> ();
	
	// Prefab for the body parts
	public GameObject body;
	public GameObject tail;
	
	public string username = "";
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public int length ()
	{
		return segments.Count;
	}
	
	public scObject snakeToSCObject ()
	{		
		// Segment for head
		scObject snake = new scObject (username + "_snake");
		snake.addString ("username", username);
		snake.addFloat ("xPos", transform.position.x);
		snake.addFloat ("yPos", transform.position.y);
		snake.addFloat ("zPos", transform.position.z);
		snake.addFloat ("xRot", transform.rotation.eulerAngles.x);
		snake.addFloat ("yRot", transform.rotation.eulerAngles.y);
		snake.addFloat ("zRot", transform.rotation.eulerAngles.z);
		snake.addInt ("segments", segments.Count);
		
		// New object for each segment
		for (int i = 0; i < segments.Count; i++) {
			snake.addFloat (i + "_xPos", segments [i].transform.position.x);
			snake.addFloat (i + "_yPos", segments [i].transform.position.y);
			snake.addFloat (i + "_zPos", segments [i].transform.position.z);
		}
		
		return snake;
	}
	
	public void scObjectToSnake (scObject s)
	{
		// unpack position of head
		Vector3 headPos = new Vector3 ();
		headPos.x = s.getFloat ("xPos");
		headPos.y = s.getFloat ("yPos");
		headPos.z = s.getFloat ("zPos");
		transform.position = headPos;
		
		// unpack rotation of head
		Vector3 headRot = new Vector3 ();
		headRot.x = s.getFloat ("xRot");
		headRot.y = s.getFloat ("yRot");
		headRot.z = s.getFloat ("zRot");
		transform.eulerAngles = headRot;
		
		// unpack segments
		int numSeg = s.getInt ("segments");
		
		if (numSeg < segments.Count) {
			foreach (GameObject go in segments) {
				Destroy (go);
			}
			
			segments.Clear ();
		}
		
		for (int i = 0; i < numSeg; i++) {
		
			Vector3 pos = new Vector3 ();
			pos.x = s.getFloat (i + "_xPos");
			pos.y = s.getFloat (i + "_yPos");
			pos.z = s.getFloat (i + "_zPos");
			if (i >= segments.Count) {
				GameObject newSegment = (GameObject)Instantiate (body, pos, Quaternion.identity);
				segments.Add (newSegment);
			} else {
				segments [i].transform.position = pos;
			}
		}
	}
}
