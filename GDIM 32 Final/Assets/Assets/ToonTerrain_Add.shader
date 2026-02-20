Shader "Hidden/ToonTerrainAdd" {
    Properties {
        [HideInInspector] _Control ("Control (RGBA)", 2D) = "black" {}
        [HideInInspector] _Splat3 ("Layer 3 (A)", 2D) = "white" {}
        [HideInInspector] _Splat2 ("Layer 2 (B)", 2D) = "white" {}
        [HideInInspector] _Splat1 ("Layer 1 (G)", 2D) = "white" {}
        [HideInInspector] _Splat0 ("Layer 0 (R)", 2D) = "white" {}
        [HideInInspector] _Normal3 ("Normal 3 (A)", 2D) = "bump" {}
        [HideInInspector] _Normal2 ("Normal 2 (B)", 2D) = "bump" {}
        [HideInInspector] _Normal1 ("Normal 1 (G)", 2D) = "bump" {}
        [HideInInspector] _Normal0 ("Normal 0 (R)", 2D) = "bump" {}
        
        // Toon Ramp Lighting Properties
        _VerticalTex ("VerticalTex (RGB)", 2D) = "white" {}
        _ToonRamp ("Toon Ramp", 2D) = "white" {}
        _RampThreshold ("Ramp Threshold", Range(0, 1)) = 0.5
        _RampSmooth ("Ramp Smoothness", Range(0, 1)) = 0.1
        _SpecColor ("Specular Color", Color) = (1,1,1,1)
        _Shininess ("Shininess", Range(0.01, 1)) = 0.7
        _ShadowIntensity ("Shadow Intensity", Range(0, 1)) = 0.3
    }
    CGINCLUDE
        #pragma surface surf ToonTerrain decal:add vertex:SplatmapVert finalcolor:SplatmapFinalColor finalprepass:SplatmapFinalPrepass finalgbuffer:SplatmapFinalGBuffer noinstancing decal:blend
        #pragma multi_compile_fog
        #define TERRAIN_SPLAT_ADDPASS
        #include "ToonTerrain.cginc"
        sampler2D _ToonRamp;
        half _RampThreshold;
        half _RampSmooth;
        half _Shininess;
        half _ShadowIntensity;
        // Reuse the lighting function from the main shader
        half4 LightingToonTerrain(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
        {
            // Compute dot product between surface normal and light direction
            half NdotL = dot(s.Normal, lightDir);
            
            // Sample the toon ramp texture based on the light intensity
            half toonRamp = tex2D(_ToonRamp, float2(NdotL * 0.5 + 0.5, 0.5)).r;
            
            // Smooth the ramp threshold
            toonRamp = smoothstep(_RampThreshold - _RampSmooth, _RampThreshold + _RampSmooth, toonRamp);
            toonRamp = lerp(_ShadowIntensity, 1.0, toonRamp);

            // Specular calculation
            half3 halfDir = normalize(lightDir + viewDir);
            half NdotH = dot(s.Normal, halfDir);
            half spec = pow(max(0, NdotH), _Shininess * 128.0) * s.Specular;
            
            // Combine diffuse and specular
            half4 c;
            c.rgb = s.Albedo * _LightColor0.rgb * toonRamp * atten + 
                    _SpecColor.rgb * spec * atten;
            c.a = s.Alpha;
            
            return c;
        }
        

        void surf(Input IN, inout SurfaceOutput o)
        {
            half4 splat_control;
            half weight;
            fixed4 mixedDiffuse;
            
            // Triplanar mapping for vertical texture
            float3 bf = normalize(abs(IN.localNormal));
            bf /= dot(bf, (float3)1);
            
            float texScale = _VerticalTex_ST.x;
            float2 tx = IN.localCoord.yz * texScale;
            float2 ty = IN.localCoord.zx * texScale;
            float2 tz = IN.localCoord.xy * texScale;
            
            // Base color from triplanar mapping
            half4 cx = tex2D(_VerticalTex, tx) * bf.x;
            half4 cy = tex2D(_VerticalTex, ty) * bf.y;
            half4 cz = tex2D(_VerticalTex, tz) * bf.z;
            
            half4 color = (cx + cy + cz);
            
            // Mix splatmap textures
            SplatmapMix(IN, splat_control, weight, mixedDiffuse, o.Normal);
            
            // Choose between splatmap and vertical texture based on surface angle
            if (dot(IN.localNormal, fixed3(0, 1, 0)) >= 0.8)
            {
                o.Albedo = mixedDiffuse.rgb;
            }
            else 
            {
                o.Albedo = color.rgb;
            }
            
            // Set alpha for blending
            o.Alpha = weight;
            
            // Initialize specular for toon ramp lighting
            o.Specular = _Shininess;
        }
    ENDCG
    Category {
        Tags {
            "Queue" = "Geometry-99"
            "IgnoreProjector"="True"
            "RenderType" = "Opaque"
        }
        SubShader { // for sm3.0+ targets
            CGPROGRAM
                #pragma target 3.0
                #pragma multi_compile __ *TERRAIN*NORMAL_MAP
            ENDCG
        }
        SubShader { // for sm2.0 targets
            CGPROGRAM
            ENDCG
        }
    }
    Fallback off
}