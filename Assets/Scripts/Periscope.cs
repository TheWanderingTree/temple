using UnityEngine;
using System.Collections;

public class Periscope : Singleton<Periscope> {
		
	// CLASS VARIABLES

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

		UpdateUI.Instance.displayLandmarks();
	}

	void rotatePeriscope()
	// rotates the periscope
	{
	
		if (Input.GetKeyDown("right")) {

			int intBearing = (int)bearing;

			if (intBearing < 7) { bearing++; }
			else { bearing = Direction.N; }

			UpdateUI.Instance.bearingIndicator.transform.Rotate(0,0,-45);


		} else if (Input.GetKeyDown("left")) {

			int intBearing = (int)bearing;

			if (intBearing > 0) { bearing--; }
			else { bearing = Direction.NW; }

			UpdateUI.Instance.bearingIndicator.transform.Rotate(0,0,45);

		}

	}

	void playAudioPreview()
	// plays sound effect(s) associated with landmark(s) along the current bearing
	{
		
	}

	void commitBearing()
		// locks in the current bearing for exploration, disables searchlight in periscope view
	{
		chosenBearing = bearing;	
	}

	void Update() {
		if (Input.GetKeyDown ("right") | Input.GetKeyDown ("left")) { rotatePeriscope (); }
		if (Input.GetKeyDown ("up") | Input.GetKeyDown ("down")) { adjustTier(); }
		if (Input.GetKeyDown ("space")) { commitBearing(); }

	}
}
