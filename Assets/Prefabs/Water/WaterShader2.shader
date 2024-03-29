Shader "Custom/WaterShader2"
{
    Properties
    {
        _DepthGradientShallow("Depth Gradient Shallow", Color) = (0.325, 0.807, 0.971, 0.725)
        _DepthGradientDeep("Depth Gradient Deep", Color) = (0.086, 0.407, 1, 0.749)
        _DepthMaxDistance("Depth Maximum Distance", Float) = 1

        _SurfaceNoise("Surface Noise", 2D) = "white" {}
        _SurfaceNoiseCutoff("Surface Noise Cutoff", Float) = 0.777
        _SurfaceNoiseScroll("Surface Noise Scroll Amount", Vector) = (0.3, 0.3, 0, 0)

        _SurfaceDistortion("Surface Distortion Noise", 2D) = "white" {}
        _SurfaceDistortionAmount("Surface Distortion Amount", Float) = 0.27

        _FoamDistance("Foam Distance", Float) = 0.4
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float4 _DepthGradientShallow;
            float4 _DepthGradientDeep;

            float _DepthMaxDistance;

            sampler2D _CameraDepthTexture;

            sampler2D _SurfaceNoise;
            float4 _SurfaceNoise_ST;

            float _SurfaceNoiseCutoff;

            float2 _SurfaceNoiseScroll;

            sampler2D _SurfaceDistortion;
            float4 _SurfaceDistortion_ST;

            float _SurfaceDistortionAmount;

            float _FoamDistance;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 noiseUV : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 screenPosition : TEXCOORD2;
                float2 distortUV : TEXCOORD1;
            };

            float CorrectDepth(float rawDepth)
            {
                float persp = LinearEyeDepth(rawDepth);
                float ortho = (_ProjectionParams.z-_ProjectionParams.y)*(1-rawDepth)+_ProjectionParams.y;
                return lerp(persp,ortho,unity_OrthoParams.w);
            }

            v2f vert(appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);

                o.screenPosition = ComputeScreenPos(o.vertex);

                o.noiseUV = TRANSFORM_TEX(v.uv, _SurfaceNoise);

                o.distortUV = TRANSFORM_TEX(v.uv, _SurfaceDistortion);
    
                return o;
            }

            float4 frag(v2f i): SV_Target
            {
                float existingDepth1 = tex2Dproj(_CameraDepthTexture,
                    UNITY_PROJ_COORD(i.screenPosition)).r;
                float existingDepthLinear = LinearEyeDepth(existingDepth1);

                float depthDifference = existingDepthLinear -
                    i.screenPosition.w;
                
                //saturate is just clamp between 0 and 1

                float waterDepthDistance1 = saturate(depthDifference / _DepthMaxDistance);

                float4 waterColor = lerp(_DepthGradientShallow, _DepthGradientDeep,
                waterDepthDistance1);

                float2 distortSample = (tex2D(_SurfaceDistortion, i.distortUV)   * 2 - 1) * _SurfaceDistortionAmount;

                float2 scrollingNoiseUV = float2((i.noiseUV.x + _Time.y * _SurfaceNoiseScroll.x) + distortSample.x,
                    (i.noiseUV.y + _Time.y * _SurfaceNoiseScroll.y) + distortSample.y);

                float surfaceNoiseSample = tex2D(_SurfaceNoise, scrollingNoiseUV).r;

                float foamDepthDifference1 = saturate(depthDifference / _FoamDistance);
                    
                float surfaceNoiseCutoff = foamDepthDifference1 * _SurfaceNoiseCutoff;

                float surfaceNoise = surfaceNoiseSample > surfaceNoiseCutoff ? 1 : 0;


                return waterColor + surfaceNoise;
            }
            ENDCG
        }
    }
}
