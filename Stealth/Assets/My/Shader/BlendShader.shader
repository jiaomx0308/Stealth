Shader "Unlit/BlendShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_OverlayTex("OverlayTex", 2D) = "white" {}
		_MaskTex("MaskTex", 2D) = "white" {}  //使用该贴图的r通道的白色部分显示MainTex,黑色部分显示OverlayTex;
	}
	SubShader
	{
		Tags {"RenderType" = "Opaque"}
		LOD 100
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_comile_fog
			#include "UnityCG.cginc"
			
			struct v2f
			{
				float4 pos:SV_POSITION;
				float2 uv:TEXCOORD0;
				UNITY_FOG_COORDS(1)
			};
			
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _OverlayTex;
			sampler2D _MaskTex;
			
			v2f vert(appdata_full v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				UNITY_TRANSFER_FOG(o, o.pos);
				return o;
			}
			
			fixed4 frag(v2f i):SV_Target
			{
				fixed4 main_col = tex2D(_MainTex, i.uv);
				fixed4 overlay_col = tex2D(_OverlayTex, i.uv);
				fixed4 mask_col = tex2D(_MaskTex, i.uv);
				
				fixed4 final_color = 1;
				final_color.rgb = main_col.rgb * mask_col.r + overlay_col*(1 - mask_col.r);
				
				UNITY_APPLY_FOG(i.fogCoord, final_color);
				return final_color;
			}
			
			ENDCG
		}
	}
}