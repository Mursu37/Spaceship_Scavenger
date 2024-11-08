Shader "XRay/OverrideVisualShader"
{
    Properties
    {
        [Enum(UnityEngine.Rendering.BlendMode)]
        _SrcFactor("Src Factor", Float) = 5
        [Enum(UnityEngine.Rendering.BlendMode)]
        _DstFactor("Dst Factor", Float) = 10
        [Enum(UnityEngine.Rendering.BlendOp)]
        _Opp("Operation", Float) = 0
        
        _MainTex ("Texture", 2D) = "white" {}
        _CheckDistance("Check distance", Range(0, 100)) = 10
        _WallColor ("Wall Color", Color) = (1,1,1,1)
        _FloorColor ("Floor Color", Color) = (1,1,1,1)
        _WallAlpha("Wall Alpha", Float) = 1
        _FloorAlpha("Floor Alpha", Float) = 1
        _FloorThreshold("Floor Threshold", float) = 0.1
    }
    SubShader
    {
        LOD 100
        Cull Off
        
        Blend [_SrcFactor] [_DstFactor]
        BlendOp [_Opp]

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
                half3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                half3 normal : TEXCOORD1;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 worldPosition : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _CheckDistance;

            fixed4 _WallColor;
            fixed4 _FloorColor;
            float _WallAlpha;
            float _FloorAlpha;
            float _FloorThreshold;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.worldPosition = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);
                float cameraDistance = distance(i.worldPosition, _WorldSpaceCameraPos);
                if (cameraDistance-0.5f > _CheckDistance) return fixed4(0,0,0,0);
                
                if (Luminance(i.normal.xyz) < _FloorThreshold)
                {
                    return fixed4(_FloorColor);
                }
                else
                {
                    return fixed4(_WallColor);
                }
            }
            ENDCG
        }
    }
}
