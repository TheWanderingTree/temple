using UnityEngine;
using System.Collections;

public class Periscope : MonoBehaviour {
		
	// CLASS VARIABLES

	public bool locked;											// True if the periscope bearing has been committed

	public int tier = 1;										// Tier the periscope is auditioning/viewing (1-3)

	public enum Direction { N, NE, E, SE, S, SW, W, NW }		// Enumeration for compass directions
	public Direction bearing;									// Compass bearing the periscope is facing (N, NE, E, SE, S, SW, W, NW)
	public Direction chosenBearing; 							// Bearing chosen for exploration

	// CLASS METHODS
	
	void adjustTier()
	// adjusts the distance the periscope is auditioning/viewing
	{
		if (Input.GetKeyDown ("up")) {
			if (tier < 3) {
				tier++;
			}
		}

		if (Input.GetKeyDown ("down")) {
			if (tier > 1) {
				tier--;
			}
		}
	}

	void rotatePeriscope()
	// rotates the periscope
	{
		if (Input.GetKeyDown("right")) {

			int intBearing = (int)bearing;

			if (intBearing < 7) { bearing++; }
			else { bearing = Direction.N; }

		} else if (Input.GetKeyDown("left")) {

			int intBearing = (int)bearing;

			if (intBearing > 0) { bearing--; }
			else { bearing = Direction.NW; }
		}
	}

	void playAudioPreview()
	// plays sound effect(s) associated with landmark(s) along the current bearing
	{
		
	}

	void commitBearing()
		// locks in the current bearing for exploration, disables searchlight in periscope view
	{
		if (!locked) {
			locked = true;
			chosenBearing = bearing;
		}
	}

	void Update() {
		if (Input.GetKeyDown ("right") | Input.GetKeyDown ("left")) { rotatePeriscope (); }
		if (Input.GetKeyDown ("up") | Input.GetKeyDown ("down")) { adjustTier(); }
		if (Input.GetKeyDown ("space")) { commitBearing(); }

	}
}
