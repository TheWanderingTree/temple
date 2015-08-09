using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> {

	// CLASS FIELDS

	private bool paused = false;  										// True if the game is paused	

	public enum GamePhase { submarine, ocean, dream } ;					// Enumeration for game phases
	private GamePhase phase;											// Current game phase (submarine, ocean or dream)


	// CLASS PROPERTIES

	public bool Paused 
	{ 
		get 
		{
			return paused;
		} 
		set 
		{
			paused = value;
		} 
	}

	public GamePhase Phase 
	{
		get
		{
			return phase;
		}
		set
		{
			phase = value;
		}
	}


	// CLASS METHODS
	
	public void pauseGame()
	// pauses the game
	{
		if (!Paused) { Paused = true; } else { Paused = false; }	
	}
	
	public void changeToPhase( GamePhase chosenPhase )
	// changes the game phase
	{
		Phase = chosenPhase;
		print ("Phase: " + Phase);
	}
}
