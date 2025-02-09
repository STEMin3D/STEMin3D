// credit to user Bgolus on Unity forum for parts of this code

Shader "Unlit/SphereShader"
{
    Properties
    {
        _Tint ("Tint Color", Color) = (1, 1, 1, 0.5)
        [Gamma] _Exposure ("Exposure", Range(0, 8)) = 1.0
        _CubeMap( "Cube Map (HDR)", Cube ) = "gray" {}
        _Transparency ("Transparency", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "DisableBatching" = "True" "Queue"="Transparent" "IgnoreProjector"="false" "RenderType"="Transparent"}
            ZWrite Off
            Cull Front
            Blend SrcAlpha OneMinusSrcAlpha
        Pass 
        {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
        
            samplerCUBE _CubeMap;
            half4 _CubeMap_HDR;
            half4 _Tint;
            half _Exposure;
            float _Transparency;

            struct v2f 
            {
                float4 pos : SV_Position;
                half3 uv : TEXCOORD0;
            };
        
            v2f vert( appdata_img v )
            {
                v2f o;
                o.pos = UnityObjectToClipPos( v.vertex );
                o.uv = v.vertex.xyz * half3(-1,1,1); // mirror so cubemap projects as expected
                return o;
            }
        
            fixed4 frag( v2f i ) : SV_Target 
            {
                half4 tex = texCUBE (_CubeMap, i.uv);
                half3 c = DecodeHDR (tex, _CubeMap_HDR);
                c = c * _Tint.rgb * unity_ColorSpaceDouble;
                c *= _Exposure;
                return half4(c, _Transparency);
            }
            ENDCG
        }
    }
}