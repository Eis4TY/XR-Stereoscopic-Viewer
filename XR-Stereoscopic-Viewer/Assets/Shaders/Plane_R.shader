Shader "Custom/Plane_R"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ID ("Mask ID", int) = 1
        [Enum(UnityEngine.Rendering.CompareFunction)] _SComp("StencilComp",float) =3
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry+2" }
        Stencil{
            Ref [_ID]
            Comp equal//当给定的索引值和当前模板缓冲区的值相等时，才会渲染这个片元。
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_stereo

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

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                // If we're rendering the left eye, we move the vertex out of the clip space,
                // effectively culling it and preventing the fragment shader from running on this vertex.
                if (unity_StereoEyeIndex == 0) // Left eye
                {
                    o.vertex = float4(0, 0, 0, 0); // Vertex positioned at the origin, but w-component is 0, so it's outside of the clip space.
                }

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
