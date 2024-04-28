using MorcuTool.Models;
using MorcuTool.ViewModels.Pages;

namespace MorcuTool;

public class Package
{
    public byte[] filebytes = Array.Empty<byte>();
    public string filename = "";

    public PackageType packageType = PackageType.Agents; //defaults to agents


    public uint majorversion = 0x00;
    public uint minorversion = 0x00;
    public uint unknown1 = 0x00;
    public uint unknown2 = 0x00;
    public uint unknown3 = 0x00;
    public DateTime date = new();
    public uint indexmajorversion = 0x00;

    public uint filecount = 0x00;
    public uint indexoffsetdeprecated = 0x00;
    public uint indexsize = 0x00;
    public uint holeentrycount = 0x00;
    public uint holeoffset = 0x00;
    public uint holesize = 0x00;
    public uint indexminorversion = 0x00;
    public uint unknown4 = 0x00;
    public uint indexoffset = 0x00; //offset of the index table
    public uint unknown5 = 0x00;
    public uint unknown6 = 0x00;
    public uint reserved1 = 0x00;
    public uint reserved2 = 0x00;

    uint MSKindexversion = 0x00;

    public readonly List<IndexEntry> IndexEntries = new();
    public ulong indexnumberofentries = 0x00;

    public List<Subfile> subfiles = new();

    public PackageViewPageViewModel viewModel;


    public enum PackageType
    {
        Kingdom = 0x00,
        Agents = 0x01,
        SkyHeroes = 0x02
    }


    public uint GetNumOccurrencesOfTypeID(uint typeID)
    {
        foreach (IndexEntry entry in IndexEntries)
        {
            if (entry.typeID == typeID)
            {
                return entry.typeNumberOfInstances;
            }
        }

        return 0;
    }


