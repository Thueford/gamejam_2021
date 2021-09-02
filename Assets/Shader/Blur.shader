Shader "Hidden/Blur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _horizontal("Horizontal", int) = 1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            Name "Blur"

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            // Weight of Pixels depending on their offset
            static const float weight[3] = { 0.2270270270, 0.3162162162, 0.0702702703 };
            static const float offset[3] = { 0.0, 1.3846153846, 3.2307692308 };

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
            float4 _MainTex_TexelSize;
            int _horizontal;

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * weight[0];
                float2 texcoord = i.uv;

                if (_horizontal == 1)   // horizontal blur pass
                {
                    for (int i = 1; i < 3; ++i)
                    {
                        float xOffset = offset[i] * _MainTex_TexelSize.x;   // offset normalized to texel size
                        col += tex2D(_MainTex, texcoord + float2(xOffset, 0)) * weight[i];
                        col += tex2D(_MainTex, texcoord - float2(xOffset, 0)) * weight[i];
                    }
                }
                else    // vertical blur pass
                {
                    for (int i = 1; i < 3; ++i)
                    {
                        float yOffset = offset[i] * _MainTex_TexelSize.y;
                        col += tex2D(_MainTex, texcoord + float2(0, yOffset)) * weight[i];
                        col += tex2D(_MainTex, texcoord - float2(0, yOffset)) * weight[i];
                    }
                }
                return col;
            }
            ENDCG
        }
    }
}
