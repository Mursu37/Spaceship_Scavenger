Shader "Custom/NormalDepthExclusion"
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
        _Color ("Color", Color) = (1,1,1,1)
        //_CheckDistance("Check distance", Range(0, 100)) = 10
        _Alpha("Alpha", Float) = 1
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
                float4 worldPosition : TEXCOORD2;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _CheckDistance;
            float _Alpha;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.worldPosition = mul(unity_ObjectToWorld, v.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);
                float dist = distance(i.worldPosition, _WorldSpaceCameraPos);
                if (dist > _CheckDistance) return fixed4(0,0,0,0);
                if (dist >= _CheckDistance - 0.2)
                {
                    return fixed4(0,0, 1, 1);
                }
                fixed4 col = fixed4((i.normal.xyz + 1) / 2, 1);
                //float y = Luminance(col);
                //return fixed4(y,y,y, _Alpha);
                
                return fixed4(col.rg, 0, _Alpha);
            }
            ENDCG
        }
    }
}
