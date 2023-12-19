Shader "Custom/GlowShader3D" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _GlowColor ("Glow Color", Color) = (1,1,0,1)
        _GlowPower ("Glow Power", Range (0.5, 5.0)) = 1.0
        _MainTex ("Base (RGB)", 2D) = "white" { }
    }
    SubShader {
        Tags {"Queue" = "Overlay" }
        LOD 100

        Pass {
            ZWrite On
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask RGB
            Cull Front
            Lighting Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f {
                float4 pos : POSITION;
            };

            uniform float _GlowPower;

            v2f vert(appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : COLOR {
                fixed4 col = tex2D(_MainTex, i.pos.xy / i.pos.w);
                fixed4 glow = _GlowColor * _GlowPower;
                return col + glow;
            }
            ENDCG
        }
    }
}