// Author: Tobias Löf Melker
// Created: 2020 for usage with Unity Engine

Shader "Custom/ThumbnailGenerator/FrameShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Frame("Frame", 2D) = "white" {}
		_Mask("Mask", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha
		Tags { "PreviewType" = "Plane" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
			sampler2D _Frame;
			sampler2D _Mask;


            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 mask = tex2D(_Mask, i.uv);
				fixed4 frame = tex2D(_Frame, i.uv);
				col *= mask;
				
				// overlay blend - Blend SrcAlpha OneMinusSrcAlpha
				col.rgb = (frame.rgb * frame.a + col.rgb * col.a * (1 - frame.a)) / (frame.a + col.a * (1 - frame.a));
				col.a = frame.a + col.a * (1 - frame.a);
            
				return col;
            }
            ENDCG
        }
    }
}
