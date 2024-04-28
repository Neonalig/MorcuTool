using MorcuTool.MorcuMath;

namespace MorcuTool;

public class WindowsModel //WMDL
{
    public DateTime date;

    public readonly List<Mesh> meshes = new();
    public readonly List<Rig> rigs = new();

    public class Mesh
    {
        public readonly List<Vertex> vertices = new();
        public readonly List<Face> faces = new();

        public List<MaterialData> materials = new();

        public ulong hash_of_material = 0;
    }

    public class Rig {

        public uint[] boneHashes = Array.Empty<uint>();
        public int numBones = 0;
        public Bone[] bones = Array.Empty<Bone>();

    }

    public class Bone {

        public uint hash;
        public float x;
        public float y;
        public float z;            
        
    }

    public WindowsModel(Subfile basis)
    {
        meshes = new List<Mesh>();
        rigs = new List<Rig>();

        int pos = 0;

        pos = 0x21;

        int numExtraParams = BitConverter.ToInt32(basis.filebytes, pos);
        pos += 4;

        for (int i = 0; i < numExtraParams; i++)
        {
            uint paramIDMaybe = BitConverter.ToUInt32(basis.filebytes, pos);
            pos += 4;
        }

        if (numExtraParams != 0)
        {
            int amountOfSpaceDedicatedToParamNames = BitConverter.ToInt32(basis.filebytes, pos);
            pos += 4;
        }

        for (int i = 0; i < numExtraParams; i++)
        {
            string paramName = "";
            while (basis.filebytes[pos] != 0x00)
            {
                paramName += $"{(char)basis.filebytes[pos]}";
                pos++;
            }

            if (i < numExtraParams - 1) {
                pos++;  //skip null char
            }

            //known param names:
            //"no_shadow" (does not cast a shadow)
            //"1" (has a rig?)
        }

        pos++; //skip null char; it's also there even if there are no params

        int numRigs = BitConverter.ToInt32(basis.filebytes, pos);
        pos += 4;
            
        for (int i = 0; i < numRigs; i++)
        {
            var newRig = new Rig
            {
                numBones = BitConverter.ToInt32(basis.filebytes, pos),
            };
            pos += 4;
            newRig.boneHashes = new uint[newRig.numBones];
            newRig.bones = new Bone[newRig.numBones];

            for (int j = 0; j < newRig.numBones; j++)
            {
                newRig.boneHashes[j] = BitConverter.ToUInt32(basis.filebytes, pos);
                pos += 4;
            }

            for (int j = 0; j < newRig.numBones; j++)
            {
                var newBone = new Bone
                {
                    hash = newRig.boneHashes[j],
                };

                pos += 0x30;  //some bone data that is not yet parsed

                newBone.x = BitConverter.ToSingle(basis.filebytes, pos);
                pos += 4;
                newBone.y = BitConverter.ToSingle(basis.filebytes, pos);
                pos += 4;
                newBone.z = BitConverter.ToSingle(basis.filebytes, pos);
                pos += 4;

                pos += 4; //skip another unknown bone data dword

                newRig.bones[j] = newBone;
            }

            rigs.Add(newRig);
        }

        int numMeshes = BitConverter.ToInt32(basis.filebytes, pos);
        pos += 4;

        for (int i = 0; i < numMeshes; i++)
        {
            var newMesh = new Mesh();
            //mesh begins
            pos += 0x44;
            int vertexCount = BitConverter.ToInt32(basis.filebytes, pos);
            pos += 4;
            int faceCount = BitConverter.ToInt32(basis.filebytes, pos);
            pos += 4;
            int vertType = BitConverter.ToInt32(basis.filebytes, pos);      //0x03: XYZpos, XYZnrm, UV? (length 0x20 in total)        0x05: XYZpos, XYZnrm, UV, boneweights? (length 0x30 in total)
            pos += 4;

            switch (vertType) {     //It may be that 3, 4 and 5 here represent bitwise options for the vertex components
                case 3:
                    pos += 0x15;
                    break;
                case 4:
                    pos += 0x1C;
                    break;
                case 5:
                    pos += 0x23;
                    break;
                case 6:
                    pos += 0x2A;
                    break;
                default:
                    MessageBox.Show($"UNKNOWN VERT TYPE IN WMDL: {vertType} IN FILE WITH DECIMAL HASH {basis.hash}");
                    break;
            }
                
            int lengthOfVertexSection = BitConverter.ToInt32(basis.filebytes, pos);
            pos += 4;

            //floats follow
            //seems like 0x20 bytes per vertex? That's enough for XYZpos, XYZnrm, and UV.
            //But if vertType above is 5 (which it is for rigged models) there are 0x10 extra bytes, probably of bone weight data.

            for (int j = 0; j < vertexCount; j++)
            {

                var newVertex = new Vertex
                {
                    position =
                    {
                        x = BitConverter.ToSingle(basis.filebytes, pos),
                    },
                };

                pos += 4;
                newVertex.position.y = BitConverter.ToSingle(basis.filebytes, pos);
                pos += 4;
                newVertex.position.z = BitConverter.ToSingle(basis.filebytes, pos);
                pos += 4;

                if (vertType is 4 or 5 or 6) {
                    pos += 0x10; //account for bone weight data, though we are not interpreting it for now
                }

                newVertex.normal.x = BitConverter.ToSingle(basis.filebytes, pos);
                pos += 4;
                newVertex.normal.y = BitConverter.ToSingle(basis.filebytes, pos);
                pos += 4;
                newVertex.normal.z = BitConverter.ToSingle(basis.filebytes, pos);
                pos += 4;

                if (vertType is 3 or 5 or 6)
                {
                    newVertex.UVchannels[0] = new Vector2
                    {
                        x = BitConverter.ToSingle(basis.filebytes, pos),
                    };
                    pos += 4;
                    newVertex.UVchannels[0].y = BitConverter.ToSingle(basis.filebytes, pos);
                    pos += 4;
                }

                if (vertType == 6)  //I don't actually know for certain whether type 6's contribution is a second UV channel as portrayed here, but type 6 does add space for two more floats so it looks like it might fit
                {
                    newVertex.UVchannels[1] = new Vector2
                    {
                        x = BitConverter.ToSingle(basis.filebytes, pos),
                    };
                    pos += 4;
                    newVertex.UVchannels[1].y = BitConverter.ToSingle(basis.filebytes, pos);
                    pos += 4;
                }


                newMesh.vertices.Add(newVertex);
            }

            int lengthOfFaceSection = BitConverter.ToInt32(basis.filebytes, pos);
            pos += 4;

            for (int j = 0; j < faceCount; j++)
            {
                var newFace = new Face
                {
                    v1 = BitConverter.ToInt16(basis.filebytes, pos),
                };
                pos += 2;
                newFace.v2 = BitConverter.ToInt16(basis.filebytes, pos);
                pos += 2;
                newFace.v3 = BitConverter.ToInt16(basis.filebytes, pos);
                pos += 2;
                newMesh.faces.Add(newFace);
            }

            pos += 4; //FF FF FF FF padding
            meshes.Add(newMesh);
        }
    }

