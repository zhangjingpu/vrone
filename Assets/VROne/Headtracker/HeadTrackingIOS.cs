using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace VROne
{
	public class HeadTrackingIOS {
		
		[DllImport ("__Internal")]
		private static extern void _StartCameraUpdates ();
		
		[DllImport ("__Internal")]
		private static extern void _GetQuaternionUpdate ([Out] out System.IntPtr ArrayReceiver);

		[DllImport ("__Internal")]
		private static extern void _ApplicationDidResume ();


		
		public static void StartCameraUpdates(){
			if (Application.platform != RuntimePlatform.OSXEditor)
				_StartCameraUpdates();
		}
		
		public static Quaternion GetQuaternionUpdate(){
			if (Application.platform != RuntimePlatform.OSXEditor){
				
				float[] q = new float[4];
				// Initialize Wannabe-Pointer
				System.IntPtr Result = System.IntPtr.Zero;
				
				// Get Start-Adress of Float-Array from C++ Method into
				//Wannabe-Pointer
				_GetQuaternionUpdate(out Result);
				
				// Use Marshal to Copy content from C++ Array to C# Array.
				//Size 4 is hardcoded.
				Marshal.Copy(Result, q, 0, 4);
				
				// Free Memory
				Marshal.FreeCoTaskMem(Result);

				return new Quaternion(q[0], q[1], q[2], q[3]);
			}
			return Quaternion.identity;
		}

		void OnApplicationResume()
		{
			_ApplicationDidResume ();
		}

	
	}
}