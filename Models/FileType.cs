namespace MorcuTool.Models;

[Flags]
public enum SupportedPlatform
{
    None = 0,
    
    [Description("Microsoft Windows")]
    PC = 1 << 0,
    [Description("Nintendo Wii")]
    Wii = 1 << 1,
    // [Description("Nintendo DS")]
    // DS = 1 << 2,
    
    All = PC | Wii,// | DS,
}

[Flags]
public enum SupportedGame
{
    None = 0,
    
    [Description("MySims")]
    MS = 1 << 0,
    [Description("MySims Kingdom")]
    MSK = 1 << 1,
    [Description("MySims Party")]
    MSP = 1 << 2,
    [Description("MySims Racing")]
    MSR = 1 << 3,
    [Description("MySims Agents")]
    MSA = 1 << 4,
    [Description("MySims SkyHeroes")]
    MSH = 1 << 5,
    
    All = MS | MSK | MSP | MSR | MSA | MSH,
}

[DebuggerDisplay("{Name} ({Value:X8})")]
public sealed class FileType : SmartEnum<FileType, uint>
{
    public readonly string Extension;
    public readonly string Comment;
    public readonly string ClassName;
    public readonly SupportedPlatform Platform;
    public readonly SupportedGame Game;

    FileType(string name, uint value, string extension = "", string comment = "", string className = "", SupportedPlatform platform = SupportedPlatform.All, SupportedGame game = SupportedGame.All) : base(name, value)
    {
        Extension = extension;
        Comment = comment;
        ClassName = className;
        Platform = platform;
        Game = game;
    }
    
