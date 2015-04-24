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

		iPhone6,	// iPhone7,2
		GalaxyS4,	// GT-I95..., SHV-E3..., SCH-I545, SPH-L720, SCH-R970, SGH-M919, SCH-R970, SGH-I337, SCH-I959, SC-04E
		GalaxyS5,	// SM-G90...
		GalaxyS6, 	// SM-G920T, SM-G925T, SM-G920F
		LG_G3,		// LG-D855
		Nexus5,		// LG-D820, LG-D821, LG Nexus 5
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
		 * Private initializer, if running in the editor or unsuported, Galaxy S5 will be used as fallback.
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
					A = 0.04791f;
				}
			} else if (Application.platform == RuntimePlatform.Android) {
				if (deviceModel.Contains("GT-I95") ||
				    deviceModel.Contains("SHV-E3") ||
				    deviceModel.Contains ("SCH-I545") ||
				    deviceModel.Contains ("SPH-L720") ||
				    deviceModel.Contains ("SCH-R970") ||
				    deviceModel.Contains ("SGH-M919") ||
				    deviceModel.Contains ("SCH-R970") ||
				    deviceModel.Contains ("SGH-I337") ||
				    deviceModel.Contains ("SCH-I959") ||
				    deviceModel.Contains ("SC-04E") )
				{
					model = VROneSDKSupportedDeviceModel.GalaxyS4;
					A = 0.03059f;
				}
				else if (deviceModel.Contains("SM-G90") )
				{
					model = VROneSDKSupportedDeviceModel.GalaxyS5;
					A = 0.02454f;
				}
				else if (deviceModel.Contains ("SM-G920T") ||
				         deviceModel.Contains ("SM-G925T") ||
				         deviceModel.Contains ("SM-G920F"))
				{
					model = VROneSDKSupportedDeviceModel.GalaxyS6;
					A = 0.02527f;
				}
				else if (deviceModel.Contains("D820") ||
				         deviceModel.Contains("D821") ||
				         deviceModel.Contains("LGE Nexus 5") )
				{
					model = VROneSDKSupportedDeviceModel.Nexus5;
					A = 0.03576f;
				}
				else if (deviceModel.Contains("LG-D855") )
				{
					model = VROneSDKSupportedDeviceModel.LG_G3;
					A = 0.00458f;
				} 
			} else {
				#if UNITY_EDITOR
					// unity editor is not supported, simulate Galaxy S5
					model = VROneSDKSupportedDeviceModel.GalaxyS5;
					A = 0.02454f;
				#endif
			}

			if (model == VROneSDKSupportedDeviceModel.Unsupported) {
				model = VROneSDKSupportedDeviceModel.GalaxyS5;
				A = 0.02454f;
				#if DEBUG
				Debug.LogError ("Unknown deviceModel: " + deviceModel + " (" + Screen.width + ", " + Screen.height + ")");
				#endif

			}
			Debug.Log ("DeviceModel: " + deviceModel);
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

		public float A { get; private set; }
		#endregion
	}

}
