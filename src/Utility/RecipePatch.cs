using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.ServerMods.NoObf;

namespace VanillaVariants;

public enum EnumRecipePatchType
{
    Replace,
    Copy,
    CopyReplaceIngredients
}

public class RecipePatch
{
    public EnumRecipePatchType Type;
    public string[] ConfigKeys = null;
    public PatchModDependence[] DependsOn = null;

    public string OutputCode = null;
    public string OutputCodeNew = null;
    public IngredientPatch[] Ingredients = null;

    public AssetLocation GetOutputCode() => new AssetLocation(OutputCode);
    public AssetLocation GetOutputCodeNew() => new AssetLocation(OutputCodeNew);

    public bool CanApply(ICoreAPI api)
    {
        bool hasConfigs = ConfigKeys == null || ConfigKeys.All(key => api.World.Config.GetBool(key));
        if (!hasConfigs) return false;

        HashSet<string> loadedModIds = api.ModLoader.Mods.Select(mod => mod.Info.ModID).ToHashSet();
        return DependsOn == null || DependsOn.All(dependency => loadedModIds.Contains(dependency.modid));
    }
}