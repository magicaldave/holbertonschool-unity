using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinTrigger : MonoBehaviour
{
	private Text timeUI;
	private GameObject Player;
	private Timer timeScript;

	void Start()
	{
		Player = GameObject.Find("Player");
		timeUI = GameObject.Find("TimerText").GetComponent<Text>();
		timeScript = Player.GetComponent<Timer>();
	}

	void OnTriggerEnter()
	{

		// Run the ExplodeFace Method from the PlayerController
		Player.GetComponent<PlayerController>().ExplodeFace();

		// Blow up the timer text, then stop the script
		timeUI.color = Color.green;
		timeUI.fontSize = 60;
		Destroy(timeScript);
	}
}
