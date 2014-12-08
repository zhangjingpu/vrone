using UnityEngine;
using System.Collections;

namespace VROne
{
	public class CursorAttachment : MenuCallback {
		#region implemented abstract members of MenuCallback
		public override void ReceiveMenuCallback (params string[] info)
		{
			VRCursor.instance.AttachIcon (recenterPrefab);
		}

		public GameObject recenterPrefab;
		#endregion
	}
}