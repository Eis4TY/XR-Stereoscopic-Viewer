Shader "Custom/GaussianBlur_R"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MaskTex ("Mask", 2D) = "white" {}
        _BlurSize ("Blur Size", Range(0, 20)) = 1.0
        _ID ("Mask ID", int) = 1
        [Enum(UnityEngine.Rendering.CompareFunction)] _SComp("StencilComp",float) =3
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry+3" }
        Stencil{
            Ref [_ID]
            Comp equal//当给定的索引值和当前模板缓冲区的值相等时，才会渲染这个片元。
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha // This line is moved inside the Pass block

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
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
            sampler2D _MaskTex;
            float _BlurSize;
            float4 _MainTex_ST; // For tiling and offset

            // Gaussian weights
            static const float weight[7] = { 0.1278, 0.1155, 0.0933, 0.0647, 0.0381, 0.0189, 0.0079 };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                if (unity_StereoEyeIndex == 0) // Left eye
                {
                    o.vertex = float4(0, 0, 0, 0); // Vertex positioned at the origin, but w-component is 0, so it's outside of the clip space.
                }
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float mask = tex2D(_MaskTex, i.uv).r;
                float2 texelSize = _BlurSize / _ScreenParams.xy;
                float4 result = tex2D(_MainTex, i.uv) * weight[0];
                float normalization = weight[0];

                for (int j = 1; j < 7; ++j)
                {
                    result += tex2D(_MainTex, i.uv + float2(j, 0) * texelSize) * weight[j];
                    result += tex2D(_MainTex, i.uv - float2(j, 0) * texelSize) * weight[j];
                    result += tex2D(_MainTex, i.uv + float2(0, j) * texelSize) * weight[j];
                    result += tex2D(_MainTex, i.uv - float2(0, j) * texelSize) * weight[j];
                    normalization += 4 * weight[j];
                }

                result /= normalization; // normalize by the sum of weights

                // Apply the mask to the alpha channel
                result.a = mask;

                return result;
            }
            ENDCG
        }
    }
}
