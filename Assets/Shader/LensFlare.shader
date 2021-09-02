Shader "Hidden/LensFlare"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _StarburstTex("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
                col.rgb = 1 - col.rgb;
                return col;
            }
            ENDCG
        }

        Pass
        {
            Name "GenLensTex"

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
            sampler2D _StarburstTex;

            fixed4 frag(v2f i) : SV_Target
            {
                // MainTex is the LensDirt texture, StarburstTex represents a 1D barcode texture
                fixed4 col = tex2D(_MainTex, i.uv);

                // StarburstTex is accessed via the angle of the texcoord vector (uv)
                float angle = acos(normalize(i.uv - .5).x) / 3.1416 / 2 + 1;
                // Starburst is additively applied to Lens Dirt
                col += max(0, tex2D(_StarburstTex, float2(angle, .5) - .2)) * .3;

                return col;
            }
            ENDCG
        }
    }
}
