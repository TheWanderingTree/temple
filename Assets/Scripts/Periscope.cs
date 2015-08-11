using UnityEngine;
using System.Collections;

public class Periscope : Singleton<Periscope> {
		
	// CLASS VARIABLES

	public int tier = 1;										// Tier the periscope is auditioning/viewing (1-3)

	public enum Direction { N, NE, E, SE, S, SW, W, NW }		// Enumeration for compass directions
	public Direction DirectionChunk;
	public int bearing;									// Compass bearing the periscope is facing (N, NE, E, SE, S, SW, W, NW)
	public int chosenBearing; 							// Bearing chosen for exploration
	public int audioPreviewThreshold = 5;					// How many degrees away from landmark at which audio begins playing

	// CLASS METHODS
	
	void adjustTier()
	// adjusts the distance the periscope is auditioning/viewing
	{
		if (Input.GetKeyDown ("up")) {
			if (tier < 3) {
				tier++;
				UpdateUI.Instance.displayLandmarks ();
				UpdateUI.Instance.playAudioPreview ();
				UpdateUI.Instance.updateDebugText ();
			}
		}

		if (Input.GetKeyDown ("down")) {
			if (tier > 1) {
				tier--;
				UpdateUI.Instance.displayLandmarks ();
				UpdateUI.Instance.playAudioPreview ();
				UpdateUI.Instance.updateDebugText ();
			}
		}


	}

	void rotatePeriscope()
	// rotates the periscope
	{
	
		if (Input.GetKey("right")) {

			if (bearing < 360) { bearing++; }
			else { bearing = 0; }

			UpdateUI.Instance.bearingIndicator.transform.Rotate(0,0,-1);


		} else if (Input.GetKey("left")) {

			if (bearing > 0) { bearing--; }
			else { bearing = 359; }

			UpdateUI.Instance.bearingIndicator.transform.Rotate(0,0,1);

		}

		UpdateUI.Instance.playAudioPreview ();
		UpdateUI.Instance.updateDebugText ();

	}

	void commitBearing()
		// locks in the current bearing for exploration, disables searchlight in periscope view
	{
		chosenBearing = bearing;	
	}

	void Update() {
		if (Input.GetKey ("right") | Input.GetKey ("left")) { rotatePeriscope (); }
		if (Input.GetKey ("up") | Input.GetKey ("down")) { adjustTier(); }
		if (Input.GetKeyDown ("space")) { commitBearing(); }

	}
}
