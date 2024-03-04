Shader "FlexibleCelShader/Cel No Outline"
{
	Properties
	{
		_Color("Global Color Modifier", Color) = (1, 1, 1, 1)
		_ColorOverride("Global Color Override", Color) = (1, 1, 1, 0)
		_MainTex("Texture", 2D) = "white" {}
		
		_RampLevels("Ramp Levels", Range(2, 50)) = 2
		_LightScalar("Light Scalar", Range(0, 10)) = 1

		_HighColor("High Light Color", Color) = (1, 1, 1, 1)
		_HighIntensity("High Light Intensity", Range(0, 10)) = 1.5

		_LowColor("Low Light Color", Color) = (1, 1, 1, 1)
		_LowIntensity("Low Light Intensity", Range(0, 10)) = 1
	}
	
	SubShader
	{
		
		// This pass renders the object
		Cull off
		Pass
		{
			Tags{ "LightMode" = "ForwardBase" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			#pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
			#include "AutoLight.cginc"

			struct v2f
			{
				half2 uv : TEXCOORD0;
				SHADOW_COORDS(1)
				half3 worldNormal : TEXCOORD2;
				half3 worldTangent : TEXCOORD3;
				half3 worldBitangent : TEXCOORD4;
				half4 worldPos : TEXCOORD5;
				half4 pos : SV_POSITION;
			};

			v2f vert(appdata_tan v)
			{
				v2f o;

				// UV data
				o.uv = v.texcoord;

				// Position data
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.pos = mul(UNITY_MATRIX_VP, o.worldPos);

				// Normal data
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldTangent = UnityObjectToWorldNormal(v.tangent);
				o.worldBitangent = cross(o.worldTangent, o.worldNormal);
				
				// Compute shadows data
				TRANSFER_SHADOW(o);

				return o;
			}

			half4    _Color;
			half4	_ColorOverride;
			sampler2D _MainTex;
			uniform half4 _MainTex_ST;
			int       _RampLevels;
			half     _LightScalar;
			half     _HighIntensity;
			half4    _HighColor;
			half     _LowIntensity;
			half4    _LowColor;

			fixed4 frag(v2f i) : SV_Target
			{
				_RampLevels -= 1;

				// Get view direction && light direction for rim lighting
				half3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
				half3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);

				// Sample textures
				fixed4 col = tex2D(_MainTex, i.uv * _MainTex_ST.xy + _MainTex_ST.zw);

				// Get normal
				half3 worldNormal = half3(i.worldTangent * 0 + i.worldBitangent * 0 + i.worldNormal * 1);

				// Get shadow attenuation
				fixed shadow = SHADOW_ATTENUATION(i);

				// Calculate light intensity
				half intensity = dot(worldNormal, lightDirection);
				intensity = clamp(intensity * _LightScalar, 0, 1);
				
				// Factor in the shadow
				intensity *= shadow;
				
				// Determine level
				half rampLevel = round(intensity * _RampLevels);
				
				// Get light multiplier based on level
				half lightMultiplier = _LowIntensity + ((_HighIntensity - _LowIntensity) / (_RampLevels)) * rampLevel;

				// Get color multiplier based on level
				half4 highColor = (rampLevel / _RampLevels) * _HighColor;
				half4 lowColor = ((_RampLevels - rampLevel) / _RampLevels) * _LowColor;
				half4 mixColor = (highColor + lowColor) / 2;
				
				// Apply light multiplier and color
				col *= lightMultiplier;
				col *= _Color * mixColor;
				col = lerp(col, _ColorOverride, _ColorOverride.a);
	
				return col;
			}
				
			ENDCG
		} // End Main Pass


		// Shadow casting
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}

	CustomEditor "CelCustomEditor"
}