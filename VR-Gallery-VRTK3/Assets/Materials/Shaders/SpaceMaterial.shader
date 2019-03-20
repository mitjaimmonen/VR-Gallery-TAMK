Shader "Custom/SpaceMaterial" {
	Properties {
		_ColorTT ("Color", Color) = (1,1,1,1)
		_ColorTB ("Color", Color) = (1,1,1,1)
		_ColorBT ("Color", Color) = (1,1,1,1)
		_ColorBB ("Color", Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Refract ("Refract", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Normal ("Normal", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows
		#pragma vertex vert
		#pragma target 3.0

		sampler2D _Normal;

		struct Input {
			float2 uv_Normal;
			float3 localPos;
			float3 worldPos;
		};

		void vert (inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.localPos = v.vertex.xyz;
		}

		half _Glossiness;
		half _Metallic;
		half _Refract;
		fixed4 _ColorTT;
		fixed4 _ColorTB;
		fixed4 _ColorBT;
		fixed4 _ColorBB;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			float3 localPos = IN.worldPos - mul(unity_ObjectToWorld, float4(0,0,0,1)).xyz;
			float3 normal = tex2D (_Normal, IN.uv_Normal).xyz;
			//float3 localPos = IN.localPos;
			half4 g;
			half z = 5;
			localPos = localPos + 0.5;
			half split = (sin(10*localPos.x)+cos(10*localPos.z))/20 + 0.5;
			half lerpvalue = localPos.y * 1.8;
			lerpvalue = lerpvalue + normal.g * _Refract;
			if (localPos.y > split) {
				lerpvalue = lerpvalue - 0.8;
				g = lerp(_ColorTB, _ColorTT, lerpvalue);
			} else {
				g = lerp(_ColorBB, _ColorBT, lerpvalue);
			}

			o.Albedo = g.rgb;
			//o.Normal = normal.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = 1;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
