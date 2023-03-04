using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinTrigger : MonoBehaviour
{
	public Text timeUI;
	private GameObject Player;
	private Timer timeScript;

	void Start()
	{
		Player = GameObject.Find("Flat Stanley");
		timeScript = Player.GetComponent<Timer>();
	}

	void OnTriggerEnter()
	{
		timeUI.color = Color.green;
		timeUI.fontSize = 60;
		Destroy(timeScript);
	}
}
