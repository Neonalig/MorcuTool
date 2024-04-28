﻿using MorcuTool.Models;

namespace MorcuTool;

public class Subfile
{
    public byte[] filebytes = Array.Empty<byte>(); //will only contain bytes if the file has been exported or modified by the user.

    public ulong hash;
    public uint fileoffset;
    public uint filesize;
    public string filename;

    public string hashString;

    public bool has_been_decompressed = true;
    public bool should_be_compressed_when_in_package = false;

    public uint uncompressedsize; //only used by compressed files

    public FileType fileType;
    public uint groupID;

    public hkxFile hkx; //if needed
    public MsaCollision msaCol; //if needed
    public LLMF llmf; //if needed
    public RevoModel rmdl; //if needed
    public MaterialSet mtst; //if needed
    public MaterialData matd; //if needed
    public TPLtexture tpl; //if needed
    public MsaAnimation msaAnim; //if needed
    public WindowsModel wmdl; //if needed

    public TreeViewItem treeNode;

    public void Load()
    {
        using var reader = new BinaryReader(File.Open(AppState.activePackage.filename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite));
        reader.BaseStream.Position = fileoffset;

        filebytes = reader.ReadBytes((int)filesize);

        if (uncompressedsize > 0) //if it's a compressed file
        {
            filebytes = Compression.Decompress_QFS(filebytes);
            has_been_decompressed = true;
        }


        switch (fileType.Name)
        {
            case nameof(FileType.HKX_MSK): //MySims Kingdom HKX
                try
                {
                    hkx = new hkxFile(this);
                }
                catch (Exception e)
                {
                    hkx = null;
                }

                break;
            case nameof(FileType.COLLISION_MSA): //MySims Agents mesh collision
                msaCol = new MsaCollision(this);
                File.WriteAllLines($"{filename}.obj", msaCol.obj);
                break;
            case nameof(FileType.LLMF_MSK): //LLMF level bin MSK   "LevelData"
            case nameof(FileType.LLMF_MSA): //LLMF level bin MSA
                llmf = new LLMF(this);
                llmf.GenerateReport();
                break;
            case nameof(FileType.RMDL_MSK): //RMDL MSK        case global.TypeID.RMDL_MSA:          //RMDL MSA          rmdl = new RevoModel(this);
                break;
            case nameof(FileType.WMDL_MSPC): //WMDL MSPC
                wmdl = new WindowsModel(this);
                break;
            case nameof(FileType.MTST_MSK):
            case nameof(FileType.MTST_MSA):
                mtst = new MaterialSet(this);
                break;
            case nameof(FileType.MATD_MSK): //MATD MSK            "MaterialData"
            case nameof(FileType.MATD_MSA): //MATD MSA         matd = new MaterialData(this);
                break;
            case nameof(FileType.TPL_MSK):
            case nameof(FileType.TPL_MSA):
                tpl = new TPLtexture(this);
                break;
            case nameof(FileType.ANIMCLIP_MSA):
                msaAnim = new MsaAnimation(this);
                break;
        }
    }

    public void Unload()
    {
        filebytes = Array.Empty<byte>();
    }

