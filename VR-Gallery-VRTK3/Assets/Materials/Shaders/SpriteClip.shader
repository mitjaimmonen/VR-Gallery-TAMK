// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


//--------------------
//taken from https://stackoverflow.com/questions/23165899/unity3d-shader-for-sprite-clipping
//--------------------

Shader "Sprites/ClipRange"
{
Properties
{
    _MainTex ("Base (RGB), Alpha (A)", 2D) = "white" {}

	//Edited code
    _ClipRange ("Clip Threshold", Range(0.0, 1.0)) = 0
	_EmissionColor ("Edge Color", Color) = (1,1,1,1)
	//End edited code
}

SubShader
{
    LOD 200

    Tags
    {
        "Queue" = "Transparent"
        "IgnoreProjector" = "True"
        "RenderType" = "Transparent"
    }

    Pass
    {
        Cull Off
        Lighting Off
        ZWrite Off
        Offset -1, -1
        Fog { Mode Off }
        ColorMask RGB
        Blend SrcAlpha OneMinusSrcAlpha

        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag

        #include "UnityCG.cginc"

        sampler2D _MainTex;
        float4 _MainTex_ST;
		float4 _EmissionColor;
        float _ClipRange;

        struct appdata_t
        {
            float4 vertex : POSITION;
            float2 texcoord : TEXCOORD0;
        };

        struct v2f
        {
            float4 vertex : POSITION;
            float2 texcoord : TEXCOORD0;
        };

        v2f vert (appdata_t v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.texcoord = v.texcoord;
            return o;
        }

        half4 frag (v2f IN) : COLOR
        {	
			//Edited code here
			float4 col = tex2D(_MainTex, IN.texcoord);
			
            if (IN.texcoord.y > _ClipRange || _ClipRange == 0)
            {
                half4 colorTransparent = half4(0,0,0,0) ;
                return  colorTransparent ;
            }
			float w = clamp((_ClipRange - IN.texcoord.y) * 10, 0, 1);
			if (_ClipRange == 1) {
				w = 1;
			}
			col = lerp(_EmissionColor, col, w);
            return col;
			//End edited code
        }
        ENDCG
    }
}
}
