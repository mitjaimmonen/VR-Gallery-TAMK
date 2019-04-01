Shader "Custom/SpaceMaterial" {
	Properties {
		_ColorTT ("Top Gradient From", Color) = (1,1,1,1)
		_ColorTB ("Top Gradient To", Color) = (1,1,1,1)
		_ColorBT ("Bottom Gradient From", Color) = (1,1,1,1)
		_ColorBB ("Bottom Gradient To", Color) = (1,1,1,1)
		_LerpThreshold ("Interpolate gradient switch", Range(0,1)) = 0.1
		_Emission ("Emission", Range(0,1)) = 0.1
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
			float3 worldPos;
			float3 normal;
		};

		void vert (inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.normal = v.normal.xyz;
		}

		half _Glossiness;
		half _Metallic;
		half _Refract;
		half _Shine;
		half _LerpThreshold;
		half _Emission;
		fixed4 _ColorTT;
		fixed4 _ColorTB;
		fixed4 _ColorBT;
		fixed4 _ColorBB;
		fixed4 _ColorShine;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			half4 g;
			//Get local position from world position
			float3 localPos = IN.worldPos - mul(unity_ObjectToWorld, float4(0,0,0,1)).xyz;
			localPos = localPos + 0.5;

			//Get view nomals
			float3 viewSpacePos = -mul(UNITY_MATRIX_V, float4(IN.worldPos, 1.0)).xyz;

			//Get depth
			float3 depth = 1 - clamp(length(viewSpacePos.z)/10,0.01,1);

			//Get world normals
			float3 worldNormals = mul(unity_ObjectToWorld, float4(IN.normal,0)).xyz;
			worldNormals.y = worldNormals.y * IN.worldPos.y / abs(IN.worldPos.y);

			//Get Normal Map
			float3 normalMap = tex2D (_Normal, IN.uv_Normal).xyz;

			//Calculate gradient split
			half z = 5;
			half split = (sin(10*localPos.x + _Time.y)+cos(10*localPos.z + _Time.y))/20 + 0.5;
			split = split + (normalMap.g - 0.5) * _Refract;

			//Interpolate gradients
			half lerpvalue = abs(localPos.y);
			if (worldNormals.y > _LerpThreshold) {
				g = lerp(_ColorTB, _ColorTT, lerpvalue);
			} else if (worldNormals.y <= -_LerpThreshold){
				g = lerp(_ColorBB, _ColorBT, lerpvalue);
			} else {
				g = lerp(lerp(_ColorBB, _ColorBT, lerpvalue), lerp(_ColorTB, _ColorTT, lerpvalue), (worldNormals.y/_LerpThreshold)/2+0.5);
			}

			//g = g * viewSpacePos.z/;

			//Add colour shine
			//float3 absNormal = normalize(abs(UnityObjectToViewPos(IN.normal)));
			//g.rgb = lerp(g.rgb, _ColorShine, pow(dot(absNormal, float3(0,0,1)), 4)*_Shine);

			//Assign albedo
			o.Albedo = g.rgb;
			o.Emission = g.rgb * depth * _Emission; //g.rgb * _Emission;
			//o.Normal = normalMap.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = 1;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
