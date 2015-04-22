using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Snake : MonoBehaviour
{

		// The parts of a snake are made up of prefab parts
		// Last() is the tail
		private List<Transform> segments;

		private List<Vector3> posQueue;
		private List<Quaternion> rotQueue;

		// Prefab for the body parts
		public GameObject body;
		public GameObject tail;

		// The speed of the snake in ms
		// intially 0.1
		private float speed;

		// Rotation direction
		private Quaternion dir;

		// Helps simulate responsiveness
		private Quaternion lastRotation;

		// Bools for food eating
		private bool apple;
		private bool onion;
		private bool mouse;

		// Use this for initialization
		void Start ()
		{
				dir = Quaternion.identity;

				// Sets the snake to start in the center
				// TODO: Once multiplayer, the player number will matter
				transform.position = new Vector3 (0, 0);
				transform.rotation = dir;

				segments = new List<Transform> ();

				posQueue = new List<Vector3> ();
				rotQueue = new List<Quaternion> ();

				calcSpeed ();
				// Start with one part
				InvokeRepeating ("grow", 1.0f, 0.5f);

				move ();
		}
	
		// Update is called once per frame
		void Update ()
		{
				input ();
		}

		private void move ()
		{
				if (segments.Count > 0) {
						for (int i = segments.Count - 1; i > 0; i--) {
								segments [i].position = segments [i - 1].position;
								segments [i].rotation = segments [i - 1].rotation;
						}

						segments [0].position = transform.position;
						segments [0].rotation = transform.rotation;
				}

				lastRotation = transform.rotation;
				transform.Translate (Vector2.right);
	
				Invoke ("move", speed);
		}

		public void grow ()
		{
				Vector3 pos = transform.position;
				Quaternion rot = transform.rotation;
				if (segments.Count > 0) {
						pos = segments [segments.Count - 1].transform.position;
						rot = segments [segments.Count - 1].transform.rotation;
				}
				
				GameObject newSegment = (GameObject)Instantiate (body, pos - rot * new Vector3 (1, 0, 0), rot);

				segments.Add (newSegment.transform);

				calcSpeed ();
		}


		private void calcSpeed ()
		{
				speed = 0.1f + 0.005f * segments.Count;
		
				if (speed > 0.20f) {
						speed = 0.20f;
				}
		}

		private void input ()
		{
				// Use the given input for compatability and easier customization
				float horizontal = Input.GetAxisRaw ("Horizontal");
				float vertical = Input.GetAxisRaw ("Vertical");

				dir = transform.rotation;
				// Snake can not move in the oppisite direction that it is moving
				if (horizontal == -1 && !Mathf.Approximately (lastRotation.eulerAngles.z, 0)) {			// LEFT
						dir.eulerAngles = new Vector3 (0, 0, 180);
				} else if (horizontal == 1 && !Mathf.Approximately (lastRotation.eulerAngles.z, 180)) {	// RIGHT
						dir.eulerAngles = new Vector3 (0, 0, 0);	
				} else if (vertical == -1 && !Mathf.Approximately (lastRotation.eulerAngles.z, 90)) {		// DOWN
						dir.eulerAngles = new Vector3 (0, 0, 270);	
				} else if (vertical == 1 && !Mathf.Approximately (lastRotation.eulerAngles.z, 270)) {		// UP
						dir.eulerAngles = new Vector3 (0, 0, 90);
				}

				transform.rotation = dir;
		}
}
