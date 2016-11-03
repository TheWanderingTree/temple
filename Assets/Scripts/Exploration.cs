using UnityEngine;
using System.Collections.Generic;

public class Exploration : MonoBehaviour 
{
	
	// CLASS VARIABLES

	public int tier;												// Tier the player is currently in
	public int distance;											// Distance the player is at within a tier
	
	public List<string> availableEncounters = new List<string> (); 	// List of available encounters in the current tier
	
	// CLASS CONSTRUCTORS
	
	// CLASS METHODS
	
	// plucks an encounter from the list of available encounters and triggers it
	public void Encounter()
	{
		
	}

	public void EnterLandmark()
	{
	
	}
}
