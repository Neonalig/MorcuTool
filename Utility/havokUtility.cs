namespace MorcuTool;

public static class havokUtility
{

    public static List<Vertex> ParseHkVertexArray(Subfile basis, int offset) {

        int length = Utility.ReverseEndianSigned(BitConverter.ToInt32(basis.filebytes, offset + 4));
        int capacityAndFlags = Utility.ReverseEndianSigned(BitConverter.ToInt32(basis.filebytes, offset + 8));

        if (length == 0) {
            return null;
        }

        Console.WriteLine($"Going to look for offset {offset} in the local fixup table");

        offset = basis.hkx.getLocalFixUpOffset(offset - basis.hkx.dataSection.sectionOffset) + basis.hkx.dataSection.sectionOffset; //go to the offset of the actual array, using the local fixup table

        //now begins the unique bit  

        var output = new List<Vertex>();

        for (int i = 0; i < length; i++) {

            var v = new Vertex
            {
                position =
                {
                    x = Utility.ReverseEndianSingle(BitConverter.ToSingle(basis.filebytes, offset)),
                    y = Utility.ReverseEndianSingle(BitConverter.ToSingle(basis.filebytes, offset+4)),
                    z = Utility.ReverseEndianSingle(BitConverter.ToSingle(basis.filebytes, offset+8)),
                },
                W = Utility.ReverseEndianSingle(BitConverter.ToSingle(basis.filebytes, offset+12)),
            };

            output.Add(v);
            //Console.WriteLine("vertex: "+v.X+","+v.Y+","+v.Z);
            offset += 16;
            
        }
        return output;
    }

    public static List<hkSimpleMeshShapeTriangle> ParseHkTriangleArray(Subfile basis, int offset) {
        int length = Utility.ReverseEndianSigned(BitConverter.ToInt32(basis.filebytes, offset + 4));
        int capacityAndFlags = Utility.ReverseEndianSigned(BitConverter.ToInt32(basis.filebytes, offset + 8));

        if (length == 0)
        {
            return null;
        }

        Console.WriteLine($"Going to look for offset {offset} in the local fixup table");

        offset = basis.hkx.getLocalFixUpOffset(offset - basis.hkx.dataSection.sectionOffset) + basis.hkx.dataSection.sectionOffset; //go to the offset of the actual array, using the local fixup table

        //now begins the unique bit  

        var output = new List<hkSimpleMeshShapeTriangle>();

        for (int i = 0; i < length; i++)
        {

            var t = new hkSimpleMeshShapeTriangle
            {
                v1 = Utility.ReverseEndianSigned(BitConverter.ToInt32(basis.filebytes, offset)),
                v2 = Utility.ReverseEndianSigned(BitConverter.ToInt32(basis.filebytes, offset + 4)),
                v3 = Utility.ReverseEndianSigned(BitConverter.ToInt32(basis.filebytes, offset + 8)),
            };

            output.Add(t);
            //Console.WriteLine("triangle: " + t.v1 + "," + t.v2 + "," + t.v3);
            offset += 12;
        }

        return output;
    }

    public static List<int> ParseHkIntArray(Subfile basis, int offset)
    {
        int length = Utility.ReverseEndianSigned(BitConverter.ToInt32(basis.filebytes, offset + 4));
        int capacityAndFlags = Utility.ReverseEndianSigned(BitConverter.ToInt32(basis.filebytes, offset + 8));

        if (length == 0)
        {
            return null;
        }

        Console.WriteLine($"Going to look for offset {offset} in the local fixup table");

        offset = basis.hkx.getLocalFixUpOffset(offset - basis.hkx.dataSection.sectionOffset) + basis.hkx.dataSection.sectionOffset; //go to the offset of the actual array, using the local fixup table

        //now begins the unique bit  

        var output = new List<int>();

        for (int i = 0; i < length; i++)
        {
            output.Add(Utility.ReverseEndianSigned(BitConverter.ToInt32(basis.filebytes, offset)));
            Console.WriteLine($"int: {output[output.Count - 1]}");
            offset += 4;
        }

        return output;
    }
}