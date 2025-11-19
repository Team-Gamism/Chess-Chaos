Shader "UI/GrayscaleStencilMask"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        [IntRange] _StencilID("_Stencil ID", Range(0,255)) = 0
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        
        Stencil
        {
            Ref [_StencilID]
            Comp Equal
            Pass Keep
            ReadMask 255
            WriteMask 0
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata 
            { 
                float4 vertex : POSITION; 
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };
            
            struct v2f 
            { 
                float4 pos : SV_POSITION; 
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            sampler2D _MainTex;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 c = tex2D(_MainTex, i.uv) * i.color;
                float gray = dot(c.rgb, float3(0.299, 0.587, 0.114));
                return fixed4(gray, gray, gray, c.a);
            }
            ENDCG
        }
    }
}