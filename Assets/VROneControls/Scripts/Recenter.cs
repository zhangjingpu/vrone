using UnityEngine;
using System.Collections;

namespace VROne
{
	/// <summary>
	/// Shows a recenter icon with a countdown from 3 to 0. After the countdown it resets the view
	/// </summary>
	public class Recenter : MonoBehaviour {
		public TextMesh text;

		public float countDownTime = 3.0f;

		// Use this for initialization
		void Start () {
			StartCoroutine (DoCountDown ());
		}

		IEnumerator DoCountDown()
		{
			for (int i = 3; i > 0; i --)
			{
				text.text = ""+i;
				yield return new WaitForSeconds(countDownTime / 3.0f);
			}
			VrHeadTracking.instance.ResetView ();
			Destroy (gameObject);
		}
	}
}