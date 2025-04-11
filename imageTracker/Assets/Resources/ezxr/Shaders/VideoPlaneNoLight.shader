Shader "VideoPlaneNoLight" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" { }
        _texCoordScaleX ("Texture Coordinate Scale", float) = 1.0
        _texCoordScaleY ("Texture Coordinate Scale", float) = 1.0
		_isPortrait("Texture FlipXY", int) = 0
	}
	SubShader {
		Pass {
		   Name "VideoPlaneNoLight"
		

			Lighting Off
			ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

            uniform float _texCoordScaleX,_texCoordScaleY;
			uniform int _isPortrait;

            struct Vertex
			{
				float4 position : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct TexCoordInOut
			{
				float4 position : SV_POSITION;
				float2 texcoord : TEXCOORD0;
			};

			TexCoordInOut vert (Vertex vertex)
			{
				TexCoordInOut o;
                o.position = UnityObjectToClipPos(vertex.position); 
				
				float2 t = float2((0.5f - vertex.texcoord.x) * _texCoordScaleX + 0.5f, (0.5f - vertex.texcoord.y) * _texCoordScaleY + 0.5f);
				o.texcoord.xy = lerp(t.xy, t.yx, _isPortrait);
	            
				return o;
			}
			 // samplers
            sampler2D _MainTex;

			fixed4 frag (TexCoordInOut i) : SV_Target
			{
				// sample the texture
                float2 texcoord = i.texcoord;
                return tex2D(_MainTex, texcoord).rgba;
			}
			ENDCG
		}
	}
}
