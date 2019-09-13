// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custem/Simple"
{
	Properties
	{
		_Color("Base Color", Color) = (1, 1, 1, 1)
		_MainTex("Base(RGB)", 2D) = "white" {}
		_Gray("Gray", Range(0,1)) = 1
		
		_LineColor("Line Color", Color) = (1,1,1,1)
		_LineWidth("Line Width", Range(0, 10)) = 1
		
	}
	SubShader
	{
		Tags  {"Queue"="Transparent"
				"RenderType"="Transparent"
				"IgnoreProjector"="true"}
		
		Blend SrcAlpha OneMinusSrcAlpha
		
		//ZWrite On
		
		Pass
		{
			Name "Simple"
			Cull Off
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			half4 _Color;
			float _RSpeed;
			
			float4 _MainTex_ST;
			float _Gray;

			
			struct v2f4
			{
				float4 pos:SV_POSITION;
				float2 uv:TEXCOORD0;
				float4 col:COLOR;
				UNITY_FOG_COORDS(1)   //这个宏如果场景中定义了fog效果,会被转化为:float1 fogCoord : TEXCOORD##idx;  即float1 fogCoord : TEXCOORD1;
			};
			
			v2f4 vert (appdata_full v)  //v.vertex.w在传入时为1,
			{
				v2f4 o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				
				UNITY_TRANSFER_FOG(o, o.pos);   //UNITY_TRANSFER_FOG对v2f4中的fogCoord进行了赋值,o.fogCoord.x = (outpos).z  //赋予了景深的值(在UnityObjectToClipPos),不同平台不同实现
				
				o.col.w = o.pos.w;
				//o.col.w = v.vertex.w;

				return o;
			}
			
			half4 frag (v2f4 i) : COLOR
			{
				
		 		half4 c = tex2D(_MainTex, i.uv) * _Color;
				c.rgb = lerp(c.rgb, Luminance(c.rgb), _Gray);
				UNITY_APPLY_FOG(i.fogCoord, c);  //col.rgb = lerp((fogCol).rgb, (col).rgb, saturate(fogFac));不同平台实现不同,在fogCol和场景的原始col之间根据i.fogCoord的值做了个Lerp
				return c;
			}
			
			ENDCG
		}
		
	
		Pass
		{
			Name "Simple"
			Cull Front
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			half4 _Color;
			float _RSpeed;
			
			float4 _MainTex_ST;
			float _Gray;
			
			float4 _LineColor;  //线的颜色
			fixed _LineWidth;  //线的宽度

			
			struct v2f4
			{
				float4 pos:SV_POSITION;
				float2 uv:TEXCOORD0;
				float4 col:COLOR;
				UNITY_FOG_COORDS(1)   //这个宏如果场景中定义了fog效果,会被转化为:float1 fogCoord : TEXCOORD##idx;  即float1 fogCoord : TEXCOORD1;
			};
			
			v2f4 vert (appdata_full v)  //v.vertex.w在传入时为1,
			{
				v2f4 o;
				v.vertex.xyz += v.normal * _LineWidth;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				
				UNITY_TRANSFER_FOG(o, o.pos);   //UNITY_TRANSFER_FOG对v2f4中的fogCoord进行了赋值,o.fogCoord.x = (outpos).z  //赋予了景深的值(在UnityObjectToClipPos),不同平台不同实现
				
				o.col.w = o.pos.w;
				//o.col.w = v.vertex.w;

				return o;
			}
			
			half4 frag (v2f4 i) : COLOR
			{
				
		 		half4 c = _LineColor;
				UNITY_APPLY_FOG(i.fogCoord, c);  //col.rgb = lerp((fogCol).rgb, (col).rgb, saturate(fogFac));不同平台实现不同,在fogCol和场景的原始col之间根据i.fogCoord的值做了个Lerp
				return c;
			}
			
			ENDCG
		}
		
	}
}