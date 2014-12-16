# Release Notes VR ONE Unity 3D SDK 1.1

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
* [What about Head-Tracking?](#markdown-header-what-about-head-tracking)
* [What are the requirements to use the Unity package?](#markdown-header-what-are-the-requirements-to-use-the-unity-package)
* [What else do you provide?](#markdown-header-what-else-do-you-provide)
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

## What about head-tracking?
Together with the VR One Unity 3D SDK, ZEISS offers a multi-sensor head tracking plugin. It is already activated by default in the VR One Unity Package, but if you wish to use your own head-tracking, you can easily deactivate it.

The head-tracking plugin provided by ZEISS makes use of all the motion sensors available on your smartphone to reduce latency between head-movement and camera update as much as possible. The plugin is is provided as a .a library for iOS, and a .jar library for Android. A sample use for both the platforms can be found in the provided demo scene. 

### How do I use the head-tracking library natively, without Unity? 

#### iOS
The VR One Head Tracking plugin (which can be found [here](https://bitbucket.org/vrone/unity3d/src/046156661b834aca2b5a242c85f756ed044574ca/Assets/Plugins/iOS/libVROneHeadTracking.a?at=master)) consists of three functions: 

`    void _StartCameraUpdates();`
Initialiser function. Should be called once.


`    void _GetQuaternionUpdate(float** arrayToSave);`
Calculates the camera rotation update, and should thus be called on continually. The parameter it receives is a pointer to the float array where the Quaternion values should be saved. As a quaternion contains four components, at the end of the function call this array contains the components ordered in x,y,z,w. This quaternion should then be directly used in the camera rotation update.


`    void _ApplicationDidResume ();`
Re-calibrates the view's zero reference. Can be called, for instance, from applicationWillEnterForeground. Not calling this function will result in a small drifting each time the applications re-opens, for the camera was set to the wrong direction,

It is also important to note that this library requires the use of `CoreMotion.framework` and `SceneKit.framework`, which should be added to XCode's `Link Binary with Libraries` Build Phases.

#### Android
In order to use the .jar (available [here](https://bitbucket.org/vrone/unity3d/src/046156661b834aca2b5a242c85f756ed044574ca/Assets/Plugins/Android/androidsensorfusion.jar?at=master)) VR One Head-Tracking library with your native Android application, simply `import de.zeiss.mmd.headtracking` to your project, and you can then call the `public static double[] getRotation(Context context)` function. For the calculations done internally, the function requires the context it is called from. As the head tracking is optimized for 60 fps, the function should be called approximatly 60 times per second. The double array delivered represents the four Quaternion components, ordered as x, y, z, and w. 


## What are the requirements to use the Unity package?

For using the pre-distortion functionality, you need a Unity Pro license with at least a iOS or Android platform license. If you use our integrated iOS Head-Tracking, you will need to add the SceneKit framework to the [Link Binary With Libraries] Build Phase. 

## What else do you provide?

Included in the VR One Unity SDK is a simple Menu-prefab, which Zeiss recommends to be integrated into any application. The menu provides two basic functionality: 

1. View recenter, to allow the user to reposition his/her head and have the view adjusted to it.
2.  Jump to VR One Media Launcher button. 

By allowing the user to navigate between VR apps directly through head-tracking accessible buttons, the VR One aims provide a consistent virtual reality experience across all the supported apps. 

In the demo scene you can see a basic menu set up with a recenter icon and the jump to launcher icon.
You can create your own menu by creating an empty game object and adding the script component `Menu.cs` to it. Each menu is naturally consisted of several buttons. The prefab also allows you to define the layout of your menu.

It is important to note that you need to define a hot spot object that triggers the menu. Create such an object and add `SelectableObject.cs` to it. Next, drag and drop the menu gameobject into "Menu Prefab". You can set the menu to be active on start, define a selection time and a progress bar.

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