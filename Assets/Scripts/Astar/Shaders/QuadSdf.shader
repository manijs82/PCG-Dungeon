Shader "Unlit/QuadSdf"
{
    Properties
    {
        _Fill ("Fill", Range(0,1)) = 0.9
        _Color1 ("Color1", Color) = (1,1,1,1)
        _Color2 ("Color2", Color) = (0,0,0,0)
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float4 worldPos : TEXCOORD0;
                float4 localPos : TEXCOORD1;
            };

            float rectangle(float2 samplePosition, float2 halfSize)
            {
                float2 componentWiseEdgeDistance = abs(samplePosition) - halfSize;
                float outsideDistance = length(max(componentWiseEdgeDistance, 0));
                float insideDistance = min(max(componentWiseEdgeDistance.x, componentWiseEdgeDistance.y), 0);
                return outsideDistance + insideDistance;
            }

            float _Fill;
            float4 _Color1;
            float4 _Color2;

            v2f vert(appdata v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.localPos = v.vertex;
                return o;
            }

            fixed4 frag(v2f i) : SV_TARGET
            {
                float dist = rectangle(i.localPos.xy * 2, _Fill);
                dist = (1 - dist);
                if(dist > 1)
                    return _Color1;
                
                return _Color2;
            }
            ENDCG
        }
    }
}