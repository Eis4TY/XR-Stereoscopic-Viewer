Shader "Custom/DepthMapShader"
{
    Properties
    {
        _MainTex("MainTexture", 2D) = "white" {}
        _depth("depth", 2D) = "white" {}
        _scale("scale", Float) = 1 

        _ID ("Mask ID", int) = 1
        [Enum(UnityEngine.Rendering.CompareFunction)] _SComp("StencilComp",float) =3
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry+2" }
        Stencil{
            Ref [_ID]
            Comp equal
        }

        Pass
        {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _depth;
            float _scale;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                float depthValue = tex2Dlod(_depth, float4(v.uv,0,0)).r;
                v.vertex.y += depthValue * _scale;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.uv1;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.texcoord);
                return col;
            }

            ENDCG
        }
    }
}