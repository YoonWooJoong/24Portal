Shader "Custom/PortalShader"
{
    Properties
    {
        _MainTex ("Render Texture", 2D) = "black" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float3 _PortalCenter;
            float3 _PortalNormal;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                
                float3 viewDir = o.worldPos - _PortalCenter;
                float3 right = normalize(cross(float3(0,1,0), _PortalNormal));
                float3 up = normalize(cross(_PortalNormal, right));

                o.uv.x = dot(viewDir, right) * 0.5 + 0.5;
                o.uv.y = dot(viewDir, up) * 0.5 + 0.5;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                i.uv.x = 1.0 - i.uv.x;
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
