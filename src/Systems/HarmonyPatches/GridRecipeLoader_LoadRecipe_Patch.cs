using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.ServerMods;

namespace VanillaVariants;

public static class GridRecipeLoader_LoadRecipe_Patch
{
    public static bool Prefix(GridRecipeLoader __instance, AssetLocation loc, GridRecipe recipe)
    {
        if (loc == new AssetLocation("skippatch"))
        {
            return true;
        }

        if (!recipe.Enabled)
        {
            return true;
        }

        ICoreServerAPI api = __instance.GetField<ICoreServerAPI>("api");
        long elapsedMilliseconds = api.World.ElapsedMilliseconds;

        foreach (RecipePatch patch in RecipePatchLoader.prePatches)
        {
            if (WildcardUtil.Match(patch.GetOutputCode(), recipe.Output.Code))
            {
                RecipePatchLoader.HandleRecipe(recipe, patch, out _);
                NewGridRecipeLoader.count++;
            }
        }

        foreach (RecipePatch patch in RecipePatchLoader.postPatches)
        {
            if (patch.Type == EnumRecipePatchType.NewIngredientOnly || WildcardUtil.Match(patch.GetOutputCode(), recipe.Output.Code))
            {
                if (RecipePatchLoader.HandleRecipe(recipe, patch, out GridRecipe newRecipe) && newRecipe != null)
                {
                    NewGridRecipeLoader.newRecipes.Add(newRecipe);
                    NewGridRecipeLoader.count++;
                }
            }
        }

        NewGridRecipeLoader.elapsedMilliseconds += api.World.ElapsedMilliseconds - elapsedMilliseconds;
        return true;
    }
}