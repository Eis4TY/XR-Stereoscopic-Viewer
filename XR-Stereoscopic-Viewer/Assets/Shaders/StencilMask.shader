Shader "Custom/StencilMask"
{
    Properties
    {
        _ID ("Mask ID", int) = 1
        [Enum(UnityEngine.Rendering.CompareFunction)] _SComp("StencilComp",float) =8
        [Enum(UnityEngine.Rendering.StencilOp)] _SOp("StencilOp",float) =2
    }
    SubShader
    {
        //队列为Geometry+1（默认的不透明物体后进行蒙版的渲染）
        Tags { "RenderType"="Opaque" "Queue"="Geometry+1" }
        ColorMask 0 //RGBA,R,G,B,A,0 颜色罩照   0为不输出任何颜色
        ZWrite off
        Stencil{
            Ref [_ID]
            Comp always//默认 always
            Pass replace  //默认keep
            //Fail keep
            //ZFail keep
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
            };
            struct v2f
            {
                float4 vertex : SV_POSITION;
            };
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }
            fixed4 frag (v2f i) : SV_Target
            {
                return half4(1,1,1,1);//无所谓
            }
            ENDCG
        }
    }
}