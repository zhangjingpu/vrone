/*
 * Copyright (C) 2014 - Carl Zeiss AG
 */

using UnityEngine;
using System.Collections;

namespace VROne
{

	/**
	 * Devices currently supported by the VROneSDK.
	 */
	public enum VROneSDKSupportedDeviceModel : int {
		Unsupported,

		iPhone6, // iPhone7,2
		GalaxyS5, // SM-G900F
	}

	/**
	 * The VROneSDKDevice identifies the device.
	 * This is important for selecting the correct LUT.
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
			// default value
			model = VROneSDKSupportedDeviceModel.Unsupported;

			var deviceModel = SystemInfo.deviceModel;
			if (Application.platform == RuntimePlatform.IPhonePlayer) {
				if (deviceModel == "iPhone7,2") { // iPhoneGeneration.iPhone6
					#if DEBUG
					if (Mathf.Min (Screen.width, Screen.height) != 750.0f || Mathf.Max (Screen.width, Screen.height) != 1334.0f) {
						Debug.LogError ("App is running on iPhone 6, but not in native screen resolution (750x1334). Make sure that you provide all required launch images.");
					}
					#endif
					model = VROneSDKSupportedDeviceModel.iPhone6;
				}
			} else if (Application.platform == RuntimePlatform.Android) {
				if (deviceModel.Contains ("SM-G900F") || deviceModel.Contains("SM-G906S")) {
					model = VROneSDKSupportedDeviceModel.GalaxyS5;
				}
			} else {
				#if UNITY_EDITOR
				// unity editor is not supported, simulate iPhone
				model = VROneSDKSupportedDeviceModel.iPhone6;
				#endif
			}

			#if DEBUG
			if (model == VROneSDKSupportedDeviceModel.Unsupported) {
				Debug.LogError ("Unknown deviceModel: " + deviceModel + " (" + Screen.width + ", " + Screen.height + ")");
			}
			#endif
		}
		
		#region Properties
		/**
		 * Returns the name of the current device model.
		 */
		public string modelName {
			get {
				return model.ToString();
			}
		}
		/**
		 * Device type.
		 */
		public VROneSDKSupportedDeviceModel model { get; private set; }
		#endregion
	}

}
