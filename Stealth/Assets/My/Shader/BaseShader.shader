// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custem/Simple"
{
	Properties
	{
		_Color("Base Color", Color) = (1, 1, 1, 1)
		_MainTex("Base(RGB)", 2D) = "white" {}
		
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
			
			float4 _MainTex_ST;
			
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
				
		 		half4 c = tex2D(_MainTex, i.uv) * _Color;
				return c;
			}
			
			ENDCG
		}
	}
}