# Release Notes VR ONE
The VR ONE Unity Package supports developers to build apps for the VR ONE with Unity3D.
It provides all the basic utilities required for writing an Unity-based application
which will be presented using the VR ONE.

## Version 0.1
This first version of the VR ONE SDK consists of an Unity Package (`<vronesdk.unitypackage>`).
This package includes:

- VROne Camera Prefab (drag and drop camera configuration),
- VROne Look-Up-Table distortion Shader (for each color channel) which applies a
pre-distortion to the image, so the final distortion through the lenses is compensated,
- Readme (including a How-To-Guide and instructions on how to use the package's componentes),
- and a Demo Scene (VROneSDKDemoScene) to get you started right away and see the
results of our efforts.

Known Issues:

- Aliasing, 
- LUTs are non-final and added as example only. As of now there is only one set of LUTs for Samsung Galaxy S5 included. This set is used for all devices.

## Important

To make full use of the Unity package (in particular the pre-distortion functionality),
an Unity Pro license is required.

VROne is compatible to the following devices only:

- Apple iPhone 6
- Samsung Galaxy S5

When your game targets iPhone use `Unity 4.5.4p1` or later. In
your `PlayerSettings` configuration provide all required launch
images so that project will run at full resolution.

The VR ONE SDK is work in progress and subject to change.

## Roadmap
The upcoming version of the VR ONE SDK will include the following features:

- More LUTs for more devices and pupil distances (IPD).
- Native support for iOS and Android.
