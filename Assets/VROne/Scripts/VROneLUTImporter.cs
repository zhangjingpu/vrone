/*
 * Copyright (C) 2014 - Carl Zeiss AG
 */

using UnityEngine;
using System.Collections;

namespace VROne
{
	public class VROneLUTImporter : Object {

		/**
		 * Returns the path which contains user defined LUTs.
		 * All LUTs need to be located inside folder `LUTs`
		 * in the `Application.persistentDataPath` folder.
		 */
		public static string documentLUTPath () {
			return System.IO.Path.Combine(Application.persistentDataPath, "LUTs");
		}

		/**
		 * Returns a list of all LUTs path names which can be
		 * found inside the `documentLUTPath` folder.
		 */
		public static string[] availableDocumentLUTs () {
			var lutPath = documentLUTPath ();
			var availableLuts = new System.Collections.Generic.List <string> ();
			if (System.IO.Directory.Exists(lutPath)) {
				foreach (string entry in System.IO.Directory.GetFileSystemEntries(lutPath)) {
					if (isLUTPath (entry)) {
						availableLuts.Add (System.IO.Path.GetFileName (entry));
					}
				}
			}
			return availableLuts.ToArray ();
		}

		/**
		 * Checks if all required LUT files can be found inside
		 * the given `path`.
		 * Required files are:
		 * LUT_XR.png, LUT_XG.png, LUT_XB.png and
		 * LUT_YR.png, LUT_YG.png, LUT_YB.png.
		 */
		private static bool isLUTPath (string path) {
			if (System.IO.Directory.Exists (path)) {
				foreach (char dimension in new char[] {'X', 'Y'}) {
					foreach (char color in new char[] {'R', 'G', 'B'}) {
						if (!System.IO.File.Exists (System.IO.Path.Combine(path, "LUT_" + dimension + color + ".png"))) {
							return false;
						}
					}
				}
			}
			return true;
		}

		/**
		 * Returns the LUT with given file name.
		 * If required it loads this file from the system.
		 */
		public static Texture2D textureFromDocuments (string fileName) {
			var path = Application.persistentDataPath + System.IO.Path.DirectorySeparatorChar + fileName + ".png";
			Texture2D texture = null;
			// if file does exist, read it, otherwise return null and thus disable pre-distortion
			if (System.IO.File.Exists (path)) {
				byte[] fileData = System.IO.File.ReadAllBytes (path);
				texture = new Texture2D (2, 2, TextureFormat.RGB24, false);
				texture.LoadImage (fileData);
			}
			return texture;
	}
}
}
