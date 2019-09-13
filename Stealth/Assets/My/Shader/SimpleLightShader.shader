// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custem/SimpleLightShader"
{
	Properties
	{
		_Color("Base Color", Color) = (1, 1, 1, 1)
		_MainTex("Base(RGB)", 2D) = "white" {}
		
	}
	SubShader
	{
		Tags  {
				"RenderType"="Opaque"
				/*"LightMode"="ForwardBase"*/}  //声明光照类型,ForwardBase用于正向渲染
		
		Pass
		{
			Name "Simple"
			Cull off
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc" //引用灯光库
			
			sampler2D _MainTex;
			half4 _Color;
			float _RSpeed;
			
			float4 _MainTex_ST;

			
			struct v2f4
			{
				float4 pos:SV_POSITION;
				float2 uv:TEXCOORD0;
				UNITY_FOG_COORDS(1)
				
				float3 worldNormal:NORMAL; //用于保存世界坐标顶点法线方向信息
				float4 diff:COLOR;     //用于保存于灯光计算后的色彩
			};
			
			v2f4 vert (appdata_full v)
			{
				v2f4 o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				
				UNITY_TRANSFER_FOG(o, o.pos);
				
				o.worldNormal = UnityObjectToWorldNormal(v.normal);//将本地法线方向转化为世界坐标法线方向
				half nl = max(0, dot(o.worldNormal, _WorldSpaceLightPos0.xyz));  //表征Lambert灯光计算, _WorldSpaceLightPos0是默认方向光的方向;
				o.diff = nl * _LightColor0;  //与默认方向光的颜色相乘;
				
				return o;
			}
			
			half4 frag (v2f4 i) : COLOR
			{
				
		 		half4 c = tex2D(_MainTex, i.uv) * _Color;
				c *= i.diff;  //纹理采样结果和关照计算结果相乘
				UNITY_APPLY_FOG(i.fogCoord, c);
				
				return c;
			}
			
			ENDCG
		}
	}
}