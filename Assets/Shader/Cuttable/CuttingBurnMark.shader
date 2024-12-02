Shader "Unlit/CuttingBurnMark"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise tex", 2D) = "white" {}
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
            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;
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
                fixed4 colMask = tex2D(_NoiseTex, i.uv);

                fixed4 noiseMask = tex2D(_NoiseTex, i.uv + frac(_Time));
                fixed4 mainTex = tex2D(_MainTex, i.uv);
                fixed4 additionMask = tex2D(_MainTex, lerp(i.uv, noiseMask.r, _ChangeRate));
                
                fixed4 col = fixed4(1, 0, 0, mainTex.r + additionMask.r);
                col.r = col.r +(colMask.r * _Red);
                col.b = col.b + (colMask.r * _Blue);
                col.g = col.g + (colMask.r * _Green);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
