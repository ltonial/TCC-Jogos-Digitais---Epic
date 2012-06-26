Shader "Two Color" {
  Properties {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _Color0 ("Color 0", Color) = (1,1,1,1)
    _Color1 ("Color 1", Color) = (1,0,0,1)
    _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
    _Shininess ("Shininess", Range (0.01, 1)) = 0.078125
  }
  SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 200

    CGPROGRAM
    #pragma surface surf BlinnPhong

    sampler2D _MainTex;
    float4 _Color0;
    float4 _Color1;
    half _Shininess;

    struct Input {
      float2 uv_MainTex;
    };

    void surf (Input IN, inout SurfaceOutput o) {
      fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
      fixed4 color = lerp(_Color0, _Color1, IN.uv_MainTex.y);
      o.Albedo = tex.rgb * color.rgb;
      o.Gloss = tex.a;
      o.Alpha = tex.a * color.a;
      o.Specular = _Shininess;
    }
    ENDCG
  }
  FallBack "Specular"
}
