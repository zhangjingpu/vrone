using UnityEngine;
using System.Collections;

public class OpenUrlTest : MenuCondition {
	#region implemented abstract members of MenuCondition

	public override MenuState TestCondition (params string[] info)
	{
#if UNITY_EDITOR
		bool canLaunchApp = false;
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
		bool canLaunchApp = AppLauncher.AndroidAppLauncher.CanLaunchApp ("de.zeiss.mmd.vronemedia");
#endif
#if UNITY_IOS && !UNITY_EDITOR
		bool canLaunchApp = IOSHelper.CanOpenURL ("vronemedialauncher://");
#endif
		if (canLaunchApp) return MenuState.Show;
		return MenuState.Hidden;
	}

	#endregion
}
