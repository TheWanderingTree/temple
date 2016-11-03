using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UpdateUI : Singleton<UpdateUI> 
{

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
	private GameObject sphere;
	private List<Landmark> chosenLandmarks;

	public static void PolarToCartesian(float radius, float degrees, float elevation, out Vector3 outCart) 
	{
		float polar = degrees * Mathf.Deg2Rad;
		float a = radius * Mathf.Cos (elevation);
		outCart.y = a * Mathf.Cos (polar);
		outCart.z = radius * Mathf.Sin (elevation);
		outCart.x = a * Mathf.Sin (polar);
	}

	public void updateLandmarks() 
	{
	
		//remove all current UI elements
		foreach (Transform child in mapPanel.transform) 
		{
			GameObject.Destroy(child.gameObject);
		}
		
		if (GameObject.FindWithTag("Sphere")) 
		{
			GameObject[] spheres = GameObject.FindGameObjectsWithTag ("Sphere");
			
			foreach (GameObject sphere in spheres) 
			{

				// if the sphere has an audio event on it
				if (sphere.GetComponent<AkAmbient>() != null) 
				{ 

					//Get the AKAmbient component on the sphere
					AkAmbient akAmbientComponent = sphere.GetComponent<AkAmbient>();
					akAmbientComponent.enableActionOnEvent = true;

					//Get the event
					uint eventUINT = (uint)akAmbientComponent.eventID;
					
					//Stop the event
					AkSoundEngine.ExecuteActionOnEvent(eventUINT, AkActionOnEventType.AkActionOnEventType_Stop, sphere, 400, AkCurveInterpolation.AkCurveInterpolation_SCurve);
				}

				//Destroy the sphere
				GameObject.Destroy(sphere.gameObject);
			}
		}
	}

	public void displayLandmarks() 
	{

		//pull up the list of landmarks for the chosen tier
		chosenLandmarks = LandmarkManager.Instance.getLandmarksFromTier (Periscope.Instance.tier );
	
		//for each landmark in the list...
		foreach (Landmark landmark in chosenLandmarks) 
		{
			
			//get the bearing
			int chosenBearing = landmark.Bearing;
			
			//get the distance
			int chosenDistance = landmark.Distance;
			
			//calculate a vector for the position of landmark
			Vector3 chosenVector;
			PolarToCartesian (chosenDistance, chosenBearing, 0, out chosenVector);

			GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			sphere.tag = "Sphere";
			sphere.name = landmark.Title;
			sphere.transform.position = new Vector3(chosenVector.x/2,-0.85f,chosenVector.y/2);
			
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

			// if the landmark has an audio preview event
			if (landmark.AudioEventName != null) 
			{ 

				//create an AK GameObj component on the sphere
				AkGameObj akGameObjComponent = sphere.AddComponent<AkGameObj>();
				akGameObjComponent.isEnvironmentAware = false;

				//create an AK Ambient component on the sphere
				AkAmbient akAmbientComponent = sphere.AddComponent<AkAmbient>();

				//assign the landmark's event ID to the AK Ambient component
				akAmbientComponent.eventID = (int)landmark.AudioEventName;
			}
		}
	}

	public void updateDebugText() 
	{
	// updates the debug text objects
		bearingLabel.text = "Bearing: " + Periscope.Instance.bearing.ToString ();
		tierLabel.text = "Tier: " + Periscope.Instance.tier.ToString ();
		phaseLabel.text = "Game Phase: " + GameManager.Instance.Phase.ToString();
	}

	void Start () 
	{
		displayLandmarks ();
		updateDebugText ();
	}
	
	void Update () 
	{
	
	}
}
