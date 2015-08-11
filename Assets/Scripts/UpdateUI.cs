using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UpdateUI : Singleton<UpdateUI> {

	/*DEBUG UI OBJECTS*/
	[SerializeField]
	private Text phaseLabel;

	[SerializeField]
	private Text bearingLabel;

	[SerializeField]
	private Text tierLabel;
	
	[SerializeField]
	private Component mapPanel;

	[SerializeField]
	public GameObject bearingIndicator;

	private GameObject landmarkTextObject;
	private List<Landmark> chosenLandmarks;

	public static void PolarToCartesian(float radius, float degrees, float elevation, out Vector3 outCart){
		float polar = degrees * Mathf.Deg2Rad;
		float a = radius * Mathf.Cos (elevation);
		outCart.y = a * Mathf.Cos (polar);
		outCart.z = radius * Mathf.Sin (elevation);
		outCart.x = a * Mathf.Sin (polar);
	}

	public void displayLandmarks() {

		//remove all current UI elements
		foreach (Transform child in mapPanel.transform) {
			GameObject.Destroy(child.gameObject);
		}

		//pull up the list of landmarks for the chosen tier
		chosenLandmarks = LandmarkManager.Instance.getLandmarksFromTier (Periscope.Instance.tier );
	
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

			//create an audio source component on the text object
			AudioSource audioSourceComponent = landmarkTextObject.AddComponent<AudioSource>();
			audioSourceComponent.clip = landmark.AudioPreview;
			audioSourceComponent.loop = true;

		}

	}

	public void playAudioPreview()
		// plays sound effect(s) associated with landmark(s) along the current bearing
	{

		chosenLandmarks = LandmarkManager.Instance.getLandmarksFromTier (Periscope.Instance.tier );
		
		string bearingAsString = Periscope.Instance.bearing.ToString ();
		
		foreach (Landmark landmark in chosenLandmarks) {
			
			string landmarkBearingAsString = landmark.Bearing.ToString();
			
			//find the audio source for that landmark
			GameObject landmarkTextObject = GameObject.Find (landmark.Title);
			AudioSource audioSourceComponent = landmarkTextObject.GetComponent<AudioSource>();
			
			//play audio if periscope is facing the landmark
			if (bearingAsString == landmarkBearingAsString) {
				audioSourceComponent.Play ();
			} else {
				audioSourceComponent.Stop();
			}
			
		}
	}

	public void updateDebugText() 
	// updates the debug text objects
	{
		bearingLabel.text = "Bearing: " + Periscope.Instance.bearing.ToString ();
		tierLabel.text = "Tier: " + Periscope.Instance.tier.ToString ();
	}

	// Use this for initialization
	void Start () {
		phaseLabel.text = "Game Phase: " + GameManager.Instance.Phase.ToString();

		displayLandmarks ();
		playAudioPreview ();
		updateDebugText ();
	



	}
	
	// Update is called once per frame
	void Update () {



	
	}
	

}
