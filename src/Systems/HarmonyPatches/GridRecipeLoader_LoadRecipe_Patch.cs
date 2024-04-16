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

        foreach (RecipePatch patch in Recipes.prePatches)
        {
            if (WildcardUtil.Match(patch.GetOutputCode(), recipe.Output.Code))
            {
                Recipes.HandleRecipe(recipe, patch, out _);
                NewRecipes.count++;
            }
        }

        foreach (RecipePatch patch in Recipes.postPatches)
        {
            if (patch.Type == EnumRecipePatchType.NewIngredientOnly || WildcardUtil.Match(patch.GetOutputCode(), recipe.Output.Code))
            {
                if (Recipes.HandleRecipe(recipe, patch, out GridRecipe newRecipe) && newRecipe != null)
                {
                    NewRecipes.newRecipes.Add(newRecipe);
                    NewRecipes.count++;
                }
            }
        }

        NewRecipes.elapsedMilliseconds += api.World.ElapsedMilliseconds - elapsedMilliseconds;
        return true;
    }
}