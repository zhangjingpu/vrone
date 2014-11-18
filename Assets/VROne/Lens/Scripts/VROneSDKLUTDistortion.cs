/*
 * Copyright (C) 2014 - Carl Zeiss AG
 */

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
/**
 * LUT Distortion behaviour used by the VROne.
 * LUTs are device and IPD dependent.
 */
public class VROneSDKLUTDistortion : MonoBehaviour {
	#region Variables 
	private Shader _shader;
	private Material _material;

    /*[System.Serializable]
    public class LUTDistortion
    {
        public Texture2D LUT_XR, LUT_XG, LUT_XB, LUT_YR, LUT_YG, LUT_YB;
        public string deviceName;
    }

    public LUTDistortion[] lutDistortions;*/


	#endregion

	#region Properties
	/**
	 * Using the `isMirrored` flag one can flip the LUT
	 * lookup in x-direction for the shader.
	 * 
	 * This is useful, when the LUTs for the left and right
	 * camera are the same with respect to mirroring.
	 */
	public bool isMirrored { get; set; }

	/**
	 * The VROneSDK/LUTDistortion shader, which applies 
	 * the distortion transformation to both cameras.
	 */
	public Shader shader {
		get {
			if (_shader == null) {
				// find and set shader
				_shader = Shader.Find("VROneSDK/LUTDistortion");
			}
			return _shader;
		}
	}

	/**
	 * Material created using the shader. Contain
	 * all LUTs required for applying the distortion.
	 */
	private Material material {
		get {
			if (_material == null
			    && shader != null) {
				_material = new Material (shader);
				setupMaterial ();
			}
			return _material;
		}
	}
	#endregion

	void Start () {
		if (!SystemInfo.supportsImageEffects) {
            Debug.Log("System does not support image effects");
			enabled = false;
			return;
		}
	}



	void OnRenderImage (RenderTexture sourceTexture, RenderTexture destTexture) {
		Material mat = material;
		if (shader != null && mat != null) {
			Graphics.Blit(sourceTexture, destTexture, mat);
		} else {
			Graphics.Blit(sourceTexture, destTexture); 
		}
	}

	void OnDisable () {
		if (_material) {
			DestroyImmediate(_material);
		}
	}

	/**
	 * Sets up the LUTs for the current device.
	 * All LUTs should be located in
	 * VROne/Lens/Resources/LUTs/{deviceName}/LUT_ij.png
	 * where i is one of [X, Y] and j is one of [R, G, B].
	 * 
	 * If any resource cannot be found, distortion
	 * will be disabled for this camera.
	 */
	private void setupMaterial () {
		// by default LUTs are loaded from resources folder, provided via Unity, 
		bool isResource = true;

		// default device 
		var deviceName = VROneSDKDevice.sharedInstance.modelName;

		// lut name used for file name generation
		var lutName = deviceName;

		// if developer mode is enabled and a `documentsLUTName` is supplied, use LUTs supplied there
		var documentLUTName = VROneSDK.sharedInstance.documentsLUTName;
		if (documentLUTName != null && documentLUTName.Length > 0) {
			isResource = false;
			lutName = documentLUTName;
		}

		// dir separator
        var separator = "/";//System.IO.Path.DirectorySeparatorChar;

		_material.hideFlags = HideFlags.HideAndDontSave;
		_material.SetInt ("_isMirrored", isMirrored ? 1 : 0);

		foreach (char dimension in new char[] {'X', 'Y'}) {
			foreach (char color in new char[] {'R', 'G', 'B'}) {
                var fileName = "LUTs" + separator + lutName + separator + "LUT_" + dimension + color;

				Texture2D texture = null;
				if (isResource) {
					texture = Resources.Load (fileName) as Texture2D;
				} else {
					texture = VROneLUTImporter.textureFromDocuments (fileName);
				}
	
				if (texture == null) {
					//#if DEBUG
					Debug.LogError ("Texture " + fileName + " not found, but required for VROneSDK.");
					//#endif
					enabled = false;
					return;
				}

				var textureName = "_LUT" + dimension + "Tex" + color;
				material.SetTexture (textureName, texture);
			}
		}
	}
}