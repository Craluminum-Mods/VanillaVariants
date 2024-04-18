using Vintagestory.API.Common;
using Vintagestory.API.Server;
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

        foreach (RecipePatch patch in RecipePatchLoader.ReplacePatches)
        {
            if (patch.MatchesOutput(recipe))
            {
                RecipePatchLoader.HandleRecipe(recipe, patch, out _);
                NewGridRecipeLoader.ReplacedCount++;
                NewGridRecipeLoader.Count++;
            }
        }

        foreach (RecipePatch patch in RecipePatchLoader.CopyPatches)
        {
            if (patch.Type == EnumRecipePatchType.CopyReplaceIngredients || patch.MatchesOutput(recipe))
            {
                if (RecipePatchLoader.HandleRecipe(recipe, patch, out GridRecipe newRecipe) && newRecipe != null)
                {
                    NewGridRecipeLoader.NewRecipes.Add(newRecipe);
                    NewGridRecipeLoader.Count++;
                }
            }
        }

        NewGridRecipeLoader.ElapsedMilliseconds += api.World.ElapsedMilliseconds - elapsedMilliseconds;
        return true;
    }
}