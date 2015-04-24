using UnityEngine;
using System.Collections;

public class ShowFPS : MonoBehaviour {

	void Start()
	{
		Debug.Log ("Show Fps!");
		StartCoroutine (UpdateFPS ());
	}



	void OnGUI()
	{
		GUI.Label (new Rect (0, 0, 100, 20), "Fps: " + fps.ToString("F2")+", "+fps2);
	}



	float fps;
	int fps2;

	IEnumerator UpdateFPS()
	{
		while (true)
		{
			float timeElapsed = 0;
			int frameCount = 0;
			while (timeElapsed < 1.0f)
			{
				frameCount++;
				timeElapsed += Time.deltaTime;
				yield return null;
			}
			fps = 1 / Time.deltaTime;
			fps2 = frameCount;
			Debug.Log(fps + ", " +fps2);
		}
	}
}
