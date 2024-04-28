using MorcuTool.Models;

namespace MorcuTool;

public class MaterialSet
{
    public readonly List<MaterialData> mats = new();

    public MaterialSet(Subfile basis) {

        bool useBigEndian = true;
        int start = 0x00;

        if (basis.filebytes[0] == 0x01) {
            useBigEndian = false;
            return; //Processing these MTSTs not yet implemented
        }

        int number_of_materials = Utility.ReadInt32BigEndian(basis.filebytes, start + 0x0C);
      
        var hashes = new List<ulong>();

        for (int i = 0; i < number_of_materials; i++) {
            hashes.Add(Utility.ReadUInt64BigEndian(basis.filebytes, start + 0x18 + (i * 8)));
        }

        foreach (Subfile s in AppState.activePackage.subfiles) {
            if (s.fileType != FileType.MATD_MSK && s.fileType != FileType.MATD_MSA) {
                continue;
            }
            if (hashes.Contains(s.hash)) {
                if (s.filebytes == null || s.filebytes.Length == 0) {
                    s.Load();
                }

                mats.Add(s.matd);
            }
        }
    }
}