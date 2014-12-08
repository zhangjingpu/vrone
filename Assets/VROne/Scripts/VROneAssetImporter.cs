#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

namespace VROne
{
	/**
	 * The VROneAssetImporter imports all LUTs in the required format.
	 * This can only be done when running game in the Unity editor.
	 * 
	 * Calling method `importAssets` makes sure that all LUTs are
	 * included in the correct format.
	 */
	public class VROneAssetImporter : Object {
		public static void importAssets () {
			Debug.Log ("Importing assets with correct texture settings.");
			// loop over all available devices
			foreach (VROneSDKSupportedDeviceModel supportedDevice in System.Enum.GetValues(typeof(VROneSDKSupportedDeviceModel)))
			{
				VROneAssetImporter.importAssetsForDevice (supportedDevice);
			}
		}

		private static void importAssetsForDevice (VROneSDKSupportedDeviceModel supportedDevice) {
			foreach (char dimension in new char[] {'X', 'Y'}) {
				foreach (char color in new char[] {'R', 'G', 'B'}) {
					// load texture
					var fileName = "LUTs/" + supportedDevice.ToString () + "/" + "LUT_" + dimension + color;
					Texture2D texture = Resources.Load (fileName) as Texture2D;

					if (texture != null) {
						string path = AssetDatabase.GetAssetPath(texture);
						// apply default texture importer if required
						TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(path);
						if (   importer.textureType != TextureImporterType.Advanced
						    || importer.npotScale != TextureImporterNPOTScale.None
						    || importer.wrapMode != TextureWrapMode.Clamp
						    || importer.mipmapEnabled != false
						    || importer.filterMode != FilterMode.Bilinear
						    || importer.maxTextureSize != 2048
						    || importer.textureFormat != TextureImporterFormat.RGB24) {
							importer.textureType = TextureImporterType.Advanced;
							importer.npotScale = TextureImporterNPOTScale.None;
							importer.wrapMode = TextureWrapMode.Clamp;
							importer.mipmapEnabled = false;
							importer.filterMode = FilterMode.Bilinear;
							importer.maxTextureSize = 2048;
							importer.textureFormat = TextureImporterFormat.RGB24;
							AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
						}
					}
				}
			}
		}
	}
}
#endif // UNITY_EDITOR