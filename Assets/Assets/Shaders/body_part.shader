Shader "Custom/RecolorWithMask"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _MaskTex ("Mask Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags {"Queue"="Transparent"}
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert alpha

        sampler2D _MainTex;
        sampler2D _MaskTex;
        fixed4 _Color;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_MaskTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Adjust UV coordinates for flipped sprites
            float2 uvMain = IN.uv_MainTex;
            float2 uvMask = IN.uv_MaskTex;

            // Handle flipping
            #if UNITY_UV_STARTS_AT_TOP
            bool flipX = _ScreenParams.x < 0.0;
            bool flipY = _ScreenParams.y < 0.0;
            if (flipX) uvMain.x = 1.0 - uvMain.x;
            if (flipY) uvMain.y = 1.0 - uvMain.y;
            if (flipX) uvMask.x = 1.0 - uvMask.x;
            if (flipY) uvMask.y = 1.0 - uvMask.y;
            #endif

            fixed4 c = tex2D (_MainTex, uvMain);
            fixed4 mask = tex2D (_MaskTex, uvMask);

            o.Albedo = c.rgb * (1 - mask.r) + _Color.rgb * mask.r;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
