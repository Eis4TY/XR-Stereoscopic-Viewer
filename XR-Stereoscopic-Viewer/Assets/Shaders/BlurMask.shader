// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/BlurMask" {
    Properties {
        _blurSizeXY("BlurSizeXY", Range(0,10)) = 0.0
        _subdivisions("Subdivisions", Range(1,4)) = 5
        _gradientTex("GradientTexture", 2D) = "white" {}
        _ID ("Mask ID", int) = 1
        [Enum(UnityEngine.Rendering.CompareFunction)] _SComp("StencilComp",float) =3
    }
    SubShader {
        // Draw ourselves after all opaque geometry
        Tags { "Queue" = "Transparent" }
        // Grab the screen behind the object into _GrabTexture
        GrabPass { }
        Stencil {
            Ref [_ID]
            Comp equal // 当给定的索引值和当前模板缓冲区的值相等时，才会渲染这个片元。
        }
        // Render the object with the texture generated above
        Pass {
            CGPROGRAM
            #pragma target 5.0

            #pragma debug
            #pragma vertex vert
            #pragma fragment frag 
            
            sampler2D _GrabTexture : register(s0);
            sampler2D _gradientTex;
            float _blurSizeXY;
            int _subdivisions;
            struct data {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };
            struct v2f {
                float4 position : POSITION;
                float4 screenPos : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };
            v2f vert(data i) {
                v2f o;
                o.position = UnityObjectToClipPos(i.vertex);
                o.uv = i.uv;
                o.screenPos = o.position;
                return o;
            }
            half4 frag( v2f i ) : COLOR {
                float2 screenPos = i.screenPos.xy / i.screenPos.w;
                float depth= _blurSizeXY*0.0005;
                screenPos.x = (screenPos.x + 1) * 0.5;
                screenPos.y = 1-(screenPos.y + 1) * 0.5;
                half4 sum = half4(0.0h,0.0h,0.0h,0.0h);   
                half4 color = tex2D(_GrabTexture, screenPos);
                float2 texOffset = 1.0 / _ScreenParams.xy;
                float weights[5] = {0.05, 0.09, 0.12, 0.15, 0.16};
                half4 gradientColor = tex2D(_gradientTex, i.uv);
                for(int j = -_subdivisions; j <= _subdivisions; j++) {
                    for(int i = -_subdivisions; i <= _subdivisions; i++) {
                        float2 offset = float2(i,j) * texOffset * _blurSizeXY * gradientColor.r;
                        sum += tex2D(_GrabTexture, screenPos + offset) * weights[abs(i)];
                    }
                }
                return sum/(_subdivisions*2);
            }
            ENDCG
        }
    }
    Fallback Off
}
