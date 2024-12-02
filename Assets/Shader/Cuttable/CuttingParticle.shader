Shader "Unlit/CuttingParticle"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR] _Color ("Color", Color) = (1,1,1,1)
        _Red ("Red", Float) = 1
        _Green ("Green", Float) = 0.1
        _Blue ("Blue", Float) = 0.1
        _ChangeRate ("Change rate", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
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
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _ChangeRate;
            float _Green;
            float _Blue;
            float _Red;

            v2f vert (appdata v)
            {
                v2f o;
                float3 vert = v.vertex;
                o.vertex = UnityObjectToClipPos(vert);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 colMask = tex2D(_MainTex, i.uv);
                return float4(_Color.rgb, colMask.a);
            }
            ENDCG
        }
    }
}
