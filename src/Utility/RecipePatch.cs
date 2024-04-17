using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Util;
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

    public bool MatchOutputQuantity = false;
    public bool ChangeQuantity = false;
    public int Quantity = 0;
    public int QuantityNew = 0;

    public string OutputCode = null;
    public string OutputCodeNew = null;
    public IngredientPatch[] Ingredients = null;

    public int? RecipeGroup = null;

    public AssetLocation GetOutputCode() => new AssetLocation(OutputCode);
    public AssetLocation GetOutputCodeNew() => new AssetLocation(OutputCodeNew);

    public bool MatchesOutput(GridRecipe recipe)
    {
        return MatchOutputQuantity
            ? WildcardUtil.Match(GetOutputCode(), recipe.Output.Code) && recipe.Output.Quantity == Quantity
            : WildcardUtil.Match(GetOutputCode(), recipe.Output.Code);
    }

    public bool CanApply(ICoreAPI api)
    {
        bool hasConfigs = ConfigKeys == null || ConfigKeys.All(key => api.World.Config.GetBool(key));
        if (!hasConfigs) return false;

        HashSet<string> loadedModIds = api.ModLoader.Mods.Select(mod => mod.Info.ModID).ToHashSet();
        return DependsOn == null || DependsOn.All(dependency => loadedModIds.Contains(dependency.modid));
    }
}