Release Notes VR ONE Unity 3D SDK 1.1

* Added native head tracking plugins
* Added VR One default menu
* Updated LUTs

----

# ZEISS VR ONE SDK

Table of Contents:

* [About ZEISS VR ONE](#markdown-header-about-zeiss-vr-one)
* [What is the VR ONE SDK?](#markdown-header-what-is-the-vr-one-sdk)
* [What can I expect from the VR ONE SDK?](#markdown-header-what-can-i-expect-from-the-vr-one-sdk)
* [Wait, I can use the VR ONE SDK and sell my app without paying you a cent?](#markdown-header-wait-i-can-use-the-vr-one-sdk-and-sell-my-app-without-paying-you-a-cent)
* [When should I use VR ONE SDK?](#markdown-header-when-should-i-use-vr-one-sdk)
* [What about Head-Tracking?](#markdown-header-what-about-headtracking)
* [What are the requirements to use the Unity package?](#markdown-header-what-are-the-requirements-to-use-the-unity-package)
* [Which smartphones are currently supported?](#markdown-header-which-smartphones-are-currently-supported)
* [Give me a Demo](#markdown-header-give-me-a-demo)
* [Getting started](#markdown-header-getting-started)
	* [Changing the camera distance (mono / stereo)](#markdown-header-changing-the-camera-distance-mono-stereo)
	* [Enabling / disabling VR ONE](#markdown-header-enabling-disabling-vr-one)
	* [Enabling / disabling the pre-distortion](#markdown-header-enabling-disabling-the-pre-distortion)
* [How can I contribute to VR ONE SDK development?](#markdown-header-how-can-i-contribute-to-vr-one-sdk-development)

## About ZEISS VR ONE

ZEISS VR ONE is a head-mounted virtual reality device, compatible with various smartphones. For more information, please go to [http://zeissvrone.tumblr.com/](http://zeissvrone.tumblr.com/).

## What is the VR ONE SDK?

The VR ONE SDK is a VR SDK for Unity and aims to help developers to create great virtual reality experiences for the VR ONE head-mounted virtual reality device. For now, it supports mobile apps made with Unity3D for iOS and Android platforms. 

## What can I expect from the VR ONE SDK?

The VR ONE SDK features an open source, MIT-licensed Unity package, which provides a *split sceen* and/or *stereo two-camera* setup and applies a *configurable radial and chromatic pre-distortion effect* to prepare the image for optimal display in the VR ONE for various smartphone models. 

The SDK is currently in an experimental stage. A final release as well as the complete code base will be provided once the VR ONE becomes available for sale.

![Split-screen example](http://root.innoactive.de/c/zeiss/vrone/example1.jpg "The split screen created by VR ONE SDK") ![Pre-distortion example](http://root.innoactive.de/c/zeiss/vrone/example2.jpg "The pre-distortion effect created by VR ONE SDK")

## Wait, I can use the VR ONE SDK and sell my app without paying you a cent?

You got it. The VR ONE SDK is supposed to help you jumpstart your app development.

## When should I use VR ONE SDK?

If you are a developer who aims to develop or adapt your mobile app for VR ONE with Unity, you will be able to jumpstart your Unity project by utilizing the VR ONE SDK. If you are using native development in Android or iOS, we will shortly support you with tutorials in the VR ONE wiki.

## What about Head-Tracking?
Together with the VR One Unity 3D SDK, Zeiss makes a sophisticated multi-sensor Head Tracking plugin, for the ultimate Virtual Reality experience.  By default, it is already activated in the VR One Unity Package, but if you wish to use your own head-tracking, you can easily deactivate it.

The Head-Tracking plugin provided by Zeiss makes use of all the motion sensors available on your smartphone to optimise the latency between head-movement and camera update as much as possible. This is provided as a .a library for iOS, and a .jar library for Android. A sample use for both the platforms can be found in the provided Demo Scene. 

## What are the requirements to use the Unity package?

For using the pre-distortion functionality, you need a Unity Pro license with at least a iOS or Android platform license. If you use our integrated iOS Head-Tracking, you will need to add the SceneKit framework to the [Link Binary With Libraries] Build Phase. 

## Which smartphones are currently supported?

The VR ONE SDK will work with the *iPhone 6 as well as most recent Android 4 smartphones* right out of the box. 

To maximize user experience, we add pre-distortion effects optimized for dedicated smartphones. For beginning, we include pre-distortion support for the following models.

* Apple iPhone 6
* Samsung Galaxy S5

*Important note:* Compatibility is being extended in future. This list is being updated accordingly.

Please also check the availability of VR ONE smartphone drawers for certain models on the tumblr blog: http://zeissvrone.tumblr.com/

When your game targets iPhone use Unity 4.5.4p1 or later. In your PlayerSettings configuration provide all required launch images so that project will run at full resolution. Also set Target Device to iPhone only.

## Give me a Demo

The `<vronesdk.unitypackage>` can simply be dragged into an empty Unity project and the the scene in the Demo/Scenes folder can be opened. This will setup the VROneSDK and a Scene with a spinning cube and three lights.

## Getting started

Using the VR ONE SDK is very simple:

1. Download the `<vronesdk.unitypackage>` and drag it into your Unity project. The `<vronesdk.unitypackage>` provides the `VROneSDK` prefab in the `VROneSDK` folder. The VR ONE SDK is intended to replace the cameras in your scene. 
2. Drag the VR ONE SDK into your scene, and remove any other camera from your scene. 
3. You can then associate the `VROneSDK`, the `VROneSDKHead`, and its attached GameObjects with your custom scripts for head tracking and gameplay.

The VROne SDK requests 60fps on every device. Additionally interface orientation in Unity should be set to landscape left.

### Changing the camera distance (mono / stereo)

The camera view can be adjusted through the VROneSDK singleton. By simply
getting ahold of the shared instance and setting the IPD to the preferred
value.  A value of zero (0) would indicate that both cameras are on top of
each other and are rendering the identical image (e.g. mono). Increasing
the IPD value results in a stereo image with increasing distance between
the eyes.

*Example*

```
VROneSDK.sharedInstance.IPD = 0.0;
```

would produce a mono (two identical images side by side) view. Similarly

```
VROneSDK.sharedInstance.IPD = VROneSDK.VROneSDKDefaultIPD;
```

would set the IPD to `0.065` and result in a stereo view (the two images are
rendered from slightly differing viewports).


You can all well set `runsInStereoMode` to enable or disable mono-/stereo-mode:

```
VROneSDK.sharedInstance.runsInStereMode = true;
```

### Enabling / Disabling VR ONE
When user hasn't put on the VR ONE yet, use 

```
VROneSDK.sharedInstance.isVROneEnabled = false;
```

to disable splitting the view and present a regular screen to the user.
Only one camera will be used for rendering the scene and no distortion will be applied.

Once your game starts set this value to `true`  and thus enable the full VROne experience for the user.

### Disabling / Enabling the pre-distortion

The applied distortion can be disabled (e.g. for testing without the VR ONE)
if needed by deactivating the `VROne SDKLUTDistortion` script on the left
and right eye of the `VROneSDK.prefab`.

### Building the application
After finishing your application in Unity, you will want to export it for use with the iPhone or Samsung Galaxy S5. In Unity, simply go to File/Build Settings, select iOS or Android, and Build. 

For Android, that is all you need to do. For iOS, however, it is important to note that after the exported XCode file is built, it is necessary to add the SceneKit framework. To do this, select the application target in XCode, go to Build Phases, and under "Link Binary with Libraries", add SceneKit.framework. 


## Where can I ask technical questions?

Please check Stack Overflow for already existing Q&A’s:
[http://stackoverflow.com/questions/tagged/vrone](http://stackoverflow.com/questions/tagged/vrone)

If your question has not already been answered, feel free to submit your question at StackOverflow. Please add the tags „vrone“ and „unity3d“ to your question. The developer team will do its best to answer your question as fast and good as possible.
[http://stackoverflow.com/questions/ask](http://stackoverflow.com/questions/ask)

## How can I contribute to VR ONE SDK development?

* Use the SDK and give us direct feedback [by mail](mailto:vrone@zeiss.com?subject=VR ONE SDK development)
* Help other users by answering their questions at [stackoverflow](http://stackoverflow.com/questions/tagged/vrone)
* submit any issues in the [bitbucket issue tracker](https://bitbucket.org/vrone/unity3d/issues?status=new&status=open)
* help us to fix issues and contribute by actively developing with us.