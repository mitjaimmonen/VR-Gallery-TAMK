Shader "Custom/PlatformMaterial" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Edge ("Edge Thickness", Range(0,1)) = 0.03
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		Cull Off

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows
		#pragma vertex vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 pos;
		};

		void vert(inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.pos = v.vertex.xyz;
		}

		half _Edge;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			float3 pos = IN.pos + float3(0.5, 0.5, 0.5);
			float lenx = abs(length(mul(unity_ObjectToWorld, float4(1, 0, 0, 1)).xyz - mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz));
			float leny = abs(length(mul(unity_ObjectToWorld, float4(0, 1, 0, 1)).xyz - mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz));
			float lenz = abs(length(mul(unity_ObjectToWorld, float4(0, 0, 1, 1)).xyz - mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz));

			if (pos.x * lenx < _Edge && pos.y * leny < _Edge) {
				o.Emission = _Color;
			}
			else if (pos.x * lenx < _Edge && leny - pos.y * leny < _Edge) {
				o.Emission = _Color;
			}
			else if (lenx - pos.x * lenx < _Edge && pos.y * leny < _Edge) {
				o.Emission = _Color;
			}
			else if (lenx - pos.x * lenx < _Edge && leny - pos.y * leny < _Edge) {
				o.Emission = _Color;
			}

			else if (pos.z * lenz < _Edge && pos.y * leny < _Edge) {
				o.Emission = _Color;
			}
			else if (pos.z * lenz < _Edge && leny - pos.y * leny < _Edge) {
				o.Emission = _Color;
			}
			else if (lenz - pos.z * lenz < _Edge && pos.y * leny < _Edge) {
				o.Emission = _Color;
			}
			else if (lenz - pos.z * lenz < _Edge && leny - pos.y * leny < _Edge) {
				o.Emission = _Color;
			}

			else if (pos.x * lenx < _Edge && pos.z * lenz < _Edge) {
				o.Emission = _Color;
			}
			else if (pos.x * lenx < _Edge && lenz - pos.z * lenz < _Edge) {
				o.Emission = _Color;
			}
			else if (lenx - pos.x * lenx < _Edge && pos.z * lenz < _Edge) {
				o.Emission = _Color;
			}
			else if (lenx - pos.x * lenx < _Edge && lenz - pos.z * lenz < _Edge) {
				o.Emission = _Color;
			} else {
				clip(-1);
			}
			o.Albedo = float3(0, 0, 0);
			o.Metallic = 0;
			o.Smoothness = 0;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
