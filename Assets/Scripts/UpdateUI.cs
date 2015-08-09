using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateUI : MonoBehaviour {

	[SerializeField]
	private Text phaseLabel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		phaseLabel.text = "Game Phase: " + GameManager.Instance.Phase.ToString();
	}


}
