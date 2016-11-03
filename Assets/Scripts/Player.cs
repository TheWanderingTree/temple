using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour 
{

	// CLASS VARIABLES

	static public bool mustDive;										// Whether the player has the option to stay inside the submarine or not

	static public string currentPuzzle;									// The current game-mechanics puzzle the player hasn't solved yet

	static public string[] traits = new string[4];						// Array of character traits the player has (four traits)
	static public List<string> inventory = new List<string> ();				// List of inventory items

	// CLASS CONSTRUCTORS

	// CLASS METHODS

	static public void assignTraits (string _trait1, string _trait2, string _trait3, string _trait4) 
	{
	// assigns four traits to the player, replacing previous traits

	}

	static public void addItem ( string item ) 
	{
	// adds an item to the player's inventory

	}

	static public void removeItem ( string item )
	{
	// removes an item from the player's inventory

	}

	static public void Dive()
	{
	// transitions from submarine phase to ocean phase
		GameManager.Instance.changeToPhase (GameManager.GamePhase.ocean);
	}

	static public void stayInside()
	{
	// increases Temple Pull and forces a dive on the next day
		mustDive = true;
		TemplePull.adjustTemplePull (20);
	}


}
