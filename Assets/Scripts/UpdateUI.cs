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
	private GameObject sphere;
	private List<Landmark> chosenLandmarks;
	private int audioPreviewThreshold = 5;					// How many degrees away from landmark at which audio begins playing

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

		if (GameObject.FindWithTag("Sphere")) {
			GameObject[] spheres = GameObject.FindGameObjectsWithTag ("Sphere");

			foreach (GameObject sphere in spheres) {
				GameObject.Destroy(sphere.gameObject);
			}
		}

		//pull up the list of landmarks for the chosen tier
		chosenLandmarks = LandmarkManager.Instance.getLandmarksFromTier (Periscope.Instance.tier );
	
		//for each landmark in the list...
		foreach (Landmark landmark in chosenLandmarks) {
			
			//get the bearing
			int chosenBearing = landmark.Bearing;
			
			//get the distance
			int chosenDistance = landmark.Distance;
			
			//calculate a vector for the position of landmark
			Vector3 chosenVector;
			PolarToCartesian (chosenDistance, chosenBearing, 0, out chosenVector);

			GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			sphere.tag = "Sphere";
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

			//create an audio source component on the text object
			AudioSource audioSourceComponent = landmarkTextObject.AddComponent<AudioSource>();
			audioSourceComponent.clip = landmark.AudioPreview;
			audioSourceComponent.loop = true;

		}

	}

	IEnumerator FadeIn() {
		
		AudioSource thisAudioC = landmarkTextObject.GetComponent<AudioSource>();
		float currentVol = thisAudioC.volume;
		for (float f = currentVol; f <= 1; f += 0.05f) {
			thisAudioC.volume = f;
			yield return new WaitForSeconds(0.05f);
		}
	}

	IEnumerator FadeOut() {
		
		AudioSource thisAudioC = landmarkTextObject.GetComponent<AudioSource>();
		float currentVol = thisAudioC.volume;
		for (float f = currentVol; f >= 0; f -= 0.05f) {
			thisAudioC.volume = f;
			yield return new WaitForSeconds(0.05f);
		}
		thisAudioC.Stop ();
	}


	public void playAudioPreview()
		// plays sound effect(s) associated with landmark(s) along the current bearing
	{

		chosenLandmarks = LandmarkManager.Instance.getLandmarksFromTier (Periscope.Instance.tier );

		foreach (Landmark landmark in chosenLandmarks) {
			
			//find the audio source for that landmark
			GameObject landmarkTextObject = GameObject.Find (landmark.Title);
			AudioSource audioSourceComponent = landmarkTextObject.GetComponent<AudioSource>();
			
			//play audio if periscope is facing the landmark
			if (
				(Periscope.Instance.bearing >= landmark.Bearing - audioPreviewThreshold) 
				&& 
				(Periscope.Instance.bearing <= landmark.Bearing + audioPreviewThreshold)) {
					if (audioSourceComponent.isPlaying == false) {
						audioSourceComponent.Play ();
						audioSourceComponent.volume = 0;
						StartCoroutine("FadeIn");
					}
			} else {
				if (audioSourceComponent.isPlaying == true) {
					StartCoroutine("FadeOut");

				}
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
