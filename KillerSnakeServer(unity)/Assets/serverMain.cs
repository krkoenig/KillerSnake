using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class serverMain : MonoBehaviour {

	public static bool keepAlive = true;
	
	static void Main()
	{
		// start server
		serverTCP startTCP = new serverTCP();
		
		// keep alive
		while (keepAlive)
		{
			if (Console.ReadLine().ToLower() == "exit")
			{
				keepAlive = false;
			}
		}
	}
}
