﻿Shader "Cloister/Projectile/Bolt"
{
    Properties
    {
    }

	SubShader
	{
		Blend SrcAlpha OneMinusSrcAlpha

		Tags
		{
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			sampler2D _SecondTex;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
                float2 rootXY = sqrt(i.uv);
                float sqrX = i.uv.x * i.uv.x;
				return fixed4(0.9 + 0.1 * rootXY.x, sqrX * 0.2 + (0.3 - 0.3 * rootXY.y), sqrX * 0.05, rootXY.y);
			}
			ENDCG
		}
	}
}
