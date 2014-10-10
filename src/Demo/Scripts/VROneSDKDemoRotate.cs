/*
 * Copyright (C) 2014 - Carl Zeiss AG
 */

using UnityEngine;
using System.Collections;

public class VROneSDKDemoRotate : MonoBehaviour {
	public Vector3 Pivot;

	/*
	 * Choose the axis to rotate around. 
	 */
	public bool RotateX = false;
	public bool RotateY = true;
	public bool RotateZ = false;

	void FixedUpdate()
	{
		transform.position += (transform.rotation * Pivot);

		if (RotateX)
			transform.rotation *= Quaternion.AngleAxis (45 * Time.deltaTime, Vector3.right);
		if (RotateY)
			transform.rotation *= Quaternion.AngleAxis (45 * Time.deltaTime, Vector3.up);
		if (RotateZ)
			transform.rotation *= Quaternion.AngleAxis (45 * Time.deltaTime, Vector3.forward);

		transform.position -= (transform.rotation * Pivot);
	}
}
