Shader "Hidden/CA"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CAAmount("CAAmount", float) = 0.001
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            Name "ChromaticAberration"

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _CAAmount;

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = fixed4(0, 0, 0, 1);
                // r and b channels are accessed at an offset (CAAmount)
                col.r = tex2D(_MainTex, i.uv + float2(_CAAmount, 0)).r;
                col.g = tex2D(_MainTex, i.uv).g;
                col.b = tex2D(_MainTex, i.uv - float2(_CAAmount, 0)).b;

                return col;
            }
            ENDCG
        }
    }
}
