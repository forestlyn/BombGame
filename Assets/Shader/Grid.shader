Shader "Unlit/Grid"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MyOffset ("MyOffset", Range(0,0.5)) = 0.0
        [Toggle(_True)]_IsShow("IsShow", float) = 1

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "QUEUE"="Transparent" }
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
            float _MyOffset;
            float _IsShow;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                float offset = _MyOffset;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                if(_IsShow==1)
                {
                    if(i.uv[0]<offset || i.uv[1]<offset || (1 - i.uv[1])<offset || (1-i.uv[0])<offset)
                    {
                        return fixed4(0,0,0,255);
                    }
                }
                return col;
            }
            ENDCG
        }
    }
}
