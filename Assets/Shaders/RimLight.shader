Shader "Custom/Hologram"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0.0, 0.7, 1.0, 1.0)
        _FresnelColor ("Fresnel Color", Color) = (0.0, 1.0, 1.0, 1.0)
        _ScrollSpeed ("Scroll Speed", Float) = 1.0
        _DistortionStrength ("Distortion Strength", Float) = 0.1
        _MainTex ("Main Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3 viewDirWS : TEXCOORD2;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _FresnelColor;
                float _ScrollSpeed;
                float _DistortionStrength;
                float4 _MainTex_ST;
            CBUFFER_END

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D(_NoiseTex);
            SAMPLER(sampler_NoiseTex);

            Varyings vert(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                float3 worldPos = TransformObjectToWorld(input.positionOS);
                output.positionHCS = TransformWorldToHClip(worldPos);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                output.viewDirWS = normalize(GetCameraPositionWS() - worldPos);

                return output;
            }

            half4 frag(Varyings input) : SV_TARGET
            {
                // Scroll the main texture
                float2 scrollUV = input.uv + float2(0, _Time.y * _ScrollSpeed);
                half4 mainTexColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, scrollUV);

                // Sample the noise texture for distortion
                float2 noiseUV = input.uv * 5.0;
                half4 noiseTexColor = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, noiseUV);
                float3 distortedNormal = input.normalWS + (noiseTexColor.rgb * _DistortionStrength);

                // Calculate the fresnel effect
                half fresnel = pow(1.0 - saturate(dot(normalize(input.viewDirWS), normalize(distortedNormal))), 3.0);
                half4 fresnelColor = _FresnelColor * fresnel;

                // Combine base color, main texture, and fresnel effect
                half4 finalColor = _BaseColor * mainTexColor + fresnelColor;
                finalColor.a = mainTexColor.a; // Use the alpha from the main texture

                return finalColor;
            }
            ENDHLSL
        }
    }
    Fallback "Transparent/Cutout/VertexLit"
}
