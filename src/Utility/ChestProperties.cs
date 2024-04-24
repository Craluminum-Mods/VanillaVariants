using Vintagestory.API.Common;

namespace VanillaVariants;

public class ChestProperties
{
    public string Code = "{wood}";
    public string LoadFromProperties;
    public string[] States;
    public string[] SkipVariants;
    // public string PatchesFromFile;

    public AssetLocation GetLoadFromProperties() => AssetLocation.Create(LoadFromProperties);
}