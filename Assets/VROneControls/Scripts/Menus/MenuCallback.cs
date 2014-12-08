using UnityEngine;
using System.Collections;

/// <summary>
/// This is an abstract class that implements a callback receiver for menu icons.
/// If a menu icon is selected, any component that inherits from MenuCallback can receive a callback.
/// </summary>
public abstract class MenuCallback : MonoBehaviour {
	public abstract void ReceiveMenuCallback(params string[] info);
}
