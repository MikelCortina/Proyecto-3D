Shader "Hidden/ColorBlindFilter"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;

            float4 frag(v2f_img i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);

                // Deuteranopia correction matrix (simple example)
                float3x3 correctionMatrix = float3x3(
                    0.8, 0.2, 0.0,
                    0.258, 0.742, 0.0,
                    0.0, 0.142, 0.858
                );

                col.rgb = mul(correctionMatrix, col.rgb);
                return col;
            }
            ENDCG
        }
    }
    FallBack Off
}
