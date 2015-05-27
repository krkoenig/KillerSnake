using UnityEngine;
using System.Collections;
using scMessage;
using System.Linq;
public class spwanfood: MonoBehaviour {


	public int borderLeft = 0;
	public int borderRight = 800;
	public int borderBottom = 0;
	public int borderTop = 600;

	// TODO need border information from server
	public Pair<int,int> Start (scObject s) {
		Pair<int,int> food;
		// init food coodinates
		int x = (int)Random.Range(borderLeft,
		                               borderRight);

		int y = (int)Random.Range(borderBottom,
		                                borderTop);
		// create arrays for segments
		float[] xlist;
		float[] ylist;
		int numSeg = s.getInt ("segments");
		for (int i = 0; i < numSeg; i++) {
			xlist[i] = s.getFloat (i + "_xPos");
			ylist[i] = s.getFloat (i + "_yPos");
		};

		while (xlist.Contains((float)x) ) {
			x = (int)Random.Range(borderLeft,
			                      borderRight);
		};

		while (ylist.Contains((float)y)) {
			y = (int)Random.Range(borderLeft,
			                      borderRight);
		};

		food.First = x;
		food.Second = y;

		return food;
	}
}
