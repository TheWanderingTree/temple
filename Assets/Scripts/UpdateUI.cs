using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UpdateUI : MonoBehaviour {

	[SerializeField]
	private Text phaseLabel;

	[SerializeField]
	private Text bearingLabel;
	
	[SerializeField]
	private Component mapPanel;

	[SerializeField]
	private GameObject bearingIndicator;

	private GameObject landmarkTextObject;


	private List<Landmark> chosenLandmarks;

	public static void PolarToCartesian(float radius, float degrees, float elevation, out Vector3 outCart){
		float polar = degrees * Mathf.Deg2Rad;
		float a = radius * Mathf.Cos (elevation);
		outCart.y = a * Mathf.Cos (polar);
		outCart.z = radius * Mathf.Sin (elevation);
		outCart.x = a * Mathf.Sin (polar);
	}

	// Use this for initialization
	void Start () {


		//set the current list of landmarks
		chosenLandmarks = LandmarkManager.Instance.tier1Landmarks;

		//for each landmark in the list...
		foreach (Landmark landmark in chosenLandmarks) {

			//get the bearing

			int chosenBearing = 0;

			switch (landmark.Bearing) {
				
			case Landmark.Direction.N:
				chosenBearing = 0;
				break;
				
			case Landmark.Direction.NE:
				chosenBearing = 45;
				break;
				
			case Landmark.Direction.E:
				chosenBearing = 90;
				break;
				
			case Landmark.Direction.SE:
				chosenBearing = 135;
				break;
				
			case Landmark.Direction.S:
				chosenBearing = 180;
				break;
				
			case Landmark.Direction.SW:
				chosenBearing = 225;
				break;
				
			case Landmark.Direction.W:
				chosenBearing = 270;
				break;
				
			case Landmark.Direction.NW:
				chosenBearing = 315;
				break;
			}

			//get the distance
			int chosenDistance = landmark.Distance;

			//calculate a vector for the position of landmark
			Vector3 chosenVector;
			PolarToCartesian (chosenDistance, chosenBearing, 0, out chosenVector);

			//create a new UI text object and make it a child of the map panel
			landmarkTextObject = new GameObject ();
			landmarkTextObject.transform.SetParent(mapPanel.transform);
			landmarkTextObject.name = landmark.Title;

			//create a text component on the text object
			Text textComponent = landmarkTextObject.AddComponent<Text>();

			//set the position of the text component

			RectTransform mapRect = mapPanel.GetComponent<RectTransform>();

			textComponent.rectTransform.anchorMin = new Vector2 (0.5f, 0.5f);
			textComponent.rectTransform.anchorMax = new Vector2 (0.5f, 0.5f);
			textComponent.rectTransform.anchoredPosition = new Vector2 (((chosenVector.x*(mapRect.rect.width/2))/300),((chosenVector.y*(mapRect.rect.height/2))/300));

			//set the font and alignment of the text component
			textComponent.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
			textComponent.alignment = TextAnchor.MiddleCenter;

			//set the text
			textComponent.text = "•";
		}

		phaseLabel.text = "Game Phase: " + GameManager.Instance.Phase.ToString();


	}
	
	// Update is called once per frame
	void Update () {

		bearingLabel.text = "Bearing: " + Periscope.Instance.bearing.ToString ();

		if (Input.GetKeyDown ("right")) {
			bearingIndicator.transform.Rotate(0,0,-45);
		}

		if (Input.GetKeyDown ("left")) {
			bearingIndicator.transform.Rotate(0,0,45);
		}
	
	}
	

}
