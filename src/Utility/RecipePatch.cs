using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.ServerMods.NoObf;

namespace VanillaVariants;

public class RecipePatch
{
    public string[] ConfigKeys = null;
    public PatchModDependence[] DependsOn = null;

    public bool ReplaceDefault = true;
    public bool CreateNew = true;

    public string OutputCode = null;
    public string IngredientCode = null;

    public string OldCode = null;
    public string[] OldAllowedVariants = null;

    public string NewOutputCode = null;
    public string NewCode = null;
    public string NewName = null;
    public string[] NewAllowedVariants = null;

    public AssetLocation GetIngredientCode() => new AssetLocation(IngredientCode);

    public AssetLocation GetOldCode() => string.IsNullOrEmpty(OldCode) ? GetIngredientCode() : new AssetLocation(OldCode);
    public AssetLocation GetOutputCode() => new AssetLocation(OutputCode);

    public AssetLocation GetNewCode() => string.IsNullOrEmpty(NewCode) ? GetIngredientCode() : new AssetLocation(NewCode);
    public AssetLocation GetNewOutputCode() => new AssetLocation(NewOutputCode);

    public bool CanApply(ICoreAPI api)
    {
        bool hasConfigs = ConfigKeys == null || ConfigKeys.All(key => api.World.Config.GetBool(key));
        if (!hasConfigs) return false;

        HashSet<string> loadedModIds = api.ModLoader.Mods.Select(mod => mod.Info.ModID).ToHashSet();
        return DependsOn == null || DependsOn.All(dependency => loadedModIds.Contains(dependency.modid));
    }
}