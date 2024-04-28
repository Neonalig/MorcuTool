namespace MorcuTool;

public class MSG
{
    public readonly List<string> strings = new();
    byte version;

    public string MSK_DS_string_to_normal_string(byte[] input) {

            var sb = new StringBuilder(input.Length);

            foreach (byte b in input) {
               // sb.Append(Utility.CharFromMSKDSChar(b));
               // temporarily dummmied out
            }

            return sb.ToString();        
        }

    public string MSA_DS_string_to_normal_string(byte[] input)
    {

            var sb = new StringBuilder(input.Length);

            foreach (byte b in input)
            {
                sb.Append(Utility.CharFromMSADSChar(b));
            }

            return sb.ToString();
        }


    public MSG(byte[] filebytes) {

            version = filebytes[0x08];
            int numTexts = BitConverter.ToInt32(filebytes, 0x09) & 0x00FFFFFF;

            int pos = 0;

            for (int i = 0; i < numTexts; i++) {

                pos = BitConverter.ToInt32(filebytes, 0x10 + (i * 4));

                int startPos = pos;

                while (pos < filebytes.Length && filebytes[pos] != 0x02) {
                    pos++;
                }

                byte[] stringBytes = new byte[pos - startPos];

                for (int j = startPos; j < pos; j++) {
                    stringBytes[j - startPos] = filebytes[j];
                }

                switch (version)
                {
                    case 3:
                        strings.Add(MSA_DS_string_to_normal_string(stringBytes));
                        break;
                    case 4:
                        strings.Add(MSK_DS_string_to_normal_string(stringBytes));
                        break;
                }
            }
        }
}