Shader "Cloister/Enemy/Glitch"
{
	Properties
	{
		_MainTex("Texture Main", 2D) = "white" {}
		_SecondTex("Texture Blend", 2D) = "white" {}
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
				float2 uv = i.uv;
				float p = _Time.y * 0.2;
				float q1 = (uv.y + p * 2) * 200;
				float q2 = (uv.y + p) * 200;
				float r = sin(q1) * sin(2 * q2) + sin(4 * q1) - sin(4 * q1 * q2);
				//float r = sin(q);
				float x = (sin(_Time.y - 3.14) + 1) * 0.50;
				x = pow(x, 4);

				float dx = lerp(0.0, 0.04 * r, x);
				//float dy = lerp(0.0, 0.01 * r, x);

				uv.x += dx;
				//uv.y += dy;

				fixed4 col1 = tex2D(_MainTex, uv);
				fixed4 col2 = tex2D(_SecondTex, uv);


				float v = fmod((i.uv.y + p) * 100, 2);
				if (v > 1.0) v = 1.0;

				return lerp(col1, col2, x * x * 0.6) * lerp(1.0, v, x * 0.8);
			}
			ENDCG
		}
	}
}