    public void ExportFile(bool silent, string silentPath)
    {
        if (filebytes == null || filebytes.Length == 0)
        {
            Load();
        }

        var saveFileDialog1 = new SaveFileDialog();

        if (!silent)
        {
            saveFileDialog1.FileName = Path.GetFileName(filename);

            saveFileDialog1.Title = $"Export {Path.GetFileName(filename)}";
            saveFileDialog1.CheckPathExists = true;

            if (rmdl != null)
            {
                saveFileDialog1.Filter = "Wavefront OBJ (*.obj)|*.obj|MySims RevoModel (*.rmdl)|*.rmdl";
                saveFileDialog1.FileName = saveFileDialog1.FileName.Replace(".rmdl", ".obj");
            }
            else if (wmdl != null)
            {
                saveFileDialog1.Filter = "Wavefront OBJ (*.obj)|*.obj|MySims Windows Model (*.wmdl)|*.wmdl";
                saveFileDialog1.FileName = saveFileDialog1.FileName.Replace(".wmdl", ".obj");
            }
            else if (tpl != null)
            {
                if (tpl.images.Count == 0)
                {
                    saveFileDialog1.Filter = "DDS image (*.DDS)|*.dds";
                    saveFileDialog1.FileName = saveFileDialog1.FileName.Replace(".tpl", ".dds");
                }
                else
                {
                    saveFileDialog1.Filter = "PNG image (*.PNG)|*.png|TPL image (*.tpl)|*.tpl";
                    saveFileDialog1.FileName = saveFileDialog1.FileName.Replace(".tpl", ".png");
                }
            }
            else if (fileType == FileType.LUAC_MSK || fileType == FileType.LUAC_MSA)
            {
                saveFileDialog1.Filter = "Decompiled lua script (*.lua)|*.lua|Compiled lua script (*.luac)|*.luac";
                saveFileDialog1.FileName = saveFileDialog1.FileName.Replace(".luac", ".lua");
            }
            else
            {
                saveFileDialog1.Filter = $"{fileType.Extension.ToUpper()} file (*{fileType.Extension.ToLower()})|*{fileType.Extension.ToLower()}|All files (*.*)|*.*";
            }

            if (saveFileDialog1.ShowDialog() == true)
            {
                silentPath = saveFileDialog1.FileName;
            }
        }
        else
        {
            if (tpl != null)
            {
                silentPath = silentPath.Replace(".tpl", ".png");
            }
            else if (fileType == FileType.LUAC_MSK || fileType == FileType.LUAC_MSA)
            {
                //these MSK hashes are some of the ones that crash the decompiler
                if (hash != 0x13A985F3E3E05FD1 && hash != 0x1DCDDD2C8D672B03 && hash != 0x225C6F76F6DFE215 || hash != 0x2B19149AE76336EA || hash != 0x13A985F3E3E05FD1)
                {
                    silentPath = silentPath.Replace(".luac", ".lua");
                }
            }
        }

        if (!string.IsNullOrEmpty(silentPath))
        {
            switch (Path.GetExtension(silentPath))
            {
                case ".obj" when rmdl != null:
                    rmdl.GenerateObj(silentPath);
                    break;
                case ".obj":
                {
                    if (wmdl != null)
                    {
                        wmdl.GenerateObj(silentPath);
                    }

                    break;
                }
                case ".png" when (tpl != null):
                {
                    switch (tpl.images.Count)
                    {
                        case > 1:
                        {
                            for (int i = 0; i < tpl.images.Count; i++)
                            {
                                tpl.images[i].Save(silentPath.Replace(".png", $"_{i}.png"));
                            }

                            break;
                        }
                        case > 0:
                            tpl.images[0].Save(silentPath);
                            break;
                        default:
                            File.WriteAllBytes(silentPath.Replace(".tpl", ".dds").Replace(".png", ".dds"), filebytes);
                            break;
                    }

                    break;
                }
                default:
                {
                    if (tpl != null)
                    {
                        File.WriteAllBytes(silentPath, imageTools.ConvertToNintendoTPL(filename, filebytes).ToArray());
                    }
                    else if ((fileType == FileType.LUAC_MSK || fileType == FileType.LUAC_MSA) && Path.GetExtension(silentPath) == ".lua")
                    {
                        Console.WriteLine($"Trying to decompile {filename}");
                        DecompileLuc(filebytes, silentPath);
                    }
                    else
                    {
                        File.WriteAllBytes(silentPath, filebytes);
                    }

                    break;
                }
            }

            if (AppState.activePackage.date.Year > 1)
            {
                File.SetLastWriteTime(silentPath, AppState.activePackage.date);
            }
        }
    }

    public string DecompileLuc(byte[] input, string destfile)
    {
        Stream stream = new MemoryStream(input);

        var header = new BHeader(stream);

        LFunction lmain = header.Function.Parse(stream, header);

        var d = new Decompiler(lmain);
        d.Decompile();

        using var writer = new StreamWriter(destfile, false, new UTF8Encoding(false));
        d.Print(new Output(writer));

        writer.Flush();

        return (null);
    }
}