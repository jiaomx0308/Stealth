// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custem/Simple"
{
	Properties
	{
		_Color("Base Color", Color) = (1, 1, 1, 1)
		_MainTex("Base(RGB)", 2D) = "white" {}
		_RSpeed("RotateSpeed", Range(1, 100)) = 30
		
	}
	SubShader
	{
		Tags  {"Queue"="Transparent"
				"RenderType"="Transparent"
				"IgnoreProjector"="true"}
		
		Blend SrcAlpha OneMinusSrcAlpha
		
		Pass
		{
			Name "Simple"
			Cull off
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			half4 _Color;
			float _RSpeed;
			
			struct v2f4
			{
				float4 pos:POSITION;
				float4 uv:TEXCOORD0;
				float4 col:COLOR;
			};
			
			v2f4 vert (appdata_full v)
			{
				v2f4 o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;

				return o;
			}
			
			half4 frag (v2f4 i) : COLOR
			{
				float2 uv = i.uv.xy - float2(0.5, 0.5);
				
				//float2 rotate = float2(cos(_RSpeed * _Time.x), sin(_RSpeed * _Time.x);
				//uv = float2(uv.x * rotate.x - uv.y * rotate.y, uv.x * rotate.y + uv.y * rotate.x);
				
				uv = float2(uv.x * cos(_RSpeed * _Time.x) - uv.y * sin(_RSpeed * _Time.x),
							uv.x * sin(_RSpeed * _Time.x) + uv.y * cos(_RSpeed * _Time.x));
				
				uv += float2(0.5, 0.5);
				
		 		half4 c = tex2D(_MainTex, uv) * _Color;
				return c;
			}
			
			ENDCG
		}
	}
}