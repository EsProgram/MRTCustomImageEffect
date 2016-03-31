Shader "Unlit/MRTOutout"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		CGINCLUDE
		#include "UnityCG.cginc"

		struct appdata
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
			float3 normal : NORMAL;
		};

		struct v2f
		{
			float2 uv : TEXCOORD0;
			float4 vertex : SV_POSITION;
			float4 diffuse : COLOR0;
		};

		struct buffer {
			half4 buf1 : COLOR0;
			half4 buf2 : COLOR1;
		};

		sampler2D _MainTex;
		float4 _MainTex_ST;
		float4 _LightColor0;

		v2f vert (appdata v)
		{
			v2f o;
			o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			float3 normal = UnityObjectToWorldNormal(v.normal);
			//Diffuseカラー計算
			o.diffuse = max(0, dot(normal, _WorldSpaceLightPos0.xyz)) * _LightColor0;
			//球面調和
			o.diffuse.rgb += ShadeSH9(half4(normal,1));
			return o;
		}

		ENDCG

		Pass
		{
			Tags{"LightMode"="ForwardBase"}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			buffer frag (v2f i)
			{
				buffer o;

				float4 col = tex2D(_MainTex, i.uv) * i.diffuse;

				o.buf1 = col;
				o.buf2 = float4(0, 0, 1, 1);

				return o;
			}
			ENDCG
		}
	}
}
