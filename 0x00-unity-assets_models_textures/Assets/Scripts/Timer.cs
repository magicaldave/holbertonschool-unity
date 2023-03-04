using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	public Text TimerText;
	private float runTime;

	void Update()
	{
		runTime += Time.deltaTime;
		TimerText.text = Mathf.FloorToInt(runTime / 60F).ToString("0") + ":" + // Minutes
			Mathf.FloorToInt(runTime % 60F).ToString("00") + "." + // Seconds
			Mathf.FloorToInt((runTime * 100F) % 100F).ToString("00"); // Milliseconds
	}
}
