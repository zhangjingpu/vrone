/*
 * Copyright (C) 2014 - Carl Zeiss AG
 */

using UnityEngine;
using System.Collections;

/**
 * Devices currently supported by the VROneSDK.
 */
public enum VROneSDKSupportedDeviceModel : int {
	Unsupported,

	iPhone6,
	GalaxyS5,
}

/**
 * The VROneSDKDevice identifies the device and
 * holds some device specific values.
 * 
 * These values are hardcoded for now, since not all
 * of them can be retrieved properly using Unity.
 * 
 * All future devices will be listed here.
 */
public sealed class VROneSDKDevice : Object {

	/**
	 * The device currently running the app.
	 */
	private static readonly VROneSDKDevice _sharedInstance = new VROneSDKDevice ();
	public static VROneSDKDevice sharedInstance {
		get  {
			return _sharedInstance; 
		}
	}

	/**
	 * Private initializer, if running in the editor iPhone 6 will be simulated.
	 */
	private VROneSDKDevice () {
		var deviceModel = SystemInfo.deviceModel;
		
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			if (deviceModel == "iPhone7,2") { // iPhoneGeneration.iPhone6
				#if DEBUG
				if (Mathf.Min (Screen.width, Screen.height) != 750.0f || Mathf.Max (Screen.width, Screen.height) != 1334.0f) {
					Debug.LogError ("App is running on iPhone 6, but not in native screen resolution (750x1334). Make sure that you provide all required launch images.");
				}
				#endif
				dpi = 326.0f; diagonal = 4.7f; model = VROneSDKSupportedDeviceModel.iPhone6;
			}
		} else if (Application.platform == RuntimePlatform.Android) {
			// TODO Galaxy S5 identifier unknown
			// dpi = 432.0f; diagonal = 5.1f;  model = VROneSDKSupportedDeviceModel.GalaxyS5;
		} else {
			#if UNITY_EDITOR
			// unity editor is not supported, simulate iPhone 6
			dpi = 432.0f; diagonal = 5.1f; model = VROneSDKSupportedDeviceModel.GalaxyS5;
			#else
			#if DEBUG
			Debug.LogError ("Unknown deviceModel: " + deviceModel + " (" + Screen.width + ", " + Screen.height + ")");
			#endif
			#endif
		}

		if (dpi <= 0.0f) {
			// if this device is not supported yet, we will calculate some values
			dpi = Screen.dpi > 0.0f ? Screen.dpi : 160.0f;
			diagonal = Mathf.Sqrt (Mathf.Pow(Screen.width / dpi, 2.0f) + Mathf.Pow(Screen.height / dpi, 2.0f));
			model = VROneSDKSupportedDeviceModel.Unsupported;
		}
	}
	
	#region Properties
	/**
	 * Returns the name of the current device model.
	 */
	public string modelName {
		get {
			// TODO for now there are only LUTs for the Galaxy S5 
			return VROneSDKSupportedDeviceModel.GalaxyS5.ToString ();
			// return model.ToString();
		}
	}
	/**
	 * Device type.
	 */
	public VROneSDKSupportedDeviceModel model { get; private set; }
	
	/**
	 * For all devices supported by the VROne the dimensions
	 * of the screen need to be supplied.
	 * This method returns the devices screen diagonal in inch.
	 */
	public float diagonal  { get; private set; }

	/**
	 * Hardcoded DPI for now, since Screen.dpi doesn't always return
	 * correct dpi.
	 */
	public float dpi  { get; private set; }
	
	/**
	 * Camera offset applied to center of each camera.
	 */
	public float cameraOffset {
		get {
			return Mathf.Round((float)((32.5 - ((double)diagonal * 25.4 / 4.589)) * (double)dpi / 25.4));
		}
	}
	#endregion
}