    public void LoadPackage()
    {
        using var reader = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite));
        uint magic;

        magic = Utility.ReverseEndian(reader.ReadUInt32());

        switch (magic)
        {
            case 0x44425046: //DBPF
                packageType = PackageType.Kingdom;
                break;

            case 0x46504244: //FPBD
                packageType = PackageType.Agents;
                break;

            case 0x030502ED: //Skyheroes
            case 0x03051771: //Skyheroes
                packageType = PackageType.SkyHeroes;
                reader.Close();
                LoadSkyHeroesPackage();
                return;

            default:
                MessageBox.Show("This is not a valid package!", "Package not supported", MessageBoxButton.OK, MessageBoxImage.Information);
                Console.WriteLine("invalid file magic");
                break;
        }


        if (packageType == PackageType.Agents)
        {
            majorversion = Utility.ReverseEndian(reader.ReadUInt32());
            minorversion = Utility.ReverseEndian(reader.ReadUInt32());
            unknown1 = Utility.ReverseEndian(reader.ReadUInt32());
            unknown2 = Utility.ReverseEndian(reader.ReadUInt32());
            unknown3 = Utility.ReverseEndian(reader.ReadUInt32());
            date = DateTime.FromBinary(Utility.ReverseEndianLong(reader.ReadInt64()));
            indexmajorversion = Utility.ReverseEndian(reader.ReadUInt32());

            filecount = Utility.ReverseEndian(reader.ReadUInt32());
            indexoffsetdeprecated = Utility.ReverseEndian(reader.ReadUInt32());
            indexsize = Utility.ReverseEndian(reader.ReadUInt32());
            holeentrycount = Utility.ReverseEndian(reader.ReadUInt32());
            holeoffset = Utility.ReverseEndian(reader.ReadUInt32());
            holesize = Utility.ReverseEndian(reader.ReadUInt32());
            indexminorversion = Utility.ReverseEndian(reader.ReadUInt32());
            unknown4 = Utility.ReverseEndian(reader.ReadUInt32());
            indexoffset = Utility.ReverseEndian(reader.ReadUInt32());
            unknown5 = Utility.ReverseEndian(reader.ReadUInt32());
            unknown6 = Utility.ReverseEndian(reader.ReadUInt32());
            reserved1 = Utility.ReverseEndian(reader.ReadUInt32());
            reserved2 = Utility.ReverseEndian(reader.ReadUInt32());
        }
        else
        {
            majorversion = reader.ReadUInt32();
            minorversion = reader.ReadUInt32();
            unknown1 = reader.ReadUInt32();
            unknown2 = reader.ReadUInt32();
            unknown3 = reader.ReadUInt32();
            date = DateTime.FromBinary(Utility.ReverseEndianLong(reader.ReadInt64()));
            indexmajorversion = reader.ReadUInt32();

            filecount = reader.ReadUInt32();
            indexoffsetdeprecated = reader.ReadUInt32();
            indexsize = reader.ReadUInt32();
            holeentrycount = reader.ReadUInt32();
            holeoffset = reader.ReadUInt32();
            holesize = reader.ReadUInt32();
            indexminorversion = reader.ReadUInt32();
            indexoffset = reader.ReadUInt32();
            unknown5 = reader.ReadUInt32();
            unknown6 = reader.ReadUInt32();
            reserved1 = reader.ReadUInt32();
            reserved2 = reader.ReadUInt32();
        }

        Console.WriteLine($"Package date: {date}");


        reader.BaseStream.Position = indexoffset;

        //index header

        if (packageType == PackageType.Agents)
        {
            indexnumberofentries = Utility.ReverseEndianULong(reader.ReadUInt64());

            for (uint i = 0; i < indexnumberofentries; i++) //a bunch of entries that describe how many files there are of each type
            {
                var newEntry = new IndexEntry
                {
                    typeID = Utility.ReverseEndian(reader.ReadUInt32()),
                    groupID = Utility.ReverseEndian(reader.ReadUInt32()),
                    typeNumberOfInstances = Utility.ReverseEndian(reader.ReadUInt32()),
                    indexnulls = Utility.ReverseEndian(reader.ReadUInt32()),
                };

                IndexEntries.Add(newEntry);
            }

            var typeIndex = 0;
            var instancesOfType = 0;

            for (uint i = 0; i < filecount; i++) //go through the files, they are organised by type, one type after the other. (So X number of type A, as described above, then Y number of type B...)
            {
                var newSubfile = new Subfile
                {
                    hash = Utility.ReverseEndianULong(reader.ReadUInt64()),
                    fileoffset = Utility.ReverseEndian(reader.ReadUInt32()),
                    filesize = Utility.ReverseEndian(reader.ReadUInt32()),
                    fileType = FileType.FromValue(IndexEntries[typeIndex].typeID),
                    groupID = IndexEntries[typeIndex].groupID,
                };

                if (IndexEntries[typeIndex].groupID != 0) //it's compressed
                {
                    newSubfile.should_be_compressed_when_in_package = true;
                    newSubfile.uncompressedsize = Utility.ReverseEndian(reader.ReadUInt32());
                    Console.WriteLine("Found a compressed file");
                }

                instancesOfType++;

                if (instancesOfType == IndexEntries[typeIndex].typeNumberOfInstances)
                {
                    Console.WriteLine($"Processed {instancesOfType} instances of {typeIndex}");
                    typeIndex++;
                    instancesOfType = 0;
                }

                subfiles.Add(newSubfile);
            }
        }
        else //MySims and MySims Kingdom use these
        {
            MSKindexversion = reader.ReadUInt32();

            switch (MSKindexversion)
            {
                case 0:
                {
                    Console.WriteLine("index version 0");
                    for (uint i = 0; i < filecount; i++)
                    {
                        var newSubfile = new Subfile
                        {
                            fileType = FileType.FromValue(reader.ReadUInt32()),
                            groupID = reader.ReadUInt32(),
                        };

                        reader.BaseStream.Position += 8;

                        newSubfile.fileoffset = reader.ReadUInt32();
                        newSubfile.filesize = reader.ReadUInt32() & 0x7FFFFFFF;
                        newSubfile.uncompressedsize = reader.ReadUInt32();
                        reader.BaseStream.Position += 0x04; //flags

                        newSubfile.hash = newSubfile.fileoffset;

                        if (newSubfile.filesize == newSubfile.uncompressedsize)
                        {
                            newSubfile.uncompressedsize = 0;
                        }
                        else
                        {
                            newSubfile.should_be_compressed_when_in_package = true;
                        }

                        subfiles.Add(newSubfile);
                    }

                    break;
                }
                case 1:
                    throw new NotImplementedException("Index version 1 not implemented!");
                case 2:
                {
                    reader.BaseStream.Position += 4;

                    for (uint i = 0; i < filecount; i++)
                    {
                        var newSubfile = new Subfile
                        {
                            fileType = FileType.FromValue(reader.ReadUInt32()),
                            hash = (ulong)(reader.ReadUInt32()) << 32,
                        };

                        newSubfile.hash |= reader.ReadUInt32();

                        newSubfile.fileoffset = reader.ReadUInt32();
                        newSubfile.filesize = reader.ReadUInt32() & 0x7FFFFFFF;
                        newSubfile.uncompressedsize = reader.ReadUInt32();
                        reader.BaseStream.Position += 0x04; //flags

                        if (newSubfile.filesize == newSubfile.uncompressedsize)
                        {
                            newSubfile.uncompressedsize = 0;
                        }
                        else
                        {
                            newSubfile.should_be_compressed_when_in_package = true;
                        }

                        subfiles.Add(newSubfile);
                    }

                    break;
                }
                case 3:
                {
                    FileType allFilesType = FileType.FromValue(reader.ReadUInt32());
                    reader.BaseStream.Position += 4;

                    for (uint i = 0; i < filecount; i++)
                    {
                        var newSubfile = new Subfile
                        {
                            fileType = allFilesType,
                            hash = (ulong)(reader.ReadUInt32()) << 32,
                        };

                        newSubfile.hash |= reader.ReadUInt32();

                        newSubfile.fileoffset = reader.ReadUInt32();
                        newSubfile.filesize = reader.ReadUInt32() & 0x7FFFFFFF;
                        newSubfile.uncompressedsize = reader.ReadUInt32();
                        reader.BaseStream.Position += 0x04; //flags

                        if (newSubfile.filesize == newSubfile.uncompressedsize)
                            newSubfile.uncompressedsize = 0;
                        else
                            newSubfile.should_be_compressed_when_in_package = true;

                        subfiles.Add(newSubfile);
                    }

                    break;
                }
                case 4:
                {
                    // AS SEEN IN SPORE? I certainly hope it doesn't appear in MSK, because if it does, the extra 4 bytes I'm reading after every entry may not apply!
                    // This also appears to occur during certain MySims PC packages, so it's not just a Spore thing.
                    reader.BaseStream.Position += 4;

                    for (uint i = 0; i < filecount; i++)
                    {
                        var newSubfile = new Subfile
                        {
                            fileType = FileType.FromValue(reader.ReadUInt32()),
                            hash = (ulong)(reader.ReadUInt32()) << 32,
                        };

                        newSubfile.hash |= reader.ReadUInt32();

                        newSubfile.fileoffset = reader.ReadUInt32();
                        newSubfile.filesize = reader.ReadUInt32() & 0x7FFFFFFF;
                        newSubfile.uncompressedsize = reader.ReadUInt32();

                        if (newSubfile.filesize == newSubfile.uncompressedsize)
                        {
                            newSubfile.uncompressedsize = 0;
                        }
                        else
                        {
                            newSubfile.should_be_compressed_when_in_package = true;
                        }

                        reader.ReadUInt32(); // TODO: Determine what this is (unknown flags?)

                        subfiles.Add(newSubfile);
                    }

                    break;
                }
                case 7:
                {
                    for (uint i = 0; i < filecount; i++)
                    {
                        var newSubfile = new Subfile
                        {
                            fileType = FileType.FromValue(reader.ReadUInt32()),
                            groupID = reader.ReadUInt32(),
                            hash = (ulong)(reader.ReadUInt32()) << 32,
                        };

                        newSubfile.hash |= reader.ReadUInt32();

                        newSubfile.fileoffset = reader.ReadUInt32();
                        newSubfile.filesize = reader.ReadUInt32() & 0x7FFFFFFF;
                        newSubfile.uncompressedsize = reader.ReadUInt32();
                        reader.BaseStream.Position += 0x04; //flags

                        if (newSubfile.filesize == newSubfile.uncompressedsize)
                            newSubfile.uncompressedsize = 0;
                        else
                            newSubfile.should_be_compressed_when_in_package = true;

                        subfiles.Add(newSubfile);
                    }

                    break;
                }
                default:
                    throw new NotImplementedException($"Unknown index version: {MSKindexversion}");
            }
        }


        //extract files

        Console.WriteLine($"filecount {filecount}");

        int filesprocessed = 0;


        var luaFilenamesForDict = new List<string>();

        bool containsLua = false;

        for (int i = 0; i < filecount; i++)
        {
            Subfile subfile = subfiles[i];
            if (subfile.should_be_compressed_when_in_package)
            {
                subfile.has_been_decompressed = false; //set default state of a compressed file to compressed
            }

            {
                Vault.luaString typeIDRealString = AppState.activeVault.GetLuaStringWithHash(subfile.fileType);

                // if (typeIDRealString != null)
                //     fileExtension = $".{typeIDRealString.name}";

                byte[] newfilenameasbytes = BitConverter.GetBytes(Utility.ReverseEndianULong(subfile.hash));

                subfile.filename = "0x";

                ulong newfilenameasulong = Utility.ReverseEndianULong(subfile.hash);

                subfile.hashString = $"0x{BitConverter.ToString(newfilenameasbytes).Replace("-", "")}";

                if (AppState.activeVault.VaultHashesAndFileNames.Keys.Contains(subfile.hash))
                {
                    subfile.filename = AppState.activeVault.VaultHashesAndFileNames[subfile.hash];
                }
                else
                {
                    subfile.filename = subfile.hashString;
                }

                subfile.filename += subfile.fileType.Extension;

                /*

                    f (fileextension == ".lua") //temp



                        tring luaName = "";


                        nt currentOffset = 0x10;


                        hile (newfile[currentOffset] != 0x00)



                            urrentOffset++;




                        hile (newfile[currentOffset] != 0x2F)



                            urrentOffset--;




                        urrentOffset++;


                        hile (newfile[currentOffset] != 0x2E)



                            uaName += ((char)newfile[currentOffset]) + "";

                            urrentOffset++;




                        uaFilenamesForDict.Add("VaultHashesAndFileNames.Add("+newfilename+",\""+luaName+"\");");

                        */

                filesprocessed++;
            }
        }

        viewModel.MakeFileTree();


        if (containsLua && AppState.activeVault.luaStrings.Count > 0)
        {
            string[] luaStringsForExport = new string[AppState.activeVault.luaStrings.Count];

            for (int i = 0; i < AppState.activeVault.luaStrings.Count; i++)
            {
                luaStringsForExport[i] = BitConverter.ToString(BitConverter.GetBytes(Utility.ReverseEndian(AppState.activeVault.luaStrings[i].hash))).Replace("-", "");
                luaStringsForExport[i] += $" {AppState.activeVault.luaStrings[i].name}";
            }

            File.WriteAllLines("global.activeVault.luaStrings.lua", luaStringsForExport);
        }

        File.WriteAllLines("dict.txt", luaFilenamesForDict);
        MessageBox.Show($"Processed {filesprocessed} files (out of a total {filecount}).", "Task complete", MessageBoxButton.OK, MessageBoxImage.Information);
    }




    public void LoadSkyHeroesPackage()
    {
        MessageBoxResult result = MessageBox.Show("This operation will dump the files to the same directory as the input file. Proceed?", "Extract files?", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result != MessageBoxResult.Yes)
        {
            return;
        }

        using (var reader = new BinaryReader(File.Open(filename, FileMode.Open)))
        {

            uint sig = Utility.ReverseEndian(reader.ReadUInt32());

            if (sig != 0x030502ED && sig != 0x03051771)
            {
                Console.WriteLine("invalid signature");
                return;
            }

            uint fileversion = Utility.ReverseEndian(reader.ReadUInt32());

            reader.BaseStream.Position += 0x04;

            ushort unknown1 = Utility.ReverseEndianShort(reader.ReadUInt16());
            ushort numberoffiles = Utility.ReverseEndianShort(reader.ReadUInt16()); //maybe???

            reader.BaseStream.Position += 0x30;

            var chunklist = new Dictionary<uint, uint>(); //counts and offsets. count is how many 8 byte entries there are. offset is where the first one starts from.


            for (uint i = 0; i < 110; i++)
            {
                uint count = Utility.ReverseEndian(reader.ReadUInt32());
                uint offset = Utility.ReverseEndian(reader.ReadUInt32());

                if (offset != 0xFFFFFFFF)
                {
                    chunklist.Add(offset, count);

                }
            }


            var IDsandoffsets = new Dictionary<uint, uint>();
            var offsets = new List<uint>();

            foreach (uint offset in chunklist.Keys)
            {
                reader.BaseStream.Position = offset; //go to offset where chunk begins

                for (int i = 0; i < chunklist[offset]; i++) //for each item in the chunk
                {
                    uint fileID = Utility.ReverseEndian(reader.ReadUInt32());
                    uint fileoffset = Utility.ReverseEndian(reader.ReadUInt32());

                    IDsandoffsets.Add(fileID, fileoffset + 0x10); //and the ID and offset of the file to the dictionary
                    offsets.Add(fileoffset + 0x10); // just so that we can reference which one comes after etc.
                }
            }

            //extract files

            foreach (uint ID in IDsandoffsets.Keys)
            {
                var newfile = new List<byte>();


                if (offsets.IndexOf(IDsandoffsets[ID]) != offsets.Count - 1) //if it's not the last file
                {
                    uint numberofbytestoread = (offsets[offsets.IndexOf(IDsandoffsets[ID]) + 1] - IDsandoffsets[ID]) - 0x10;

                    reader.BaseStream.Position = IDsandoffsets[ID];

                    for (int i = 0; i < numberofbytestoread; i++) //add bytes to the file
                    {
                        newfile.Add(reader.ReadByte());
                    }

                    byte[] filetype = new byte[2];

                    byte[] IDasbytearray = BitConverter.GetBytes(ID);

                    Array.Reverse(IDasbytearray, 0, IDasbytearray.Length);

                    Array.Copy(IDasbytearray, 0, filetype, 0, 2);


                    switch (filetype[0])
                    {
                        case 0x22:
                            newfile = imageTools.ConvertToNintendoTPL(filename + ID, newfile.ToArray());
                            File.WriteAllBytes($"{filename}{ID}.tpl", newfile.ToArray());
                            break;
                        case 0x28:
                            File.WriteAllBytes($"{filename}{ID}.proxy", newfile.ToArray());
                            break;
                        case 0x29:
                            File.WriteAllBytes($"{filename}{ID}.animdata", newfile.ToArray());
                            break;
                        case 0x45:
                            File.WriteAllBytes($"{filename}{ID}.fx", newfile.ToArray());
                            break;
                        case 0x5A:
                            File.WriteAllBytes($"{filename}{ID}.snddata", newfile.ToArray());
                            break;
                        case 0x5B:
                            //newfile = ConvertSkyHeroesModel(file + ID.ToString(), newfile.ToArray());
                            File.WriteAllBytes($"{filename}{ID}.mdl", newfile.ToArray());
                            break;
                        case 0x63:
                            File.WriteAllBytes($"{filename}{ID}.godinfo", newfile.ToArray());
                            break;
                        default:
                            File.WriteAllBytes($"{filename}{ID}_{filetype}", newfile.ToArray());
                            break;
                    }
                }
            }

            //0x2246  = texture type?
            //0x2846  = proxies
            //0x2946  = animation data/pointers?
            //0x4546  = effect file
            //0x5A46  = sound effect data/pointers
            //0x5B46  = model???
            //0x6346  = godinfo
        }

        MessageBox.Show("Files extracted. They are in the same folder as the original archive.", "Extraction complete", MessageBoxButton.OK, MessageBoxImage.Information);
    }


    public Subfile FindFileByHashAndTypeID(ulong hash, uint typeID)
    {

        foreach (Subfile s in AppState.activePackage.subfiles)
        {
            if (s.hash == hash && s.fileType == typeID)
            {
                return s;
            }
        }

        Console.WriteLine($"File with hash {hash} and type ID {typeID} not found...");
        return null;
    }



    public uint ReverseEndianIfNeeded(uint input)
    {
        if (packageType == PackageType.Agents)
        {
            input = Utility.ReverseEndian(input);
        }

        return input;
    }


    public void RebuildPackage()
    {
        var output = new List<byte>();

        uint packageVersion = 0;

        switch (packageType)
        {
            case PackageType.Kingdom:
                output.Add((byte)'D');
                output.Add((byte)'B');
                output.Add((byte)'P');
                output.Add((byte)'F');
                packageVersion = 2;
                break;
            case PackageType.Agents:
                output.Add((byte)'F');
                output.Add((byte)'P');
                output.Add((byte)'B');
                output.Add((byte)'D');
                packageVersion = 3;
                break;
        }

        //offset 0x04

        Utility.AddUIntToList(output, ReverseEndianIfNeeded(packageVersion));

        //offset 0x08

        for (int i = 0; i < 0x10; i++)
        {
            output.Add(0x00);
        }

        //offset 0x18

        switch (packageType)
        {
            case PackageType.Kingdom:
            {
                for (int i = 0; i < 8; i++)
                {
                    output.Add(0x00); //pad
                }

                break;
            }
            case PackageType.Agents:
                Utility.AddLongToList(output, Utility.ReverseEndianLong(DateTime.Now.ToBinary()));
                break;
        }

        Utility.AddUIntToList(output, ReverseEndianIfNeeded(indexmajorversion));

        Utility.AddUIntToList(output, ReverseEndianIfNeeded((uint)subfiles.Count));

        //offset 0x28

        switch (packageType)
        {
            case PackageType.Kingdom:
            {
                for (int i = 0; i < 4; i++)
                {
                    output.Add(0x00); //pad
                }

                break;
            }
            case PackageType.Agents:
                Utility.AddUIntToList(output, ReverseEndianIfNeeded(indexoffset)); //this will be returned to later once we know what it is
                break;
        }

        Utility.AddUIntToList(output, ReverseEndianIfNeeded(indexsize)); //this will be returned to later once we know what it is
        Utility.AddUIntToList(output, ReverseEndianIfNeeded(holeentrycount));
        Utility.AddUIntToList(output, ReverseEndianIfNeeded(holeoffset));
        Utility.AddUIntToList(output, ReverseEndianIfNeeded(holesize));
        Utility.AddUIntToList(output, ReverseEndianIfNeeded(indexminorversion));

        if (packageType == PackageType.Agents)
        {
            Utility.AddUIntToList(output, ReverseEndianIfNeeded(unknown4));
        }

        Utility.AddUIntToList(output, ReverseEndianIfNeeded(indexoffset)); //this will be returned to later once we know what it is
        Utility.AddUIntToList(output, ReverseEndianIfNeeded(unknown5));
        Utility.AddUIntToList(output, ReverseEndianIfNeeded(unknown6));
        Utility.AddUIntToList(output, ReverseEndianIfNeeded(reserved1));
        Utility.AddUIntToList(output, ReverseEndianIfNeeded(reserved2));

        while (output.Count < 0x60)
        {
            output.Add(0x00);
        }

        //sort by type ID, and within a type ID, by hash
        subfiles = subfiles.OrderBy(s => s.fileType).ThenBy(s => s.hash).ToList();

        var indexEntriesForWriting = new List<IndexEntry>();
        var TypeIDsThatRequireCompression = new List<uint>();

        int[] subfileOffsets = new int[subfiles.Count];

        for (int f = 0; f < subfiles.Count; f++)
        {
            subfileOffsets[f] = output.Count;

            bool typeIDhasIndexEntry = false;

            foreach (IndexEntry entry in indexEntriesForWriting)
            {
                if (entry.typeID == subfiles[f].fileType)
                {
                    entry.typeNumberOfInstances++;
                    typeIDhasIndexEntry = true;
                    break;
                }
            }

            if (!typeIDhasIndexEntry)
            {
                var newIndexEntry = new IndexEntry
                {
                    typeID = subfiles[f].fileType,
                };

                if (subfiles[f].should_be_compressed_when_in_package)
                {
                    newIndexEntry.groupID = 2;
                }
                else
                {
                    newIndexEntry.groupID = 0;
                }

                newIndexEntry.indexnulls = 0;
                newIndexEntry.typeNumberOfInstances = 1;

                if (newIndexEntry.groupID != 0)
                {
                    TypeIDsThatRequireCompression.Add(newIndexEntry.typeID);
                }

                indexEntriesForWriting.Add(newIndexEntry);
            }


            if (subfiles[f].should_be_compressed_when_in_package)
            {
                if (subfiles[f].has_been_decompressed) //if it was decompressed by the user then compress it
                {
                    subfiles[f].filebytes = Compression.Compress_QFS(subfiles[f].filebytes);
                }
            }

            if (subfiles[f].filebytes == null || subfiles[f].filebytes.Length == 0) //then the file was not modified or read, so transfer it directly from the old package
            {
                for (int i = 0; i < subfiles[f].filesize; i++)
                {
                    output.Add(filebytes[subfiles[f].fileoffset + i]);
                }
            }
            else //if it was modified or read, use the bytes from its filebytes array
            {
                subfiles[f].filesize = (uint)subfiles[f].filebytes.Length;
                Console.WriteLine($"LENGTH BEING ADDED IS: {subfiles[f].filesize}");
                for (int i = 0; i < subfiles[f].filebytes.Length; i++)
                {
                    output.Add(subfiles[f].filebytes[i]);
                }
            }

            while (output.Count % 0x20 != 0)
            {
                output.Add(0x00); //pad to multiple of 0x20
            }
        }

        //that should bring us up to the start of the index table

        uint newIndexOffset = (uint)output.Count;

        if (packageType == PackageType.Agents)
        {
            Utility.AddULongToList(output, Utility.ReverseEndianULong((ulong)indexEntriesForWriting.Count));

            for (int i = 0; i < indexEntriesForWriting.Count; i++) //a bunch of entries that describe how many files there are of each type
            {
                Utility.AddUIntToList(output, Utility.ReverseEndian(indexEntriesForWriting[i].typeID));
                Utility.AddUIntToList(output, Utility.ReverseEndian(indexEntriesForWriting[i].groupID));
                Utility.AddUIntToList(output, Utility.ReverseEndian(indexEntriesForWriting[i].typeNumberOfInstances));
                Utility.AddUIntToList(output, Utility.ReverseEndian(indexEntriesForWriting[i].indexnulls));
            }

            for (int i = 0; i < subfiles.Count; i++) //go through the files and add them to the index list. They are organised by type, one type after the other. (So X number of type A, as described above, then Y number of type B...) Within a type, they are organised by hash
            {
                Utility.AddULongToList(output, Utility.ReverseEndianULong(subfiles[i].hash));
                Utility.AddIntToList(output, Utility.ReverseEndianSigned(subfileOffsets[i]));
                Utility.AddUIntToList(output, Utility.ReverseEndian(subfiles[i].filesize));
                // this line is probably only required in MSK     utility.AddUIntToList(output, utility.ReverseEndian(subfiles[i].typeID));

                // this line is probably only required in MSK     utility.AddUIntToList(output, utility.ReverseEndian(subfiles[i].groupID));

                if (TypeIDsThatRequireCompression.Contains(subfiles[i].fileType))
                {
                    Utility.AddUIntToList(output, Utility.ReverseEndian(subfiles[i].uncompressedsize));
                }
            }

            Utility.OverWriteUIntInList(output, 0x28, ReverseEndianIfNeeded(newIndexOffset));
            Utility.OverWriteUIntInList(output, 0x2C, ReverseEndianIfNeeded((uint)(output.Count - newIndexOffset)));
            Utility.OverWriteUIntInList(output, 0x44, ReverseEndianIfNeeded(newIndexOffset));
        }
        else
        {
            MessageBox.Show("only Agents works at the moment");

        }

        var saveFileDialog1 = new SaveFileDialog();
        saveFileDialog1.Title = "Save new package";
        saveFileDialog1.Filter = ".package files (*.package)|*.package";

        if (saveFileDialog1.ShowDialog() == true)
        {
            File.WriteAllBytes(saveFileDialog1.FileName, output.ToArray());
        }
    }
}

public class IndexEntry
{
    public uint typeID = 0;
    public uint groupID = 0;
    public uint typeNumberOfInstances = 0;
    public uint indexnulls = 0;
}