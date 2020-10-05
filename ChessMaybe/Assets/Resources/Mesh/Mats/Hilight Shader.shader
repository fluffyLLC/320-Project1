Shader "Unlit/Hilight Shader"
{
    Properties
    {

        
        _Mask ("GlowMask", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        
    }


    SubShader
    {

        Tags {"Queue" = "Transparent" "RenderType"="Transparent" }
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

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


            sampler2D _Mask;
            float4 _Mask_ST;
            float4 _Color;



            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _Mask);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

                fixed4 msk = tex2D(_Mask, i.uv);
                fixed4 col = _Color;
                col.w = msk.r;


                return col;
            }

            ENDCG
        }
    }
}
