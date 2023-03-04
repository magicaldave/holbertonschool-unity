using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerTrigger : MonoBehaviour
{
	public GameObject Player;
	private Timer timeScript;

	// Start is called before the first frame update
	void Start()
	{
		timeScript = Player.GetComponent<Timer>();
	}

	// Update is called once per frame
	void OnTriggerExit(Collider other)
	{
		timeScript.enabled = true;
		Destroy(gameObject);
	}
}
