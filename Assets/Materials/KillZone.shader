Shader "Custom/KillZone"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0

			_XOffset("X Offset", Range(0, 1)) = 0
			_YOffset("Y Offset", Range(0, 1)) = 0
			_XOffsetStart("X Offset Start", Range(0, 1)) = 0
			_YOffsetStart("Y Offset Start", Range(0, 1)) = 0
			_XOffsetRange("X Offset Range", Range(0, 1)) = 0
			_YOffsetRange("Y Offset Range", Range(0, 1)) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _Color;
			float _XOffset;
			float _YOffset;
			float _XOffsetStart;
			float _YOffsetStart;
			float _XOffsetRange;
			float _YOffsetRange;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			float _AlphaSplitEnabled;

			fixed4 SampleSpriteTexture (float2 uv, bool invert)
			{
				if (invert) {
					uv += float2((1 - _XOffset) * _XOffsetRange, (1 - _YOffset) * _YOffsetRange);
				} else {
					uv += float2(_XOffset * _XOffsetRange, _YOffset * _YOffsetRange);
				}
				if (uv.x > _XOffsetStart + _XOffsetRange) {
					uv.x -= _XOffsetRange;
				}
				if (uv.y > _YOffsetStart + _YOffsetRange) {
					uv.y -= _YOffsetRange;
				}

				fixed4 color = tex2D (_MainTex, uv);

#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D (_AlphaTex, uv).r;
#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

				return color;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord, (IN.vertex.y % 42) < 21) * IN.color;
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
}