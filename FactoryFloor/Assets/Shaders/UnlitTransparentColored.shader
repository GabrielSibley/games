 Shader "Unlit/TransparentColored" {
     Properties {
         _Color("Tint", Color) = (1, 1, 1, 1)
         _MainTex ("Texture (RGB) Alpha (A)", 2D) = "white"
     }
     SubShader {
         Lighting Off
         ZWrite Off
         Cull Back
         Blend SrcAlpha OneMinusSrcAlpha
         Tags {"Queue" = "Transparent"}
         Color[_Color]
            Pass {
               SetTexture [_MainTex] {
                      Combine Texture * Primary, Texture * Primary
                }
            }
     } 
     FallBack "Unlit/Transparent"
 }