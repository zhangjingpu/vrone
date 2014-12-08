using UnityEngine;
using System.Collections;

public static class HeadTrackingAndroid {

#if UNITY_ANDROID
	static AndroidJavaObject playerActivityContext;
	static AndroidJavaClass jc;

	static bool initialized = false;

	// Use this for initialization
	public static void Initialize () {
		AndroidJavaClass actClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		playerActivityContext = actClass.GetStatic<AndroidJavaObject>("currentActivity");
		jc = new AndroidJavaClass("de.innoactive.AndroidSensorFusion.AndroidSensorFusion");
		initialized = true;
		//jc.CallStatic("setFactors", 1.0f, 0.0f);
	}
	
	// Update is called once per frame
	public static Quaternion GetQuaternionUpdate () {
		if (!initialized) return Quaternion.identity;
		double[] q = jc.CallStatic<double[]>("getRotation", playerActivityContext);
		return new Quaternion((float)q[0], (float)q[1], (float)q[2], (float)q[3]);
	}
#endif
}
