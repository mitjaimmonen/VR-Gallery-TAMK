Shader "Custom/InvertCulling" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags {"Queue" = "Transparent" "RenderType"="Transparent" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			float3 normal;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			float3 localPos = IN.worldPos - mul(unity_ObjectToWorld, float4(0,0,0,1)).xyz;
			//localPos.z += 0.5;
			localPos = mul(UNITY_MATRIX_V, float4(localPos, 0)).xyz;
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = 	c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;

			float3 cameraDir = normalize(-UnityObjectToViewPos(float3(0,0,0)));
			float3 fadeValue = abs(((-cameraDir * 0.5)-(localPos)).z);
			//o.Alpha = clamp(localPos.z * localPos.z,0,1);
			o.Alpha = (sin((fadeValue * fadeValue + 0.5) * 3.1416) + 1) * 0.5;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
