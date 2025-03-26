Shader "Unlit/connectionFlow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SecondMask ("Texture", 2D) = "white" {}
        [HDR] _Color ("Color", Color) = (0, 1, 0, 1)
        
        _Speed ("Speed", Float) = 1
        _VerticalCulling ("Vertical Culling", Float) = 0
        
        [Enum(UnityEngine.Rendering.BlendMode)]
        _SrcFactor("Src Factor", Float) = 5
        [Enum(UnityEngine.Rendering.BlendMode)]
        _DstFactor("Dst Factor", Float) = 10
        [Enum(UnityEngine.Rendering.BlendOp)]
        _Opp("Operation", Float) = 0
    }
    SubShader
    {
        LOD 100
        
        Blend [_SrcFactor] [_DstFactor]
        BlendOp [_Opp]
        
        ZTest Always

        Pass
        {
            Tags { "RenderType"="Transparent" "Queue"="Transparent"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 worldPosition : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _SecondMask;
            float4 _Color;
            float2 _AnimateXY;
            float _Speed;
            float _VerticalCulling;
            float _CheckDistance;

            v2f vert (appdata v)
            {
                _AnimateXY = float2(_Speed, 0);
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPosition = mul(unity_ObjectToWorld, v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv += frac(_AnimateXY * _Time.yy);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float dist = distance(i.worldPosition, _WorldSpaceCameraPos);
                if (dist > _CheckDistance) return fixed4(0,0,0,0);
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 col2 = tex2D(_SecondMask, i.uv);
                float reduction;
                if (i.uv.y > 0.5)
                {
                    reduction = smoothstep(0.5, 1, i.uv.y);
                }
                else
                {
                    reduction = smoothstep(0.5, 0, i.uv.y);
                }
                reduction *= _VerticalCulling * reduction;
                col.r = col.r * 0.8 + col2.r * 0.2;
                col.r -= reduction;
                if (col.r < 0) col.r = 0;
                _Color.a = col.r - _Color.a;
                if (_Color.a <0) _Color.a = 0;
                return _Color;
            }
            ENDCG
        }
    }
}
