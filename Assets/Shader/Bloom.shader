Shader "Hidden/Bloom"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BrightTex ("Texture", 2D) = "black" {}
        _Threshold ("Threshold", float) = 0.75
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            Name "Bloom"

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
            float _Threshold;

            fixed4 frag (v2f i) : SV_Target
            {

                if (dot(float3(0.2126, 0.7152, 0.0722), tex2D(_MainTex, i.uv).rgb) > _Threshold)
                {
                    return tex2D(_MainTex, i.uv);
                }

                return fixed4(0, 0, 0, 1);

            }
            ENDCG
        }

        Pass
        {
            Name "WriteBack"

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
            sampler2D _BrightTex;

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb = min(col.rgb + tex2D(_BrightTex, i.uv).rgb, 1);

                return col;
            }
            ENDCG
        }
    }
}