    public static readonly FileType RMDL_MSK = new(nameof(RMDL_MSK), 0xF9E50586, extension: ".rmdl", comment: "RevoModel", className: "ModelData", platform: SupportedPlatform.Wii, game: SupportedGame.MSK);
    public static readonly FileType RMDL_MSA = new(nameof(RMDL_MSA), 0x2954E734, extension: ".rmdl", comment: "RevoModel", className: "ModelData", platform: SupportedPlatform.Wii, game: SupportedGame.MSA);
    public static readonly FileType WMDL_MSPC = new(nameof(WMDL_MSPC), 0xB359C791, extension: ".wmdl", comment: "WindowsModel", className: "ModelData", platform: SupportedPlatform.PC, game: SupportedGame.MS);
    public static readonly FileType MATD_MSK = new(nameof(MATD_MSK), 0x01D0E75D, extension: ".matd", comment: "MaterialData", className: "MaterialData", platform: SupportedPlatform.Wii, game: SupportedGame.MSK);
    public static readonly FileType MATD_MSA = new(nameof(MATD_MSA), 0xE6640542, extension: ".matd", comment: "MaterialData", className: "MaterialData", platform: SupportedPlatform.Wii, game: SupportedGame.MSA);
    public static readonly FileType TPL_MSK = new(nameof(TPL_MSK), 0x00B2D882, extension: ".tpl", comment: "Very similar to Nintendo tpl (just has a different header)", className: "TextureData", platform: SupportedPlatform.Wii, game: SupportedGame.MSK);
    public static readonly FileType TPL_MSA = new(nameof(TPL_MSA), 0x92AA4D6A, extension: ".tpl", comment: "Altered TPL MSA", className: "TextureData", platform: SupportedPlatform.Wii, game: SupportedGame.MSA);
    public static readonly FileType MTST_MSK = new(nameof(MTST_MSK), 0x02019972, extension: ".mtst", comment: "MTST Material Set", className: "MaterialSetData", platform: SupportedPlatform.Wii, game: SupportedGame.MSK);
    public static readonly FileType MTST_MSA = new(nameof(MTST_MSA), 0x787E842A, extension: ".mtst", comment: "MTST Material Set", className: "MaterialSetData", platform: SupportedPlatform.Wii, game: SupportedGame.MSA);
    public static readonly FileType FPST_MS = new(nameof(FPST_MS), 0x2C81B60A, extension: ".fpst", comment: "Footprint set MySims - contains a model footprint (ftpt) which is possibly documented at http://simswiki.info/wiki.php?title=Sims_3:PackedFileTypes", className: "FootprintData", platform: SupportedPlatform.All, game: SupportedGame.MS);
    public static readonly FileType FPST_MSK = new(nameof(FPST_MSK), 0x8101A6EA, extension: ".fpst", comment: "FootprintData - contains a model footprint (ftpt) which is possibly documented at http://simswiki.info/wiki.php?title=Sims_3:PackedFileTypes", className: "FootprintData", platform: SupportedPlatform.Wii, game: SupportedGame.MSK);
    public static readonly FileType FPST_MSA = new(nameof(FPST_MSA), 0x0EFC1A82, extension: ".fpst", comment: "FootprintData - contains a model footprint (ftpt) which is possibly documented at http://simswiki.info/wiki.php?title=Sims_3:PackedFileTypes", className: "FootprintData", platform: SupportedPlatform.Wii, game: SupportedGame.MSA);
    public static readonly FileType BNK_MSK = new(nameof(BNK_MSK), 0xB6B5C271, extension: ".bnk", comment: "Big endian BNK - VGmstream can decode these", className: "AudioData", platform: SupportedPlatform.Wii, game: SupportedGame.MSK);
    public static readonly FileType BNK_MSA = new(nameof(BNK_MSA), 0x2199BB60, extension: ".bnk", comment: "Unknown endian (needs testing, assumed big endian) BNK - VGmstream can decode these", className: "AudioData", platform: SupportedPlatform.Wii, game: SupportedGame.MSA);
    public static readonly FileType BIG_MSK = new(nameof(BIG_MSK), 0x5BCA8C06, extension: ".big", comment: "AptData - Standard EA .BIG archive", className: "AptData", platform: SupportedPlatform.Wii, game: SupportedGame.MSK);
    public static readonly FileType BIG_MSA = new(nameof(BIG_MSA), 0x2699C28D, extension: ".big", comment: "Standard EA .BIG archive", className: "AptData", platform: SupportedPlatform.Wii, game: SupportedGame.MSA);
    public static readonly FileType COLLISION_MSA = new(nameof(COLLISION_MSA), 0x1A8FEB14, extension: ".collision", comment: "Mesh collision", className: "CollisionData", platform: SupportedPlatform.Wii, game: SupportedGame.MSK);
    public static readonly FileType FX = new(nameof(FX), 0x6B772503, extension: ".fx", comment: "FX", className: "FXData", platform: SupportedPlatform.All, game: SupportedGame.All);
    public static readonly FileType LUAC_MSA = new(nameof(LUAC_MSA), 0x3681D75B, extension: ".luac", comment: "LUAC", className: "LuaObjectData", platform: SupportedPlatform.Wii, game: SupportedGame.MSA);
    public static readonly FileType LUAC_MSK = new(nameof(LUAC_MSK), 0x2B8E2411, extension: ".luac", comment: "LuaObjectData", className: "LuaObjectData", platform: SupportedPlatform.Wii, game: SupportedGame.MSK);
    public static readonly FileType SLOT_MSK = new(nameof(SLOT_MSK), 0x487BF9E4, extension: ".slot", comment: "SlotData", className: "SlotData", platform: SupportedPlatform.Wii, game: SupportedGame.MSK);
    public static readonly FileType SLOT_MSA = new(nameof(SLOT_MSA), 0x2EF1E401, extension: ".slot", comment: "SlotData", className: "SlotData", platform: SupportedPlatform.Wii, game: SupportedGame.MSA);
    public static readonly FileType PARTICLES_MSA = new(nameof(PARTICLES_MSA), 0x28707864, extension: ".particles", comment: "Particles file", className: "ParticlesData", platform: SupportedPlatform.Wii, game: SupportedGame.MSA);
    public static readonly FileType BUILDABLEREGION_MSA = new(nameof(BUILDABLEREGION_MSA), 0x41C4A8EF, extension: ".buildableregion", comment: "Buildable region MSA", className: "BuildableRegionData", platform: SupportedPlatform.Wii, game: SupportedGame.MSA);
    public static readonly FileType BUILDABLEREGION_MSK = new(nameof(BUILDABLEREGION_MSK), 0xC84ACD30, extension: ".buildableregion", comment: "Buildable region MSK", className: "BuildableRegionData", platform: SupportedPlatform.Wii, game: SupportedGame.MSK);
    public static readonly FileType LLMF_MSK = new(nameof(LLMF_MSK), 0x58969018, extension: ".llmf", comment: "LevelData", className: "LevelData", platform: SupportedPlatform.Wii, game: SupportedGame.MSK);
    public static readonly FileType LLMF_MSA = new(nameof(LLMF_MSA), 0xA5DCD485, extension: ".llmf", comment: "LevelData", className: "LevelData", platform: SupportedPlatform.Wii, game: SupportedGame.MSA);
    public static readonly FileType RIG_MSK = new(nameof(RIG_MSK), 0x8EAF13DE, extension: ".grannyrig", comment: "Interesting granny struct info at 0x49CFDD in MSA's main.dol", className: "RigData", platform: SupportedPlatform.Wii, game: SupportedGame.MSK);
    public static readonly FileType RIG_MSA = new(nameof(RIG_MSA), 0x4672E5BD, extension: ".grannyrig", comment: "Interesting granny struct info at 0x49CFDD in MSA's main.dol", className: "RigData", platform: SupportedPlatform.Wii, game: SupportedGame.MSA);
    public static readonly FileType ANIMCLIP_MSK = new(nameof(ANIMCLIP_MSK), 0x6B20C4F3, extension: ".clip", comment: "Animation clip", className: "ClipData", platform: SupportedPlatform.Wii, game: SupportedGame.MSK);
    public static readonly FileType ANIMCLIP_MSA = new(nameof(ANIMCLIP_MSA), 0xD6BEDA43, extension: ".clip", comment: "Animation clip", className: "ClipData", platform: SupportedPlatform.Wii, game: SupportedGame.MSA);
    public static readonly FileType LTST_MSA = new(nameof(LTST_MSA), 0xE55D5715, extension: ".ltst", comment: "Possibly lighting set", className: "LightSetData", platform: SupportedPlatform.Wii, game: SupportedGame.MSA);
    public static readonly FileType TTF_MSK = new(nameof(TTF_MSK), 0x89AF85AD, extension: ".ttf", comment: "TrueType font MySims Kingdom", className: "TrueTypeFont", platform: SupportedPlatform.Wii, game: SupportedGame.MSK);
    public static readonly FileType TTF_MSA = new(nameof(TTF_MSA), 0x276CA4B9, extension: ".ttf", comment: "TrueType font MySims Agents", className: "TrueTypeFont", platform: SupportedPlatform.Wii, game: SupportedGame.MSA);
    public static readonly FileType HKX_MSK = new(nameof(HKX_MSK), 0xD5988020, extension: ".hkx", comment: "MSK HKX Havok collision file", className: "PhysicsData", platform: SupportedPlatform.Wii, game: SupportedGame.MSK);
    public static readonly FileType OGVD_MSK = new(nameof(OGVD_MSK), 0xD00DECF5, extension: ".objectGridVolumeData", comment: "ObjectGridVolumeData", className: "ObjectGridVolumeData", platform: SupportedPlatform.Wii, game: SupportedGame.MSK);
    public static readonly FileType OGVD_MSA = new(nameof(OGVD_MSA), 0x8FC0DE5A, extension: ".objectGridVolumeData", comment: "MSA ObjectGridVolumeData bounding box collision (for very simple objects)", className: "ObjectGridVolumeData", platform: SupportedPlatform.Wii, game: SupportedGame.MSA);
    public static readonly FileType SPD_MSK = new(nameof(SPD_MSK), 0xB70F1CEA, extension: ".snapPointData", comment: "SnapPointData", className: "SnapPointData", platform: SupportedPlatform.Wii, game: SupportedGame.MSK);
    public static readonly FileType SPD_MSA = new(nameof(SPD_MSA), 0x5027B4EC, extension: ".snapPointData", comment: "MSA SnapPointData, most likely", className: "SnapPointData", platform: SupportedPlatform.Wii, game: SupportedGame.MSA);
    public static readonly FileType VGD_MSK = new(nameof(VGD_MSK), 0x614ED283, extension: ".voxelGridData", comment: "VoxelGridData", className: "VoxelGridData", platform: SupportedPlatform.Wii, game: SupportedGame.MSK);
    public static readonly FileType VGD_MSA = new(nameof(VGD_MSA), 0x9614D3C0, extension: ".voxelGridData", comment: "MSA VoxelGridData, most likely", className: "VoxelGridData", platform: SupportedPlatform.Wii, game: SupportedGame.MSA);
    public static readonly FileType MODEL_MS = new(nameof(MODEL_MS), 0x01661233, extension: ".model", comment: "Model used by MySims, not the same as RMDL", className: "ModelData", platform: SupportedPlatform.All, game: SupportedGame.MS);
    public static readonly FileType KEYNAMEMAP_MS = new(nameof(KEYNAMEMAP_MS), 0x0166038C, extension: ".KeyNameMap", comment: "KeyNameMap", className: "KeyNameMap", platform: SupportedPlatform.All, game: SupportedGame.MS);
    public static readonly FileType GEOMETRY_MS = new(nameof(GEOMETRY_MS), 0x015A1849, extension: ".geometry", comment: "Geometry", className: "Geometry", platform: SupportedPlatform.All, game: SupportedGame.MS);
    public static readonly FileType OLDSPEEDTREE_MS = new(nameof(OLDSPEEDTREE_MS), 0x00B552EA, extension: ".oldSpeedTree", comment: "Old SpeedTree", className: "OldSpeedTree", platform: SupportedPlatform.All, game: SupportedGame.MS);
    public static readonly FileType SPEEDTREE_MS = new(nameof(SPEEDTREE_MS), 0x021D7E8C, extension: ".speedTree", comment: "SpeedTree", className: "SpeedTree", platform: SupportedPlatform.All, game: SupportedGame.MS);
    public static readonly FileType COMPOSITETEXTURE_MS = new(nameof(COMPOSITETEXTURE_MS), 0x8E342417, extension: ".compositeTexture", comment: "CompositeTexture", className: "CompositeTexture", platform: SupportedPlatform.All, game: SupportedGame.MS);
    public static readonly FileType SIMOUTFIT_MS = new(nameof(SIMOUTFIT_MS), 0x025ED6F4, extension: ".simOutfit", comment: "SimOutfit", className: "SimOutfit", platform: SupportedPlatform.All, game: SupportedGame.MS);
    public static readonly FileType LEVELXML_MS = new(nameof(LEVELXML_MS), 0x585EE310, extension: ".levelXml", comment: "LevelXml", className: "LevelXml", platform: SupportedPlatform.All, game: SupportedGame.MS);
    public static readonly FileType LUA_MSK = new(nameof(LUA_MSK), 0x474999B4, extension: ".lua", comment: "Uncompiled Lua script", className: "LuaTextData", platform: SupportedPlatform.Wii, game: SupportedGame.MSK);
    public static readonly FileType LIGHTSETXML_MS = new(nameof(LIGHTSETXML_MS), 0x50182640, extension: ".lightSetXml", comment: "LightSetXml", className: "LightSetXml", platform: SupportedPlatform.All, game: SupportedGame.MS);
    public static readonly FileType LIGHTSETBIN_MSK = new(nameof(LIGHTSETBIN_MSK), 0x50002128, extension: ".lightSetBin", comment: "Light set bin MySims - Named 'LightSetData' in MSK's executable", className: "LightSetData", platform: SupportedPlatform.Wii, game: SupportedGame.MSK);
    public static readonly FileType XML_MS = new(nameof(XML_MS), 0xDC37E964, extension: ".xml", comment: "XML", className: "XML", platform: SupportedPlatform.All, game: SupportedGame.MS);
    public static readonly FileType XML2_MS = new(nameof(XML2_MS), 0x6D3E3FB4, extension: ".xml2", comment: "Another XML", className: "XML", platform: SupportedPlatform.All, game: SupportedGame.MS);
    public static readonly FileType OBJECTCONSTRUCTIONXML_MS = new(nameof(OBJECTCONSTRUCTIONXML_MS), 0xC876C85E, extension: ".objectConstructionXml", comment: "Object construction XML", className: "ObjectConstructionXml", platform: SupportedPlatform.All, game: SupportedGame.MS);
    public static readonly FileType OBJECTCONSTRUCTIONBIN_MS = new(nameof(OBJECTCONSTRUCTIONBIN_MS), 0xC08EC0EE, extension: ".objectConstructionBin", comment: "Object construction bin", className: "ObjectConstructionBin", platform: SupportedPlatform.All, game: SupportedGame.MS);
    public static readonly FileType SLOTXML_MS = new(nameof(SLOTXML_MS), 0x4045D294, extension: ".slotXml", comment: "Slot XML", className: "SlotXml", platform: SupportedPlatform.All, game: SupportedGame.MS);
    public static readonly FileType SWARM_MS = new(nameof(SWARM_MS), 0x9752E396, extension: ".swm", comment: "Swarm", className: "SwarmData", platform: SupportedPlatform.All, game: SupportedGame.MS);
    public static readonly FileType SWARM_MSK = new(nameof(SWARM_MSK), 0xCF60795E, extension: ".swm", comment: "SwarmData", className: "SwarmData", platform: SupportedPlatform.Wii, game: SupportedGame.MSK);
    public static readonly FileType XMLBIN_MS = new(nameof(XMLBIN_MS), 0xE0D83029, extension: ".XmlBin", comment: "XmlBin", className: "XmlBin", platform: SupportedPlatform.All, game: SupportedGame.MS);
    public static readonly FileType CABXML_MS = new(nameof(CABXML_MS), 0xA6856948, extension: ".CABXml", comment: "CABXml", className: "CABXml", platform: SupportedPlatform.All, game: SupportedGame.MS);
    public static readonly FileType CABBIN_MS = new(nameof(CABBIN_MS), 0xC644F440, extension: ".CABBin", comment: "CABBin", className: "CABBin", platform: SupportedPlatform.All, game: SupportedGame.MS);
    public static readonly FileType LIGHTBOXXML_MS = new(nameof(LIGHTBOXXML_MS), 0xB61215E9, extension: ".lightBoxXml", comment: "LightBoxXml", className: "LightBoxXml", platform: SupportedPlatform.All, game: SupportedGame.MS);
    public static readonly FileType LIGHTBOXBIN_MS = new(nameof(LIGHTBOXBIN_MS), 0xD6215201, extension: ".lightBoxBin", comment: "LightBoxBin - Named 'LightBoxData' in MSK's executable", className: "LightBoxBin", platform: SupportedPlatform.All, game: SupportedGame.MS);
    public static readonly FileType XMB_MS = new(nameof(XMB_MS), 0x1E1E6516, extension: ".xmb", comment: "XMB", className: "XMB", platform: SupportedPlatform.All, game: SupportedGame.MS);
}