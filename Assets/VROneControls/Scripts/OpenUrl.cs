using UnityEngine;
using System.Collections;

namespace VROne
{
	public class OpenUrl : MenuCallback {
		#region implemented abstract members of MenuCallback

		public override void ReceiveMenuCallback (params string[] info)
		{
	#if UNITY_IOS
			Application.OpenURL ("vronemedialauncher://");
	#endif
	#if UNITY_ANDROID && !UNITY_EDITOR
			AppLauncher.AndroidAppLauncher.LaunchApp("de.zeiss.mmd.vronemedia");
	#endif
		}

		#endregion
	}
}
