namespace MorcuTool;

public class hkSimpleMeshShape : havokObject
{
    public bool disableWelding;

    public readonly List<Vertex> vertices = new();
    public readonly List<hkSimpleMeshShapeTriangle> triangles = new();
    public List<int> materialIndices;
    public double radius;

    public hkSimpleMeshShape(Subfile basis, int objectOffset) {

        Console.WriteLine("Exporting a hkSimpleMeshShape");

        if (basis.filebytes[objectOffset + 16] == 0)
        {
            disableWelding = false;
        }
        else {
            disableWelding = true;
        }

        vertices = havokUtility.ParseHkVertexArray(basis, objectOffset + 20);
        triangles = havokUtility.ParseHkTriangleArray(basis, objectOffset + 32);
        materialIndices = havokUtility.ParseHkIntArray(basis, objectOffset + 44);

        radius = Utility.ReverseEndianSingle(BitConverter.ToSingle(basis.filebytes, objectOffset + 56));
    }
}