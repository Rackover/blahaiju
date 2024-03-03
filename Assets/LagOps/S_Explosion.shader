// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Unlit/S_Explosion"
{
	Properties
	{
		_Texture("Texture", 2D) = "white" {}
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_PanningSpeedMultiplier("PanningSpeedMultiplier", Range( 0 , 1)) = 0.2
		_FillAmount("FillAmount", Range( 0 , 1)) = 0
		_EmissiveMultiplier("EmissiveMultiplier", Range( 1 , 100)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform sampler2D _Texture;
		uniform float _PanningSpeedMultiplier;
		uniform float _EmissiveMultiplier;
		uniform float _FillAmount;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float mulTime4 = _Time.y * _PanningSpeedMultiplier;
			float temp_output_5_0 = ( mulTime4 * -0.3 );
			float4 appendResult3 = (float4(mulTime4 , temp_output_5_0 , 0.0 , 0.0));
			float2 uv_TexCoord2 = i.uv_texcoord + appendResult3.xy;
			o.Emission = ( tex2D( _Texture, uv_TexCoord2 ) * _EmissiveMultiplier ).rgb;
			o.Alpha = 1;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 break11 = ( ( temp_output_5_0 + ase_vertex3Pos ) * 10.0 * UNITY_PI );
			float clampResult17 = clamp( ( sin( ( break11.x + break11.y + break11.z ) ) + cos( ( break11.x + break11.y + ( break11.z * -1.0 ) ) ) + ( (-1.0 + (_FillAmount - -0.2) * (1.0 - -1.0) / (1.05 - -0.2)) * UNITY_PI ) ) , 0.0 , 1.0 );
			clip( clampResult17 - _Cutoff );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
110;256;1707;645;687.376;-350.3518;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;6;-1698.2,145.294;Inherit;False;Property;_PanningSpeedMultiplier;PanningSpeedMultiplier;2;0;Create;True;0;0;0;False;0;False;0.2;0.594;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;4;-1371.57,148.8315;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-1138.093,207.7899;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-0.3;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;7;-990.0966,326.0081;Inherit;True;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;10;-925.4106,530.4392;Inherit;False;Constant;_GridScale;GridScale;3;0;Create;True;0;0;0;False;0;False;10;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;18;-732.5635,184.7065;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PiNode;23;-828.2842,643.9381;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-486.4233,279.2874;Inherit;True;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-541.1215,585.8223;Inherit;False;Constant;_MinusOne;MinusOne;3;0;Create;True;0;0;0;False;0;False;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;11;-285.8659,280.427;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-377.0295,757.8914;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-128.8797,726.6826;Inherit;False;Property;_FillAmount;FillAmount;3;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;16;-155.959,284.9852;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;3;-971.8409,86.58318;Inherit;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.PiNode;37;3.108387,915.4641;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;21;-166.2155,503.7754;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;38;200.39,691.1569;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-0.2;False;2;FLOAT;1.05;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;20;-10.09959,492.3803;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;8;-7.820334,266.7527;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;382.8078,756.0172;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-700.8,29.8584;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;22;181.6221,275.7491;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-455.6683,-3.243802;Inherit;True;Property;_Texture;Texture;0;0;Create;True;0;0;0;False;0;False;-1;f083ca2560fe4444fbf1aaf15b29d69d;f083ca2560fe4444fbf1aaf15b29d69d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;40;506.5846,126.4264;Inherit;False;Property;_EmissiveMultiplier;EmissiveMultiplier;4;0;Create;True;0;0;0;False;0;False;1;1;1;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;883.0685,-2.370584;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-372.4713,651.9146;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-374.7502,544.7988;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;17;416.0863,272.4501;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1146.365,26.48804;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Unlit/S_Explosion;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;TransparentCutout;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;6;0
WireConnection;5;0;4;0
WireConnection;18;0;5;0
WireConnection;18;1;7;0
WireConnection;9;0;18;0
WireConnection;9;1;10;0
WireConnection;9;2;23;0
WireConnection;11;0;9;0
WireConnection;29;0;11;2
WireConnection;29;1;26;0
WireConnection;16;0;11;0
WireConnection;16;1;11;1
WireConnection;16;2;11;2
WireConnection;3;0;4;0
WireConnection;3;1;5;0
WireConnection;21;0;11;0
WireConnection;21;1;11;1
WireConnection;21;2;29;0
WireConnection;38;0;35;0
WireConnection;20;0;21;0
WireConnection;8;0;16;0
WireConnection;36;0;38;0
WireConnection;36;1;37;0
WireConnection;2;1;3;0
WireConnection;22;0;8;0
WireConnection;22;1;20;0
WireConnection;22;2;36;0
WireConnection;1;1;2;0
WireConnection;39;0;1;0
WireConnection;39;1;40;0
WireConnection;28;0;11;1
WireConnection;28;1;26;0
WireConnection;25;0;11;0
WireConnection;25;1;26;0
WireConnection;17;0;22;0
WireConnection;0;2;39;0
WireConnection;0;10;17;0
ASEEND*/
//CHKSM=14482731EE214392CD80BA70DBE43E962A40E0AD