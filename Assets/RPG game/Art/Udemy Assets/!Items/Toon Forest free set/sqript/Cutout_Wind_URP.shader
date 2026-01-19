Shader "Custom/Cutout_Wind_URP"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
        _ShakeDisplacement ("Displacement", Range (0, 1.0)) = 1.0
        _ShakeTime ("Shake Time", Range (0, 1.0)) = 1.0
        _ShakeWindspeed ("Shake Windspeed", Range (0, 1.0)) = 1.0
        _ShakeBending ("Shake Bending", Range (0, 1.0)) = 1.0
    }

    SubShader
    {
        Tags { "RenderType" = "TransparentCutout" "Queue" = "AlphaTest" "RenderPipeline" = "UniversalPipeline" }
        LOD 200
        Cull Off 

        // --- FORWARD PASS ---
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            // 1. Core URP Libraries
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            // 2. Material Data
            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _Color;
                float _Cutoff;
                float _ShakeDisplacement;
                float _ShakeTime;
                float _ShakeWindspeed;
                float _ShakeBending;
            CBUFFER_END

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            struct Attributes {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                float3 normalOS : NORMAL;
            };

            struct Varyings {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
            };

            // Wind Functions
            void FastSinCos (float4 val, out float4 s, out float4 c) {
                val = val * 6.408849 - 3.1415927;
                float4 r5 = val * val; float4 r6 = r5 * r5; float4 r7 = r6 * r5; float4 r8 = r6 * r5;
                float4 r1 = r5 * val; float4 r2 = r1 * r5; float4 r3 = r2 * r5;
                float4 sin7 = {1, -0.16161616, 0.0083333, -0.00019841} ;
                float4 cos8  = {-0.5, 0.041666666, -0.0013888889, 0.000024801587} ;
                s =  val + r1 * sin7.y + r2 * sin7.z + r3 * sin7.w;
                c = 1 + r5 * cos8.x + r6 * cos8.y + r7 * cos8.z + r8 * cos8.w;
            }

            float3 ApplyWind(float3 positionOS, float2 uv, float4 color) {
                const float _WindSpeed = (_ShakeWindspeed + color.g);
                const float4 _waveXSize = float4(0.048, 0.06, 0.24, 0.096);
                const float4 _waveZSize = float4(0.024, .08, 0.08, 0.2);
                const float4 waveSpeed = float4(1.2, 2, 1.6, 4.8);
                float4 _waveXmove = float4(0.024, 0.04, -0.12, 0.096);
                float4 _waveZmove = float4(0.006, .02, -0.02, 0.1);

                float4 waves = positionOS.x * _waveXSize;
                waves += positionOS.z * _waveZSize;
                waves += _Time.x * (1 - _ShakeTime * 2 - color.b) * waveSpeed * _WindSpeed;

                float4 s, c;
                waves = frac(waves);
                FastSinCos(waves, s, c);

                float waveAmount = uv.y * (color.a + _ShakeBending);
                s *= waveAmount;
                s *= normalize(waveSpeed);
                s = s * s; s = s * s;

                float3 waveMove = float3(0,0,0);
                waveMove.x = dot(s, _waveXmove);
                waveMove.z = dot(s, _waveZmove);

                positionOS.xz -= waveMove.xz;
                return positionOS;
            }

            Varyings vert (Attributes v) {
                Varyings o;
                float3 pos = ApplyWind(v.positionOS.xyz, v.uv, v.color);
                o.positionCS = TransformObjectToHClip(pos);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normalWS = TransformObjectToWorldNormal(v.normalOS);
                return o;
            }

            half4 frag (Varyings i) : SV_Target {
                half4 c = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv) * _Color;
                clip(c.a - _Cutoff);
                Light mainLight = GetMainLight();
                half3 lighting = mainLight.color * saturate(dot(i.normalWS, mainLight.direction));
                return half4(c.rgb * lighting, 1.0);
            }
            ENDHLSL
        }

        // --- SHADOW CASTER PASS ---
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            Cull Off 

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // 1. CORE INCLUDES (ORDER IS CRITICAL FOR METAL)
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            // Declare global URP variables for shadows
            float3 _LightDirection;

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _Color;
                float _Cutoff;
                float _ShakeDisplacement;
                float _ShakeTime;
                float _ShakeWindspeed;
                float _ShakeBending;
            CBUFFER_END

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            struct Attributes {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                float3 normalOS : NORMAL;
            };

            struct Varyings {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            // Duplicating wind logic in this pass for strict compiler isolation
            void FastSinCos (float4 val, out float4 s, out float4 c) {
                val = val * 6.408849 - 3.1415927;
                float4 r5 = val * val; float4 r6 = r5 * r5;
                float4 r1 = r5 * val; float4 r2 = r1 * r5; float4 r3 = r2 * r5;
                float4 sin7 = {1, -0.16161616, 0.0083333, -0.00019841} ;
                float4 cos8  = {-0.5, 0.041666666, -0.0013888889, 0.000024801587} ;
                s =  val + r1 * sin7.y + r2 * sin7.z + r3 * sin7.w;
                c = 1 + r5 * cos8.x + r6 * cos8.y + 1; // Simplified for shadow
            }

            float3 ApplyWind(float3 positionOS, float2 uv, float4 color) {
                const float _WindSpeed = (_ShakeWindspeed + color.g);
                const float4 _waveXSize = float4(0.048, 0.06, 0.24, 0.096);
                const float4 waveSpeed = float4(1.2, 2, 1.6, 4.8);
                float4 _waveXmove = float4(0.024, 0.04, -0.12, 0.096);
                
                float4 waves = positionOS.x * _waveXSize;
                waves += _Time.x * (1 - _ShakeTime * 2 - color.b) * waveSpeed * _WindSpeed;
                float4 s, c;
                waves = frac(waves);
                FastSinCos(waves, s, c);
                s *= uv.y * (color.a + _ShakeBending);
                positionOS.x -= dot(s, _waveXmove);
                return positionOS;
            }

            Varyings vert (Attributes v) {
                Varyings o;
                float3 posOS = ApplyWind(v.positionOS.xyz, v.uv, v.color);
                float3 posWS = TransformObjectToWorld(posOS);
                float3 normalWS = TransformObjectToWorldNormal(v.normalOS);
                
                float4 posCS = TransformWorldToHClip(ApplyShadowBias(posWS, normalWS, _LightDirection));
                #if UNITY_REVERSED_Z
                    posCS.z = min(posCS.z, posCS.w * UNITY_NEAR_CLIP_VALUE);
                #else
                    posCS.z = max(posCS.z, posCS.w * UNITY_NEAR_CLIP_VALUE);
                #endif
                o.positionCS = posCS;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 frag (Varyings i) : SV_Target {
                half4 c = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                clip(c.a - _Cutoff);
                return 0;
            }
            ENDHLSL
        }
    }
}