using UnityEngine;
using System.Collections;

public class Leviathan : MonoBehaviour {

	// CLASS VARIABLES

	static public int attractionCurrentAmount = 0;								// Current amount the leviathan is attracted to the player
	
	// CLASS CONSTRUCTORS
	
	// CLASS METHODS
	
	static public void adjustAttraction( int amount )
		// adjusts the amount the Leviathan is attracted to the player
	{
		attractionCurrentAmount = attractionCurrentAmount + amount;
	}




}
