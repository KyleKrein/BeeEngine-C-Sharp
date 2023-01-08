#include <metal_stdlib>
using namespace metal;

struct VertexInput{
    float3 a_Position [[attribute(0)]];
    float4 a_Color [[attribute(1)]];
    float2 a_TexCoord [[attribute(2)]];
    float a_TextureIndex [[attribute(3)]];
    float a_TilingFactor [[attribute(4)]];
};

struct ColorInOut{
    float4 v_Position [[position]];
    float4  v_Color;
    float2 v_TexCoord;
    float v_TextureIndex;
    float v_TilingFactor;
};

// Vertex shader function
vertex float4 quad_vertex(const device VertexInput *vertices [[ buffer(0) ]], uint ID[[ vertex_id ]])
{
    ColorInOut out;
   
    out.v_Position = float4(vertices[ID].a_Position, 1);
    out.v_Color = vertices[ID].a_Color;
    out.v_TexCoord = vertices[ID].a_TexCoord;
    out.v_TextureIndex = vertices[ID].a_TextureIndex;
    out.v_TilingFactor = vertices[ID].a_TilingFactor;
    
    return out.v_Position;
}

// Fragment shader function
fragment float4 quad_fragment()
{
    return float4(1);;
}