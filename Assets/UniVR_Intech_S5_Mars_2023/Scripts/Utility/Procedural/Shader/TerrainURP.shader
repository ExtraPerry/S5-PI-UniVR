Shader "Custom/TerrainURP"
{
    // In Editor Properties for unity to display to the user as input values.
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BaseColour ("Base Colour", Color) = (1, 1, 1, 1)
    }

    // SubShader block is the container for all of the shader code.
    SubShader
    {
        // Tags define under which conditions the SubShader block or Pass is executed.
        Tags { "RenderType" = "Opaque" }
        // LOD defines the performance level of the shader. System handles that automatically.
        LOD 100

        // The Pass contains all the calculations that the shader will perform.
        Pass
        {
            HLSLPROGRAM
            // Name of the vertex shader.
            #pragma vertex vert
            // Name of the fragment shader.
            #pragma fragment frag
            // Core.hlsl library.
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // Declare the attribute variables.
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            struct VertexInput
            {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct VertexOutput
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            // Vertex shader code.
            VertexOutput vert(VertexInput input)
            {
                VertexOutput output;
                output.position = TransformObjectToHClip(input.position.xyz);
                output.uv = input.uv;
                return output;
            }

            // Fragment shader code.
            float4 frag(VertexOutput input) : SV_Target
            {
                float4 baseTex;
                baseTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                return baseTex;
            }

            ENDHLSL
        }
    }
}
