using MorcuTool.MorcuMath;

namespace MorcuTool;

public class Vertex
{
    uint startingVertexID;

    public Vector3 position = new();
    public float W; //when needed

    public readonly Vector2[] UVchannels = new Vector2[8];

    public Vector3 normal = new();

    public readonly Vector3 binormal = new();

    public readonly Vector3 tangent = new();

    public Color color;

    public Vertex() { 
    }
    public Vertex(float x, float y, float z) {
        position = new Vector3(x, y, z);
    }
}