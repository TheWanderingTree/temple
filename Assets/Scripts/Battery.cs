﻿using UnityEngine;

public class Battery : MonoBehaviour 
{

	// CLASS VARIABLES

	static public int batteryTier = 0;										// The current upgrade level of the battery (0-3)
	static public int startingAmount = 50;									// Amount of battery fuel at game start (should be constant)
	static public int dailyRechargeAmount = 30;								// Amount of battery fuel recharged each night (should be slightly random)
	static public int maxCapacity = 100;									// Maximum fuel capacity for the battery, increased along with batteryTier
	static public int depletionAmount = 1;									// Amount of battery fuel depleted during gameplay, at each depletionInterval (should be constant)
	static public int depletionInterval = 12;								// Duration of time between each fuel depletion (should be constant)
	static public int currentAmount;										// Current amount of battery fuel
	static public int timeAtLastDeplete;									// Game time at which last depletion occured

	// CLASS METHODS
	
	//increases or decreases an amount of fuel from the battery
	static public void adjustBattery( int amount)
	{
		currentAmount = currentAmount + amount;
	}

	//constantly depletes the battery by depletionAmount every depletionInterval (in seconds)
	public void constantlyDepleteBattery() 
	{
		int timeAsInt = (int)Time.time;
		if ((timeAsInt % depletionInterval == 0) && (timeAsInt != timeAtLastDeplete)) {
			timeAtLastDeplete = timeAsInt;
			adjustBattery(depletionAmount);
		}
	}

	// upgrades the battery's tier and max capacity
	static public void upgradeBattery()
	{
		if (batteryTier < 3) 
		{
			batteryTier++;
		}
	}

	// resets the battery amount after death
	static public void resetBattery()
	{
		currentAmount = startingAmount;
	}

	static public void print() 
	{
		print ("Current battery amount: " + currentAmount);
	}

	void Start() 
	{
		resetBattery ();
	}

	void Update() 
	{
		constantlyDepleteBattery ();
	}
}
