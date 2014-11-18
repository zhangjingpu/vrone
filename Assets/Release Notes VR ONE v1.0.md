# Release Notes VR ONE
The VR ONE Unity Package supports developers to build apps for the VR ONE with Unity3D.
It provides all the basic utilities required for writing an Unity-based application
which will be presented using the VR ONE.

## Version 1.0
This first version of the VR ONE SDK consists of an Unity Package (`<vronesdk.unitypackage>`).
This package includes:

- VROne Camera Prefab (drag and drop camera configuration),
- VROne Look-Up-Table (LUT) distortion Shader (for each color channel) which applies a
pre-distortion to the image, so the final distortion through the lenses is compensated. For each device, 3 sets of LUTs is provided, for different pupile distance (small, default, large). This is to account for the fact that different people have wider or narrower faces. 
- Readme (including a How-To-Guide and instructions on how to use the package's componentes),
- Demo Scene (VROneSDKDemoScene) to get you started right away and see the
results of our efforts.

Known Issues:

- Aliasing

## Important

To make full use of the Unity package (in particular the pre-distortion functionality),
an Unity Pro license is required.

VROne is currently compatible with the following devices:

- Apple iPhone 6
- Samsung Galaxy S5

If your game targets iPhone please use `Unity 4.5.4p1` or later. In
your `PlayerSettings` configuration provide all required launch
images so that project will run at full resolution.

The VR ONE SDK is work in progress and subject to change.

## Roadmap
The upcoming version of the VR ONE SDK will include the following features:

- More LUTs for more devices.
- Native support for iOS and Android.
- Head tracking support
