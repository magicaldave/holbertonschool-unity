using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveplatforms : MonoBehaviour
{
	Vector3 originalPos, orientation, endPos;
	[SerializeField] float seedSpeed = 1f, seedSpeedRange = 0.5f;
	float platformSpeed;
	[SerializeField] Vector3 moveRange;

	void Start()
	{
		originalPos = transform.position;
		endPos =  originalPos + moveRange;
		platformSpeed = Random.Range(seedSpeed, seedSpeed + seedSpeedRange);
	}

	void Update()
	{
		if (transform.position == originalPos)
			orientation = endPos;
		else if (transform.position == endPos)
			orientation = originalPos;

		transform.position = Vector3.MoveTowards(transform.position, orientation, platformSpeed * Time.deltaTime);

	}
}
