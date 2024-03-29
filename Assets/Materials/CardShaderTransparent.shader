Shader "Custom/CardShaderTransparent"
{
 
	Properties
	{
    	_MainTex ("Texture", 2D) = "white" { }
  
	}
  
 
    SubShader
    {
  
	        Tags {"Queue"="Transparent" }
            CGPROGRAM
	        #pragma surface surf Lambert alpha 	

	        sampler2D _MainTex;
        	struct Input
			{
				float2 uv_MainTex;
				float4 color : COLOR;
			};
			
			void surf (Input IN, inout SurfaceOutput o)
			{
				half4 c = tex2D(_MainTex, IN.uv_MainTex);
				o.Albedo = c.rgb * IN.color;
				o.Alpha = 0.15*c.a;
	 
			}
			ENDCG
		

        
 
    }
    Fallback "VertexLit"


}
