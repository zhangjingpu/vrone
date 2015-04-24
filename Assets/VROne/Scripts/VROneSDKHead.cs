/*
 * Copyright (C) 2014 - Carl Zeiss AG
 */

using UnityEngine;
using System.Collections;

/**
 * The VROneSDK knows its two VROneSDKEyes and
 * keeps them updated properly.
 */
namespace VROne
{
	public class VROneSDKHead : MonoBehaviour {
		#region Variables
		private VROneSDKEye _eyeLeft, _eyeRight;
		#endregion

		#region Properties
		/**
		 * The interpupullary distance.
		 * The IPD is the interpupillary distance (the distance between
		 * the pupils of the left and right eye).
		 * This value is measured in meters and is between 2cm and 8cm.
		 * Thus it is always positive.
		 * 
		 * If general you should use the VROneSDK behaviour to set the IPD.
		 */
		public float IPD {
			get {
				return eyeRight.IPD;
			}
			set {
				/*
				 * As soon as the IPD changes, both eyes need to
				 * be adapted accordingly.
				 */
				eyeLeft.IPD = value;
				eyeRight.IPD = value;
			}
		}
		
		public float A {
			get {
				return eyeRight.A;
			}
			set {
				/*
				 * As soon as A changes, both eyes need to
				 * be adapted accordingly.
				 */
				eyeLeft.A = value;
				eyeRight.A = value;
			}
		}
		
		/**
		 * VROne is enabled only if both eyes are enabled.
		 * Accordingly will both eyes be updated upon change.
		 */
		public bool isVROneEnabled {
			get {
				return eyeLeft.isVROneEnabled && eyeRight.isVROneEnabled;
			}
			set {
				eyeLeft.isVROneEnabled = value;
				eyeRight.isVROneEnabled = value;
			}
		}
		
		public bool isDistortionEnabled{
			get {
				return eyeLeft.isDistortionEnabled && eyeRight.isDistortionEnabled;
			}
			set {
				eyeLeft.isDistortionEnabled = value;
				eyeRight.isDistortionEnabled = value;
			}
		}


		/**
		 * Left eye of the VROneSDK. Contains the left camera.
		 */
		public VROneSDKEye eyeLeft {
			get {
				if (!_eyeLeft) {
					foreach (VROneSDKEye eye in GetComponentsInChildren<VROneSDKEye> ()) {
						if (eye.isLeftEye) {
							_eyeLeft = eye;
						}
					}
				}
				return _eyeLeft;
			}
		}

		/**
		 * Right eye of the VROneSDK. Contains the left camera.
		 */
		public VROneSDKEye eyeRight {
			get {
				if (!_eyeRight) {
					foreach (VROneSDKEye eye in GetComponentsInChildren<VROneSDKEye> ()) {
						if (!eye.isLeftEye) {
							_eyeRight = eye;
						}
					}
				}
				return _eyeRight;
			}
		}
		#endregion
	}
}
