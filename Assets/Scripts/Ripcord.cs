using UnityEngine;
using System.Collections;

public class Ripcord : MonoBehaviour {

	// CLASS FIELDS
	
	bool used;											// True if the ripcord has been pulled
	bool hasCharge;										// True if the player has installed a pneumatic charge in the ripcord

	// CLASS METHODS
	
	void installCharge()
	// installs a pneumatic charge in the ripcord for the rest of the day
	{
		hasCharge = true;
	}

	void pullRipcord()
	// pulls the ripcord, pulling the player back to the submarine along the path they took
	{
		used = true;
		if (hasCharge) { hasCharge = false; }
		GameManager.Instance.changeToPhase (GameManager.GamePhase.submarine);
	}

	void rechargeRipcord()
	// re-enables the use of the ripcord (happens overnight)
	{
		used = false;
	}

}
