// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Slime"
{
	Properties
	{
		_NumSteps("NumSteps", Int) = 0
		_StepSize("StepSize", Float) = 0
		_Sphere("Sphere", Vector) = (0,0,0,0)
		_Sphere2("Sphere2", Vector) = (0,0,0,0)
		_DensityScale("DensityScale", Float) = 0
		_Lower("Lower", Float) = 0
		_Thresh("Thresh", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float3 worldPos;
		};

		uniform float _Lower;
		uniform float _Thresh;
		uniform int _NumSteps;
		uniform float _StepSize;
		uniform float4 _Sphere;
		uniform float4 _Sphere2;
		uniform float _DensityScale;


		float Raymarch2( float3 rayOrigin, float Lower, float Thresh, float3 rayDir, int numSteps, float stepSize, float4 Sphere, float4 Sphere2, float densityScale )
		{
			float density;
			for(int i = 0; i < numSteps; i++){
				rayOrigin += (rayDir * stepSize);
				float sphereDist = distance(rayOrigin, Sphere.xyz);
				float sphereDist2 = distance(rayOrigin, float3(Sphere2.x, Sphere2.y *0, Sphere2.z));
				density += .1 * densityScale * smoothstep(Lower, Sphere.w, sphereDist);
				density += .1 * densityScale * smoothstep(Lower, Sphere2.w, sphereDist2);
				//density = step(0.5, density);
				//if(sphereDist < Sphere.w || sphereDist2 < Sphere2.w){
				//	density += .1 * densityScale;
				//}else{
				//}
			}
			return smoothstep(0, Thresh, exp(-density));
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 rayOrigin2 = ase_worldPos;
			float Lower2 = _Lower;
			float Thresh2 = _Thresh;
			float3 rayDir2 = ( ase_worldPos - _WorldSpaceCameraPos );
			int numSteps2 = _NumSteps;
			float stepSize2 = _StepSize;
			float4 Sphere2 = _Sphere;
			float4 Sphere22 = _Sphere2;
			float densityScale2 = _DensityScale;
			float localRaymarch2 = Raymarch2( rayOrigin2 , Lower2 , Thresh2 , rayDir2 , numSteps2 , stepSize2 , Sphere2 , Sphere22 , densityScale2 );
			float3 temp_cast_0 = (localRaymarch2).xxx;
			o.Emission = temp_cast_0;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18912
2553;42;2560;1329;2027.67;688.3878;1.3;True;True
Node;AmplifyShaderEditor.WorldPosInputsNode;4;-741,-154;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceCameraPos;6;-880,13;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleSubtractOpNode;5;-553,28;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.IntNode;7;-726,174;Inherit;False;Property;_NumSteps;NumSteps;0;0;Create;True;0;0;0;False;0;False;0;64;False;0;1;INT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-743,281;Inherit;False;Property;_StepSize;StepSize;1;0;Create;True;0;0;0;False;0;False;0;0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-730.2702,726.6624;Inherit;False;Property;_DensityScale;DensityScale;4;0;Create;True;0;0;0;False;0;False;0;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;10;-887.1992,365.4001;Inherit;False;Property;_Sphere;Sphere;2;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.5,0,0,0.6;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;12;-816.0703,542.7117;Inherit;False;Property;_Sphere2;Sphere2;3;0;Create;True;0;0;0;False;0;False;0,0,0,0;-0.5,0,0,0.6;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;13;-481.9702,-345.1878;Inherit;False;Property;_Lower;Lower;5;0;Create;True;0;0;0;False;0;False;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-513.1701,-241.188;Inherit;False;Property;_Thresh;Thresh;6;0;Create;True;0;0;0;False;0;False;0;0.04;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;1;-297.8001,-267.0999;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CustomExpressionNode;2;-242.8,111;Inherit;False;float density@$$for(int i = 0@ i < numSteps@ i++){$	rayOrigin += (rayDir * stepSize)@$	float sphereDist = distance(rayOrigin, Sphere.xyz)@$	float sphereDist2 = distance(rayOrigin, float3(Sphere2.x, Sphere2.y *0, Sphere2.z))@$$	density += .1 * densityScale * smoothstep(Lower, Sphere.w, sphereDist)@$	density += .1 * densityScale * smoothstep(Lower, Sphere2.w, sphereDist2)@$	//density = step(0.5, density)@$	//if(sphereDist < Sphere.w || sphereDist2 < Sphere2.w){$	//	density += .1 * densityScale@$	//}else{$	//}$}$$return smoothstep(0, Thresh, exp(-density))@;1;Create;9;True;rayOrigin;FLOAT3;0,0,0;In;;Inherit;False;True;Lower;FLOAT;0;In;;Inherit;False;True;Thresh;FLOAT;0;In;;Inherit;False;True;rayDir;FLOAT3;0,0,0;In;;Inherit;False;True;numSteps;INT;0;In;;Inherit;False;True;stepSize;FLOAT;0;In;;Inherit;False;True;Sphere;FLOAT4;0,0,0,0;In;;Inherit;False;True;Sphere2;FLOAT4;0,0,0,0;In;;Inherit;False;True;densityScale;FLOAT;0;In;;Inherit;False;Raymarch;True;False;0;;False;9;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;INT;0;False;5;FLOAT;0;False;6;FLOAT4;0,0,0,0;False;7;FLOAT4;0,0,0,0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;105,-84;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Slime;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;4;0
WireConnection;5;1;6;0
WireConnection;2;0;4;0
WireConnection;2;1;13;0
WireConnection;2;2;14;0
WireConnection;2;3;5;0
WireConnection;2;4;7;0
WireConnection;2;5;9;0
WireConnection;2;6;10;0
WireConnection;2;7;12;0
WireConnection;2;8;11;0
WireConnection;0;2;2;0
ASEEND*/
//CHKSM=D8135B7F6F7971CD8DBF2D518B3A53F599E3E4CB