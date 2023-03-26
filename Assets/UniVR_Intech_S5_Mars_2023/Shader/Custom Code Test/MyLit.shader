shader "ExtraPerry/MyLit" {

	// Subshader allows for different behaviours based on options such as : Pipelines & Platforms.
	SubShader{
		// Tags server as the parameter identifier and is shared by all passes in this sub shader.
		Tags{"RenderPipeline" = "UniversalPipeline"}

		Pass {
			Name "ForwardLit" //For debugging.
			Tags{"LightMode" = "UniversalForward"} // Pass specific tags.
			// "UniversalForward" tells Unity this is the main lighting pass of this shader.


			HLSLPROGRAM	//Start of HLSL code.

			// Register our programmable stage functions
			#pragma vertex Vertex
			#pragma fragment Fragment

			// Include our code file
			#include "MyLitForwardLitPass.hlsl"

			ENDHSLS	// End of HLSL code.
		}
	}
}