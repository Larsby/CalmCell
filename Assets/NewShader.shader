Shader "Custom/BlendMaterials"
{

    Properties {
        _Tint ("Tint Color", Color) = (.9, .9, .9, 1.0)
        _TexMat1 ("Texture Material1", 2D) = "white" {}
        _TexMat2 ("Texture Material2", 2D) = "white" {}
        _Blend ("Blend", Range(0.0,1.0)) = 0.0
     

         _Cutoff ("Alpha cutoff", Range (0,1)) = 0.5
    }
   
    Category {
        ZWrite On
        Alphatest Greater [_Cutoff]
        Tags {Queue=Transparent}
        Blend SrcColor DstColor
        ColorMask RGB
    SubShader {
        Pass {
           
            Material {
                Diffuse [_Tint]
                Ambient [_Tint]
            }
            Lighting On
           
            SetTexture [_TexMat1] { combine texture }
            SetTexture [_TexMat2] { constantColor (0,0,0,[_Blend]) combine texture lerp(constant) previous }
            SetTexture [_TexMat2] { combine previous +- primary, previous * primary }
        }
    }
    FallBack " Diffuse", 1
}
}