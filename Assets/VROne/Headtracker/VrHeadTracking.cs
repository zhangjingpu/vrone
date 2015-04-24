using UnityEngine;
using System.Collections;

namespace VROne
{
	public class VrHeadTracking : MonoBehaviour {

		public bool resetViewOnTouch = false;
		public static VrHeadTracking instance;

		// Use this for initialization
		void Start () {
			instance = this;

			//Set the initial rotation to align the virtual world with the real world on start
			initialRotation = Quaternion.Euler (new Vector3 (-90, 0, 0));

#if UNITY_IPHONE
			HeadTrackingIOS.StartCameraUpdates();
#endif
#if UNITY_ANDROID
			HeadTrackingAndroid.Initialize();
#endif
		}

		Quaternion initialRotation = Quaternion.identity;

		public void ResetView()
		{
			recenter = true;
		}



		bool recenter = false;

		// Update is called once per frame
		void Update() {
	#if UNITY_IPHONE && !UNITY_EDITOR
			Quaternion rot = HeadTrackingIOS.GetQuaternionUpdate();
			if (recenter || resetViewOnTouch && (Input.touchCount > 0))
			{
				initialRotation = rot;
				recenter = false;
			}
			transform.rotation = Quaternion.Inverse(initialRotation) * rot; //works for landscape left
	#endif
	#if UNITY_ANDROID && !UNITY_EDITOR
			Quaternion rot = HeadTrackingAndroid.GetQuaternionUpdate();
			transform.rotation = Quaternion.Inverse(initialRotation) * rot; //works for landscape left
			if (recenter || resetViewOnTouch && (Input.touchCount > 0))
			{
				initialRotation = rot;
				recenter = false;
			}
	#endif
		}
	}
}