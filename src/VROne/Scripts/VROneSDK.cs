/*
 * Copyright (C) 2014 - Carl Zeiss AG
 */

using UnityEngine;
using System.Collections;

/**
 * This is the main behaviour, where most available API
 * should be found. This object knows the head (VROneSDKHead),
 * which knows its to eyes (VROneSDKEye).
 * 
 * ## Singleton
 * Any public paramter required for the VROneSDK components
 * should be passed through this behaviour.
 * This behaviour can easily be accessed via
 * 
 * 		VROneSDK vrOneSDK = VROneSDK.sharedInstance;
 * 
 * ## Interpupillary distance (IPD)
 * One can set the IPD using this singleton via
 * 
 * 		VROneSDK.sharedInstance.IPD = 0.065f;
 * 
 * The default IPD can be accessed via
 * `VROneSDK.VROneSDKDefaultIPD`.
 * 
 * ## Mono-Mode/Stereo-Mode
 * When VROne runs in mono-mode, both eyes are located
 * at the very same position. Nevertheless make sure
 * that you use set the users IPD correctly, since this
 * will alter the distortion!
 * 
 * Using the stereo-mode both camera position will
 * respect the users IPD and enable the full 3D 
 * user experience.
 * 
 * Enable/disable mono- and stereo-mode via
 * 
 * 		VROneSDK.sharedInstance.runsInStereMode = true;
 * 
 * ## Enable/Disable VROne
 * When user hasn't put on the VROne yet, use 
 * 
 * 		VROneSDK.sharedInstance.isVROneEnabled = false;
 * 
 * to disable splitting the view and present a
 * regular screen to the user.
 * Once your game starts set this value to `true` 
 * and thus enable the full VROne experience for
 * the user.
 * 
 * ## Enable/Disable Screen Dimming
 * Use property `neverSleep` to prevent screen dimming
 * during gameplay.
 * 
 * ## Target frame rate
 * Use property `targetFrameRate` to get or set the
 * targetFrameRate for this app.
 * Default targetFrameRate is 60fps.
 */
public class VROneSDK : MonoBehaviour {
	#region Variables
	private static VROneSDK _sharedInstance;
	private VROneSDKHead _head;
	#endregion

	#region Properties
	/** 
	 * Singleton sharedInstance.
	 * At all times there should only be one VROneSDK.
	 */
	public static VROneSDK sharedInstance {
		get {
			if (_sharedInstance == null) {
				_sharedInstance = GameObject.Find ("VROneSDK").GetComponent<VROneSDK> ();
			}
			return _sharedInstance;
		}
	}

	/**
	 * Default value for IPD. Measured in meters.
	 */
	public const float VROneSDKDefaultIPD = 0.065f;

	/**
	 * The interpupullary distance.
	 * The IPD is the interpupillary distance (the distance between
	 * the pupils of the left and right eye).
	 * 
	 * This value is measured in meters.
	 * 
	 * For mono-mode (both eyes share the same location)
	 * set the IPD to 0.0f.
	 * 
	 * For stereo-mode (use two cameras to create a 3D effect)
	 * set the IDP to the users IPD (between 0.02f and 0.08f).
	 */
	public float IPD {
		get {
			return head.IPD;
		}
		set {
			head.IPD = value;
		}
	}

	/**
	 * When VROne is disabled, no distortion should be applied
	 * to the cameras. At the same time, the camera of the left
	 * eye will be disabled and the viewport of the rights eye
	 * camera will be set to fullscreen.
	 * 
	 * This can be useful, when your game will display a menu 
	 * before the user puts on the VROne. In this case you will
	 * set isVROneEnabled to `false` on startup and set to 
	 * `true` once the game starts.
	 */
	public bool isVROneEnabled {
		get {
			return head.isVROneEnabled;
		}
		set {
			head.isVROneEnabled = value;
		}
	}

	/**
	 * Using this property you can enabled and disable
	 * stereo mode.
	 * 
	 * When VROne runs in mono-mode both eyes/cameras are located
	 * at the same position. Thus there is no specific 3D effect
	 * visible to the user.
	 * 
	 * In stereo-mode the cameras are placed using the users IPD,
	 * thus creating a 3D scene presented to the user.
	 */
	public bool runsInStereMode {
		get {
			return IPD > 0.0f;
		}
		set {
			if (value != runsInStereMode) {
				// only update IPD if not currently running in desired mode
				head.IPD = value ? VROneSDKDefaultIPD : 0.0f;
			}
		}
	}

	/**
	 * Property neverSleep turns on and off screen dimming.
	 * Set to `true` when game is running and VROne is put on.
	 */
	public bool neverSleep {
		get {
			return Screen.sleepTimeout == SleepTimeout.NeverSleep;
		}
		set {
			Screen.sleepTimeout = value ? SleepTimeout.NeverSleep : SleepTimeout.SystemSetting;
		}
	}

	/**
	 * Returns or sets the frame rate for this game.
	 */
	public int targetFrameRate {
		get {
			return Application.targetFrameRate;
		}
		set {
			Application.targetFrameRate = value;
		}
	}

	/**
	 * Head of the VROneSDK. Contains both eyes.
	 */
	public VROneSDKHead head {
		get {
			if (!_head) {
				_head = GetComponentInChildren<VROneSDKHead> ();
			}
			return _head;
		}
	}
	#endregion

	#region Life cycle
	void Start () {
		// request 60 fps
		targetFrameRate = 60;
		// don't put screen to sleep
		neverSleep = true;
		// set anti-aliasing level to 2
		// QualitySettings.antiAliasing = 2;
	}
	#endregion
}