    public void GenerateObj(string outputPath) 
    {
        var obj = new List<string>();

        int cumuVertices = 0;

        foreach (Mesh m in meshes)
        {

            obj.Add($"object {meshes.IndexOf(m)}");

            for (int v = 0; v < m.vertices.Count; v++)
            {
                obj.Add($"v {m.vertices[v].position.x} {m.vertices[v].position.y} {m.vertices[v].position.z}");
                obj.Add($"vn {m.vertices[v].normal.x} {m.vertices[v].normal.y} {m.vertices[v].normal.z}");
                if (m.vertices[v].UVchannels[0] != null) {
                    obj.Add($"vt {m.vertices[v].UVchannels[0].x} {(m.vertices[v].UVchannels[0].y * -1)}");
                }
            }

            for (int f = 0; f < m.faces.Count; f++)
            {
                obj.Add($"f {(m.faces[f].v1 + 1 + cumuVertices)}/{(m.faces[f].v1 + 1 + cumuVertices)}/{(m.faces[f].v1 + 1 + cumuVertices)} {(m.faces[f].v2 + 1 + cumuVertices)}/{(m.faces[f].v2 + 1 + cumuVertices)}/{(m.faces[f].v2 + 1 + cumuVertices)} {(m.faces[f].v3 + 1 + cumuVertices)}/{(m.faces[f].v3 + 1 + cumuVertices)}/{(m.faces[f].v3 + 1 + cumuVertices)}");
            }

            cumuVertices += m.vertices.Count;
        }

        File.WriteAllLines(outputPath, obj.ToArray());
    }
}