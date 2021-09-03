Shader "Hidden/LensFlare"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LensTex("Texture", 2D) = "white" {}
        _ResultTex("Texture", 2D) = "white" {}
        _StarburstTex("Texture", 2D) = "white" {}
        _GhostCount("GhostCount", int) = 3
        _Threshold("Threshold", float) = 0.75
        _GhostSpacing("GhostSpacing", float) = 0.75
        _CaStrength("CaStrength", float) = 0.15
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
            float _CaStrength;

            fixed3 getTexColor(float2 texcoords)
            {
                float amount = length(.5 - texcoords) * _CaStrength;
                // works like ca, offset is in Direction of Image Center
                float2 offset = normalize(.5 - texcoords) * amount;
                return fixed3(
                    tex2D(_MainTex, texcoords + offset).r,
                    tex2D(_MainTex, texcoords).g,
                    tex2D(_MainTex, texcoords - offset).b
                    );
            }

            float _Threshold;

            fixed3 generateHalo(float2 texcoord, float2 ghostVec)
            {
                fixed3 col = 0;
                // Works like ghostVec but has a constant length (radius of the Halo)
                float2 haloVec = normalize(ghostVec) * 0.35;
                fixed3 texColor = getTexColor(texcoord + haloVec);
                texColor = max(texColor - _Threshold, 0);

                if (any(texColor))
                {
                    // Halo gets dimmer closer to the edges
                    float weight = length(.5 - texcoord);
                    col += texColor * weight;
                }
                return col;
            }

            int _GhostCount;

            fixed3 generateGhosts(float2 texcoord, float2 ghostVec)
            {
                fixed3 col = 0;
                float2 offset = texcoord;

                for (int i = 0; i < _GhostCount; ++i)
                {
                    //Coordinates to look at for a bright spot 
                    offset += ghostVec;
                    //create ghosts
                    fixed3 texColor = getTexColor(offset);
                    texColor = max(texColor - _Threshold, 0);
                    if (any(texColor))
                    {
                        //Bright spots at the edge of the screen matter less;
                        float weight = 1 - length(.5 - offset);
                        weight = pow(abs(weight), 10);
                        col += texColor * weight;
                    }
                }
                return col;
            }

            float _GhostSpacing;

            fixed4 frag (v2f i) : SV_Target
            {

                //uint width, height;
                //Source.GetDimensions(width, height);
                //float2 iResolution = float2(width, height); // Size of Source

                //coordinates transformed to [0, 1]
                //float2 texcoord = (id.xy / iResolution);
                //vector from texcoord to the image center;
                float2 ghostVec = (-i.uv + .5) * _GhostSpacing;

                fixed3 col = 0;
                col += generateGhosts(i.uv, ghostVec);
                col += generateHalo(i.uv, ghostVec);

                return fixed4(col, 1);

                //fixed4 col = tex2D(_MainTex, i.uv);
                //return col;
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
            sampler2D _ResultTex;
            sampler2D _LensTex;

            fixed4 frag(v2f i) : SV_Target
            {
                // MainTex is the LensDirt texture, StarburstTex represents a 1D barcode texture
                
                fixed3 col = tex2D(_ResultTex, i.uv).rgb;
                if (any(col))
                {
                    col *= tex2D(_LensTex, i.uv).rgb + .1;
                }
                
                return tex2D(_MainTex, i.uv) + fixed4(col, 0);
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
                float angle = acos(normalize(i.uv - .5).x) / 3.1416 / 2 + 0.5;
                // Starburst is additively applied to Lens Dirt
                col += max(0, tex2D(_StarburstTex, float2(angle, .5))) * .3;

                return col;
            }
            ENDCG
        }
    }
}
