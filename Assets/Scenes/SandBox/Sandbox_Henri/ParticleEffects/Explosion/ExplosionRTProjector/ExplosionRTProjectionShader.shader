Shader "Unlit/ExplosionRTProjectionShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ProjectedTex ("RenderTexture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            sampler2D _ProjectedTex;
            float4x4 _CustomMatrix_VP;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;  // Screen-space position
                float4 projUV : TEXCOORD0; // Projected texture coordinates
            };

            v2f vert(appdata v)
            {
                v2f o;
                // Transform vertex to clip space
                o.pos = UnityObjectToClipPos(v.vertex);

                // Transform vertex to world space
                float4 worldPos = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0));

                // Transform world position into projector space
                o.projUV = mul(_CustomMatrix_VP, worldPos);

                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                // Perform perspective divide for UV mapping
                float2 uv = i.projUV.xy / i.projUV.w;

                // Sample the texture
                return tex2D(_ProjectedTex, uv);
            }
            ENDCG
        }
    }
}