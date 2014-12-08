/*
 * Copyright (C) 2014 - Carl Zeiss AG
 */

using UnityEngine;
using System.Collections;

namespace VROne
{
	/**
	 * An VROneSDKEye holds the actual camera.
	 * Its position is relative to the VROneSDKHead.
	 */
	public class VROneSDKEye : MonoBehaviour {
		#region Variables
		private VROneSDKLUTDistortion _distortion;
	    public bool isLeftEye;
		#endregion

		#region Properties

		/**
		 * When VROne is disabled, no distortion should be applied.
		 * At the same time, the camera of the right eye will be
		 * disabled and the viewport of the left eyes camera will
		 * be set to fullscreen.
		 */
		public bool isVROneEnabled {
			get {
				return distortion.enabled;
			}
			set {
				if (value != isVROneEnabled) {
					distortion.enabled = value;

					if (isLeftEye) {
						if (!value) {
							// set rights eye camera to fullscreen, when VROne is disabled
							camera.pixelRect = new Rect(0.0f, 0.0f, Screen.width, Screen.height);
						}
					} else {
						// right camera will be enabled/disabled according to value
						camera.enabled = value;
					}

					// reset the IPD for this eye, regardless of value
					IPD = IPD;
				}
			}
		}

		/**
		 * Updates the local position of this eye.
		 * This is just an x-transformation of the current local position.
		 * An eye is always positioned relative to the head.
		 * 
		 * IPD will return 0.0f and not set a value, when VROne is disabled.
		 */
		public float IPD {
			get {
				return isVROneEnabled ? (2.0f * Mathf.Abs (transform.localPosition.x)) : 0.0f;
			}
			set {
				// if VROne is currently disabled, disable setting IPD
				if (!isVROneEnabled) {
					return;
				}

				/*
			 	* Move the eye according to the new IPD. Divide by 2.0f
			 	* since we're moving relative to the center of the head.
			 	* The left eye moves to left thus requiring a
			 	* multiplication of -1.0f.
			 	*/
				value = Mathf.Abs(value);
				if (value < 1e-3) {
					// mono-mode, both cameras at the very same position
					value = 0.0f;
				} else {
					// stereo-mode, creates a 3D effect using two cameras at different positions
					value = (isLeftEye ? -1.0f : 1.0f) * Mathf.Clamp(value, 0.02f, 0.08f) / 2.0f;
				}
				transform.localPosition = new Vector3 (value, transform.localPosition.y, transform.localPosition.z);

				// dependent on the eye the camera rect needs to be adjusted
				camera.pixelRect = new Rect(isLeftEye ? 0.0f : (Screen.width / 2.0f), 0.0f, Screen.width / 2, Screen.height);
			}
		}

		/**
		 * The VROneSDKDistortion holds the actual shader.
		 */
		public VROneSDKLUTDistortion distortion {
			get {
				if (_distortion == null) {
					_distortion = gameObject.GetComponent<VROneSDKLUTDistortion> ();
					_distortion.isMirrored = !isLeftEye;
				}
				return _distortion;
			}
		}
		#endregion

		#region Life cycle
		void Start () {

			// set default IPD on start
			IPD = VROneSDK.VROneSDKDefaultIPD;
		}
		#endregion
	}
}
