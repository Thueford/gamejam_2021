Shader "Hidden/CRT"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _vignetteAmount("Amount", float) = 1.0
        _vignetteWidth("Width", float) = 0.1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            Name "CRTEffect"

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
            float _vignetteAmount;
            float _vignetteWidth;

            fixed4 getCRTCol(float2 uv)
            {
                // store original color
                fixed4 col = tex2D(_MainTex, uv);

                // uv from [0, 1] to [-1, 1]
                float2 texcoord = uv * 2 - 1;

                // Scanlines are more prominent at the corners
                float scanLineIntensity = smoothstep(.8, 1.41422, length(texcoord));

                // Apply Scanlines (unsing the sin of the y position of the pixel)
                if (scanLineIntensity > 0)
                    col.rgb *= (1 - (sin(texcoord.y * 6.28 * 100) / 2 + .5) * scanLineIntensity) * 0.8 + 0.2;

                // Vignette needs absolute texcoords
                texcoord = abs(texcoord);

                float vignetteStrength = smoothstep(1 - _vignetteWidth, 1, max(texcoord.x, texcoord.y));
                float vignette = 1 - (pow(vignetteStrength, 4) * _vignetteAmount);
                col.rgb *= vignette;

                return col;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // uv from [0, 1] to [-1, 1]
                float2 uv = i.uv * 2 - 1;

                // Warp Coordinates at the corners
                uv.x *= 1 + pow(abs(uv.y) / 8, 2);
                uv.y *= 1 + pow(abs(uv.x) / 8, 2);

                // uv from [-1, 1] to [0, 1]
                uv = uv / 2 + .5;

                fixed4 col = fixed4(0, 0, 0, 1);

                // only pixels inside the main texture get color (with vignette and scanlines applied)
                if (uv.x <= 1 && uv.x >= 0 && uv.y <= 1 && uv.y >= 0)
                    col = getCRTCol(uv);

                return col;
            }
            ENDCG
        }

        
    }
}
