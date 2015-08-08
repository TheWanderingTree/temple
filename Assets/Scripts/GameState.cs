using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {

	// CLASS VARIABLES

	static public bool paused;												// True if the game is paused

	public enum GamePhase { submarine, ocean, dream } ;				// Enumeration for game phases
	static public GamePhase phase;											// Current game phase (submarine, ocean or dream)
	



	// CLASS METHODS
	
	static public void pauseGame()
	// pauses the game
	{
		if (!paused) { paused = true; } else { paused = false; }	
	}
	
	static public void changeToPhase( GamePhase chosenPhase )
	// changes the game phase
	{
		phase = chosenPhase;
		print ("Phase: " + phase);
	}
}
