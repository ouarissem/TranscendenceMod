sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect;
float2 uZoom;


float4 Sinewave(float2 coords : TEXCOORD0) : COLOR0
{
    float4 ogColor = tex2D(uImage0, coords);
    float4 color = tex2D(uImage0, coords);

    float2 pos = float2(0.5, 0.5);
    float distance = length(coords.x - pos.x) * 16;
    float distance2 = length(coords.y - pos.y) * 32;
    float distance3 = (distance + distance2) / 4;

    float4 col = lerp(ogColor, color * 0.125f, uOpacity);
    if (distance3 < 0.5)
        return col;
    color = lerp(col, float4(0, 0, 0, 1), (distance3 - 0.5) * uOpacity);
    
    return color;
}

    
technique BlindTechnique
{
    pass BlindTechnique2
    {
        PixelShader = compile ps_2_0 Sinewave();
    }
}