using UnityEngine;
using System.Collections;

public class SmoothCamera : MonoBehaviour
{
	
	public float dampTime = 0.15f;
	private Transform target;
	private Transform top;
	private Transform bottom;
	private Transform left;
	private Transform right;
	
	
	private float minX;
	private float maxX;
	private float minY;
	private float maxY;
	
	void Start ()
	{
		target = GameObject.Find ("UserSnake").transform;
		top = GameObject.Find ("BorderTop").transform;
		bottom = GameObject.Find ("BorderBottom").transform;
		left = GameObject.Find ("BorderLeft").transform;
		right = GameObject.Find ("BorderRight").transform;
	
		var vertExtent = GetComponent<Camera> ().orthographicSize;    
		var horzExtent = vertExtent * Screen.width / Screen.height;
		
		float mapY = top.position.y - bottom.position.y;
		float mapX = right.position.x - left.position.x;
		
		
		
		// Calculations assume map is position at the origin
		minX = horzExtent - mapX / 2.0f;
		maxX = mapX / 2.0f - horzExtent;
		minY = vertExtent - mapY / 2.0f;
		maxY = mapY / 2.0f - vertExtent;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (target) {
			Vector3 from = transform.position;
			Vector3 to = target.position;
			to.z = transform.position.z;
							
			transform.position -= (from - to) * dampTime * Time.deltaTime;
			
			Vector3 v3 = transform.position;
			v3.x = Mathf.Clamp (v3.x, minX, maxX);
			v3.y = Mathf.Clamp (v3.y, minY, maxY);
			transform.position = v3;
		}		
	}
}