using System;
using Vintagestory.API.Common;

namespace VanillaVariants;

public class ChestProperties
{
    public string Code = "{wood}";
    public string LoadFromProperties = null;
    public string[] States = Array.Empty<string>();
    public string[] SkipVariants = Array.Empty<string>();
    // public string PatchesFromFile;

    public AssetLocation GetLoadFromProperties() => AssetLocation.Create(LoadFromProperties);
}