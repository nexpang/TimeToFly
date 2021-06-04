Shader "Custom/BlurShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Blur("Blur",Float) = 10
	}

		SubShader
		{
			Tags{"Queue" = "Transparent"}

			GrabPass
			{
			}

			Pass
			{
				CGPROGRAM

				#paragma vertex vert
				#paragma fragment frag
				#include "UnityCG.cginc"

				struct appdata
				{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
				};

				
			}
		}
}
