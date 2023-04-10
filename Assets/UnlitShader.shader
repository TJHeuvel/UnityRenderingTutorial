Shader "Unlit/UnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma enable_d3d11_debug_symbols
            
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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            float4x4 custom_ObjectToWorld,
                    custom_ViewMatrix,
                    custom_ProjectionMatrix;

            v2f vert (appdata v)
            {
                v2f o;
                o.uv = (v.uv * _MainTex_ST.xy) + _MainTex_ST.zw;
                
                float4 worldPos = mul(custom_ObjectToWorld, v.vertex);
                float4 viewPos = mul(custom_ViewMatrix, worldPos);
                float4 clipPos = mul(custom_ProjectionMatrix, viewPos);

                o.vertex = clipPos;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}
