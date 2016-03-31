Shader "Hidden/ImageEffectShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Buf1("", 2D) = "white"{}
		_Buf2("", 2D) = "white"{}
		_Blend("Blend", Range(0,1)) = 0.5
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			sampler2D _Buf1;
			sampler2D _Buf2;
			float _Blend;

			fixed4 frag (v2f i) : SV_Target
			{
				float4 col0 = tex2D(_Buf1, i.uv);
				float4 col1 = tex2D(_Buf2, i.uv);

				return col0 * _Blend + col1*(1 - _Blend);
			}
			ENDCG
		}
	}
}
