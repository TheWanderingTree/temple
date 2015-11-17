using UnityEngine;
using System.Collections;

public class Periscope : Singleton<Periscope> {
		
	// CLASS VARIABLES

	public int tier = 1;										// Tier the periscope is auditioning/viewing (1-3)

	public enum Direction { N, NE, E, SE, S, SW, W, NW }		// Enumeration for compass directions
	public Direction DirectionChunk;
	public float bearing;									// Compass bearing the periscope is facing (0-360)
	public float chosenBearing; 							// Bearing chosen for exploration
	public float rotationSpeed = 0f;
	public float acc = 2f;


	// CLASS METHODS
	
	void adjustTier()
	// adjusts the distance the periscope is auditioning/viewing
	{
		if (Input.GetKeyDown ("up")) {
			if (tier < 3) {
				tier++;
				UpdateUI.Instance.updateLandmarks();
				UpdateUI.Instance.displayLandmarks ();
				UpdateUI.Instance.updateDebugText ();
			}
		}

		if (Input.GetKeyDown ("down")) {
			if (tier > 1) {
				tier--;
				UpdateUI.Instance.updateLandmarks();
				UpdateUI.Instance.displayLandmarks ();
				UpdateUI.Instance.updateDebugText ();
			}
		}


	}

	void adjustVelocity() 
	// adjusts rotation speed of periscope based on input
	{
		if (Input.GetKey("right")) {
			
			if (rotationSpeed < 8f) { 
				rotationSpeed += Time.deltaTime*acc; 
			}
			
		} else if (Input.GetKey("left")) {
			
			if (rotationSpeed > -8f) { 
				rotationSpeed -=Time.deltaTime*acc; 
			}
		}
	}

	void drift()
	// drifts the periscope after releasing input
	{
		if (rotationSpeed > 0f) { rotationSpeed -= Time.deltaTime*acc; if (rotationSpeed < 0f) { rotationSpeed = 0f; } }
		if (rotationSpeed < 0f) { rotationSpeed += Time.deltaTime*acc; if (rotationSpeed > 0f) { rotationSpeed = 0f; } }
	}

	void rotatePeriscope()
	// rotates the periscope
	{
		bearing += Time.deltaTime*rotationSpeed;
		if (bearing < 0f) { bearing = 360f; }
		if (bearing > 360f) { bearing = 0f; }

		UpdateUI.Instance.bearingIndicator.transform.Rotate(0,0,-Time.deltaTime*rotationSpeed);
		Camera.main.transform.Rotate(0,Time.deltaTime*rotationSpeed,0);

		UpdateUI.Instance.updateDebugText ();

	}

	void commitBearing()
		// locks in the current bearing for exploration, disables searchlight in periscope view
	{
		chosenBearing = bearing;	
	}

	void Update() {
		if (Input.GetKey("right") | Input.GetKey("left")) { adjustVelocity(); }
		else { if (rotationSpeed != 0f) { drift(); } }

		if (rotationSpeed != 0f) { rotatePeriscope(); } 

		if (Input.GetKey ("up") | Input.GetKey ("down")) { adjustTier(); }
		if (Input.GetKeyDown ("space")) { commitBearing(); }


	}
}
