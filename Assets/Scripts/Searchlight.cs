﻿using UnityEngine;

public class Searchlight : MonoBehaviour 
{

	// CLASS VARIABLES
	
	public bool isOn = true;											// True if the searchlight is turned on

	public int switchDuration = 3;										// Duration of time spent in darkness when switching light types (in seconds)
	public int numberOfLightTypesUnlocked = 1;							// Number of light types unlocked

	public float startTime = 0f;										// Game time at which light type switch begins
	
	public enum LightType { normal, bio, temple }						// Enumeration for light types
	public LightType lightType;											// Current light type used by the searchlight (normal, bio, temple)

	public Light lightObject;
	public Light centerLight;

	public Light[] lights;

	private int baseLightIntensity = 2;


	// CLASS METHODS

	public void upgradeSearchlight()							
	{
	// unlocks a new light type for the searchlight
		if (numberOfLightTypesUnlocked < 3) 
		{
			numberOfLightTypesUnlocked++;
		}

		switch (numberOfLightTypesUnlocked) 
		{
			case 1:
				lightObject.intensity = baseLightIntensity;
				centerLight.intensity = baseLightIntensity*2;
				break;
			case 2:
				lightObject.intensity = baseLightIntensity*2;
				centerLight.intensity = baseLightIntensity*4;
				break;
			case 3:
				lightObject.intensity = baseLightIntensity*4;
				centerLight.intensity = baseLightIntensity*16;
				break;
		}
	}

	public void toggleOnOff()								
	{
	// toggles the searchlight on and off
		isOn = !isOn;
			
		foreach (Light light in lights) 
		{
			light.enabled = !light.enabled;
		}
	}

	public void startTimer() 
	{
	//starts the light-switch timer (lasts switchDuration)
		startTime = Time.time;
	}

	public void checkTimer() 
	{
		if (Time.time > startTime + switchDuration) 
		{
			switchLightType();
			toggleOnOff();
			resetTimer();
			Battery.adjustBattery(-1);
			Leviathan.adjustAttraction(5);
		}
	}

	public void resetTimer() 
	{
		startTime = 0f;
	}

	public void switchLightType()								
	{
	// switches the searchlight's light type (if available)
		int intLightType = (int)lightType;

		if (intLightType < numberOfLightTypesUnlocked - 1) 
		{
			lightType++;
		} 
		else 
		{
			lightType = 0;
		}

		switch (lightType) 
		{
			case LightType.normal:
				lightObject.color = Color.white;
				centerLight.color = Color.white;
				break;
			case LightType.bio:
				lightObject.color = Color.blue;
				centerLight.color = Color.blue;
				break;
			case LightType.temple:
				lightObject.color = Color.green;
				centerLight.color = Color.green;
				break;
		}
	}

	void Start() 
	{
		lightObject = this.GetComponent<Light> ();
		lightObject.intensity = 2;
	}

	void Update() 
	{
		if (Input.GetKeyDown ("tab") && isOn && (numberOfLightTypesUnlocked > 1) ) 
		{
			toggleOnOff();
			startTimer();
		}

		if (Input.GetKeyDown ("q")) 
		{
			upgradeSearchlight();
		}

		if (startTime > 0f) 
		{
			checkTimer ();
		}
	}
}
