Shader "Shield/Hologram"
{
    Properties
    {
        _RimColor ("Rim Color", Color) = (0.0, 0.5, 0.5, 0.0)
        _RimPower ("Rim Power", Range(0.5, 8.0)) = 3.0
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        Pass
        {
            Tags { "LightMode" = "UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
                float3 viewDirWS : TEXCOORD1;
                float2 uv : TEXCOORD2;
            };

            float4 _RimColor;
            float _RimPower;

            Varyings vert(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                float3 worldPos = TransformObjectToWorld(input.positionOS);
                output.positionHCS = TransformWorldToHClip(worldPos);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.viewDirWS = normalize(GetCameraPositionWS() - worldPos);
                output.uv = input.uv;

                return output;
            }

            half4 frag(Varyings input) : SV_TARGET
            {
                // Calculate the rim effect
                half rim = 1.0 - saturate(dot(normalize(input.viewDirWS), normalize(input.normalWS)));
                half4 rimColor = _RimColor * pow(rim, _RimPower);

                // Output color with emission and alpha
                half4 outputColor = rimColor;
                outputColor.a = pow(rim, _RimPower);

                return outputColor;
            }
            ENDHLSL
        }
    }
    Fallback "Diffuse"
}
