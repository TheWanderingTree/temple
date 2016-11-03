using UnityEngine;

public class TemplePull : MonoBehaviour 
{

	// CLASS VARIABLES
	static int dailyIncreaseAmount;								// Amount of temple pull accrued overnight (should be constant)
	static int decreaseAmount;									// Amount of temple pull reduced after progress is made
	static int currentAmount;									// Current amount of temple pull

	// CLASS METHODS
	static public void adjustTemplePull( int amount )
	{
	// adjusts the amount of Temple Pull
		currentAmount = currentAmount + amount;
	}
}