Shader "Custom/InnerWall" {

Properties

{

_MainTex ("Main Texture", 2D) = "white" {}
_MaskTex ("Mask Texture", 2D) = "white" {}

_BlurPixel("Blur", Range(0,100))=0.1

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
sampler2D _MaskTex; 

float4 _MainTex_ST;

float4 _MainTex_TexelSize;

float _BlurPixel;

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

fixed4 col = tex2D(_MainTex, i.uv)*0.2;
fixed4 mask = tex2D(_MaskTex, i.uv);
  
  _BlurPixel *= mask.r;

col += tex2D(_MainTex, i.uv+_MainTex_TexelSize*float2(0,1)*_BlurPixel)*0.03;

col += tex2D(_MainTex, i.uv+_MainTex_TexelSize*float2(0,-1)*_BlurPixel)*0.03;

col += tex2D(_MainTex, i.uv+_MainTex_TexelSize*float2(-1,0)*_BlurPixel)*0.03;

col += tex2D(_MainTex, i.uv+_MainTex_TexelSize*float2(0,1)*_BlurPixel)*0.03;

col += tex2D(_MainTex, i.uv+_MainTex_TexelSize*float2(-1,1)*_BlurPixel)*0.03;

col += tex2D(_MainTex, i.uv+_MainTex_TexelSize*float2(1,1)*_BlurPixel)*0.03;

col += tex2D(_MainTex, i.uv+_MainTex_TexelSize*float2(-1,-1)*_BlurPixel)*0.03;

col += tex2D(_MainTex, i.uv+_MainTex_TexelSize*float2(1,-1)*_BlurPixel)*0.03;

col += tex2D(_MainTex, i.uv+_MainTex_TexelSize*float2(0,1)*_BlurPixel*0.5)*0.07;

col += tex2D(_MainTex, i.uv+_MainTex_TexelSize*float2(0,-1)*_BlurPixel*0.5)*0.07;

col += tex2D(_MainTex, i.uv+_MainTex_TexelSize*float2(-1,0)*_BlurPixel*0.5)*0.07;

col += tex2D(_MainTex, i.uv+_MainTex_TexelSize*float2(0,1)*_BlurPixel*0.5)*0.07;

col += tex2D(_MainTex, i.uv+_MainTex_TexelSize*float2(-1,1)*_BlurPixel*0.5)*0.07;

col += tex2D(_MainTex, i.uv+_MainTex_TexelSize*float2(1,1)*_BlurPixel*0.5)*0.07;

col += tex2D(_MainTex, i.uv+_MainTex_TexelSize*float2(-1,-1)*_BlurPixel*0.5)*0.07;

col += tex2D(_MainTex, i.uv+_MainTex_TexelSize*float2(1,-1)*_BlurPixel*0.5)*0.07;

// apply fog

UNITY_APPLY_FOG(i.fogCoord, col);

return col;

}

ENDCG

}

}

}