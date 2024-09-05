Shader "Blood/BloodStain"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _BloodTex("Blood Texture", 2D) = "white" {}
        _Mouse("Mouse", Vector) = (0,0,0,0)
    }
    SubShader
    {
        Tags {"RenderType" = "Opaque"}
        
        LOD 100
        
        Pass
        {
            CGPROGRAM

            #pragma  vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _BloodTex;
            float4 _Mouse;

            float2 rotate(float2 pt, float theta, float aspect)
            {
                float c = cos(theta);
                float s = sin(theta);
                float2x2 mat = float2x2(c,s,-s,c);
                pt.y /= aspect;
                pt = mul(pt, mat);
                pt.y *= aspect;
                return pt;
            }

            float4 frag (v2f_img i) : COLOR{
                //fixed2 uv = fixed2(1 - i.uv.x, i.uv.y);
                float2 pos = i.uv;
                float2 center = 0.5;
                float2 uv = rotate(i.uv-center, UNITY_HALF_PI, 2.0/1.5) + center;             
                fixed3 color = tex2D(_MainTex, uv).rgb;
                fixed3 bloodColor = tex2D(_BloodTex, i.uv).rgb;

                if(uv.x<0 || uv.x>1||uv.y<0||uv.y>1)
                {
                    color = (0,0,0);
                }
                if(_Mouse.x >= pos.x * center.x && _Mouse.y >= pos.y*center.y)
                {
                    color += bloodColor;
                }
                return  fixed4(color, 1.0);
            }
            ENDCG
        }
    }
}