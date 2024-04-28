namespace MorcuTool;

class mdlObject
{
    public uint startOffset;
    public uint vertexCount;
    public uint vertexListOffset;

    public uint StartingVertexID;

    public uint faceListOffset;

    public uint facesToRemoveOffset;
    public readonly List<ushort> facesToRemove = new();

    public readonly List<Vertex> vertices = new();
    public readonly List<Face> faces = new();
}