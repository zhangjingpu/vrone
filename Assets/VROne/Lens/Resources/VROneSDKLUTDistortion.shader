//
// Copyright (C) 2014 - Carl Zeiss AG
//

// Distortion Shader
Shader "VROneSDK/LUTDistortion" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		// We have a set of LUTs.
		// for each color two. One for the x-direction
		// and one for the y-direction. _LUT<direction>Tex<Color>
		_LUTXTexR ("LUTXR", 2D) = "white" {}
		_LUTYTexR ("LUTYR", 2D) = "white" {}
		_LUTXTexG ("LUTXG", 2D) = "white" {}
		_LUTYTexG ("LUTYG", 2D) = "white" {}
		_LUTXTexB ("LUTXB", 2D) = "white" {}
		_LUTYTexB ("LUTYB", 2D) = "white" {}
	}
	SubShader {
		Pass {
			CGPROGRAM
			// We need to define a vertex shader even though
			// we don't use one.  Without this is an invalid
			// shader for GLES.
			#pragma target 3.0
			#pragma vertex vert_img
			// Fragement shader is function frag.
			#pragma fragment frag
			// Use unity
			#include "UnityCG.cginc"
			// We will use two variables _MainTex contains
			// the source texture
			uniform sampler2D _MainTex;
			
			// One Texture for each LUT (direction and color).
			uniform sampler2D _LUTXTexR;
			uniform sampler2D _LUTYTexR;
			uniform sampler2D _LUTXTexG;
			uniform sampler2D _LUTYTexG;
			uniform sampler2D _LUTXTexB;
			uniform sampler2D _LUTYTexB;

			// mirror flag for right eye (textures are for left
			// eye and mirrored for right eye)
			uniform bool _isMirrored;

			// Decoding a color value from the
			// texture into a float.  Similar to unitys
			// DecodeFloatRGBA and DecodeFloatRG.
			float DecodeFloatRGB(float3 rgb) {
				return dot(rgb, float3(1.0,1.0/255,1.0/65025.0));
			}
			
			float mirrored(float coord) {
				if (_isMirrored) {
					return 1.0f - coord;
				}
				return coord;
			}

			// compute new lookup position from a LUT.
			// For each color we extract the rgb value at the
			// coordinate we are interested in.  This rgb value
			// indicates which pixel (or interpolated pixel) we
			// need to map.
			float2 LUTDistortionR(float2 coord)
			{
				float3 lookupX = tex2D(_LUTXTexR, coord).rgb;
				float3 lookupY = tex2D(_LUTYTexR, coord).rgb;
				return float2(mirrored(DecodeFloatRGB(lookupX)),DecodeFloatRGB(lookupY));
			}

			float2 LUTDistortionG(float2 coord)
			{
				float3 lookupX = tex2D(_LUTXTexG, coord).rgb;
				float3 lookupY = tex2D(_LUTYTexG, coord).rgb;
				return float2(mirrored(DecodeFloatRGB(lookupX)),DecodeFloatRGB(lookupY));
			}

			float2 LUTDistortionB(float2 coord)
			{
				float3 lookupX = tex2D(_LUTXTexB, coord).rgb;
				float3 lookupY = tex2D(_LUTYTexB, coord).rgb;
				return float2(mirrored(DecodeFloatRGB(lookupX)),DecodeFloatRGB(lookupY));
			}
			
			float4 frag(v2f_img i) : COLOR
			{
				// our result will be initialized to 0/0/0.
				float3 res = float3(0.0f,0.0f,0.0f);
				// Get the target (u,v) coordinate (i.uv)
				// which is where we will draw the pixel.
				// What we will draw, depends on the color
				// and the distortion, which we can look up in
				// the LUT.  We do this for each color and do
				// not put xy in rb or similar to allow us to
				// improve precision with the DecodeFloatRGB method,
				// as can be seen above.
				
				// since textures are for left eye only, we need to
				// "mirror" the input coordinate for the right eye.
				float2 coord = float2(mirrored(i.uv.x), i.uv.y);

				float2 xyR = LUTDistortionR(coord);
				if (xyR.x <= 0.0f || xyR.y <= 0.0f || xyR.x >= 1.0f || xyR.y >= 1.0f) {
					// set alpha to 1 and return.
					return float4(res, 1.0f);
				}

				float2 xyG = LUTDistortionG(coord);
				if (xyG.x <= 0.0f || xyG.y <= 0.0f || xyG.x >= 1.0f || xyG.y >= 1.0f) {
					// set alpha to 1 and return.
					return float4(res, 1.0f);
				}

				float2 xyB = LUTDistortionB(coord);
				if (xyB.x <= 0.0f || xyB.y <= 0.0f || xyG.x >= 1.0f || xyG.y >= 1.0f) {
					// set alpha to 1 and return.
					return float4(res, 1.0f);
				}

				res = float3(tex2D(_MainTex,xyR).r,
							 tex2D(_MainTex,xyG).g,
							 tex2D(_MainTex,xyB).b);

				// set alpha to 1 and return.
				return float4(res, 1.0f);
			}
			ENDCG
		}
	}
}