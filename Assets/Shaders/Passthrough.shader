// This shader fills the mesh shape with a color predefined in the code.
Shader "Passthrough"
{
    // The properties block of the Unity shader. In this example this block is empty
    // because the output color is predefined in the fragment shader code.
    Properties
    {
        
    }

    // The SubShader block containing the Shader code. 
    SubShader
    {
        // SubShader Tags define when and under which conditions a SubShader block or
        // a pass is executed.
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" }

        Pass
        {
            
            BlendOp RevSub
            Blend One Zero, Zero Zero
            
            // The HLSL code block. Unity SRP uses the HLSL language.
            HLSLPROGRAM
            // This line defines the name of the vertex shader. 
            #pragma vertex vert
            // This line defines the name of the fragment shader. 
            #pragma fragment frag

            // The Core.hlsl file contains definitions of frequently used HLSL
            // macros and functions, and also contains #include references to other
            // HLSL files (for example, Common.hlsl, SpaceTransforms.hlsl, etc.).
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"            

            // The structure definition defines which variables it contains.
            // This example uses the Attributes structure as an input structure in
            // the vertex shader.
            struct Attributes
            {
                // The positionOS variable contains the vertex positions in object
                // space.
                float4 positionOS   : POSITION;

                UNITY_VERTEX_INPUT_INSTANCE_ID // Use for stereo single pass rendering
            };

            struct Varyings
            {
                // The positions in this struct must have the SV_POSITION semantic.
                float4 positionHCS  : SV_POSITION;
                UNITY_VERTEX_OUTPUT_STEREO // Use for stereo single pass rendering
            };            

            // The vertex shader definition with properties defined in the Varyings 
            // structure. The type of the vert function must match the type (struct)
            // that it returns.
            Varyings vert(Attributes IN)
            {
                // Declaring the output object (OUT) with the Varyings struct.
                Varyings OUT;

                UNITY_SETUP_INSTANCE_ID(IN); // Use for stereo single pass rendering
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT); // Use for stereo single pass rendering
                // The TransformObjectToHClip function transforms vertex positions
                // from object space to homogenous space
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                // Returning the output.
                return OUT;
            }
            
            // The fragment shader definition.            
            half4 frag(Varyings v) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(v); // Use for stereo single pass rendering
                // Defining the color variable and returning it.
                half4 customColor;
                customColor = half4(0,0,0,0);
                return customColor;
            }
            ENDHLSL
        }
    }
}