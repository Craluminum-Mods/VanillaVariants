using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Util;
using Vintagestory.ServerMods;

namespace VanillaVariants;

public class NewRecipes : ModSystem
{
    internal static long elapsedMilliseconds = 0;
    public static int count = 0;

    public static List<GridRecipe> newRecipes = new();

    public override bool ShouldLoad(EnumAppSide forSide) => forSide.IsServer();
    public override double ExecuteOrder() => 1.01;

    public override void AssetsLoaded(ICoreAPI api)
    {
        GridRecipeLoader gridRecipeLoader = api.ModLoader.GetModSystem<GridRecipeLoader>();

        foreach (GridRecipe recipe in newRecipes)
        {
            gridRecipeLoader.LoadRecipe(new AssetLocation("skippatch"), recipe);
        }
    }

    public override void AssetsFinalize(ICoreAPI api)
    {
        api.Logger.Notification($"[{Mod.Info.Name}] RecipePatch Loader: Completed in: {elapsedMilliseconds} ms, {count} patches total");
    }
}

public class Recipes : ModSystem
{
    /// Apply when recipes are not resolved yet
    public static List<RecipePatch> prePatches = new();

    // Copy existing recipes, apply patches and add to newRecipes
    public static List<RecipePatch> postPatches = new();

    public override bool ShouldLoad(EnumAppSide forSide) => forSide.IsServer();

    public override void AssetsLoaded(ICoreAPI api)
    {
        List<IAsset> many = api.Assets.GetMany("config/recipepatches/");
        foreach (IAsset asset in many)
        {
            try
            {
                List<RecipePatch> loadedAsset = asset.ToObject<List<RecipePatch>>();
                if (loadedAsset != null)
                {
                    prePatches.AddRange(loadedAsset.Where(x => x.CanApply(api) && x.Type == EnumRecipePatchType.Original));
                    postPatches.AddRange(loadedAsset.Where(x => x.CanApply(api) && (x.Type == EnumRecipePatchType.New || x.Type == EnumRecipePatchType.NewIngredientOnly)));
                }
            }
            catch (Exception e)
            {
                api.Logger.Error($"[{Mod.Info.Name}] Failed loading patches file {asset.Location}");
                api.Logger.Error(e);
            }
        }
    }

    public static GridRecipe HandleRecipe(GridRecipe recipe, RecipePatch patch)
    {
        GridRecipe newRecipe = recipe.Clone();

        switch (patch.Type)
        {
            case EnumRecipePatchType.Original:
                foreach (CraftingRecipeIngredient ingredient in recipe.Ingredients.Where(x => WildcardUtil.Match(patch.GetIngredientCode(), x.Value.Code)).Select(x => x.Value))
                {
                    ingredient.Code = patch.GetOldCode();
                    ingredient.AllowedVariants = patch.OldAllowedVariants;
                    ingredient.SkipVariants = patch.OldSkipVariants;
                }
                return null;
            case EnumRecipePatchType.New:
                {
                    newRecipe.RecipeGroup = 1;
                    bool any = false;
                    foreach (CraftingRecipeIngredient ingredient in newRecipe.Ingredients.Where(x => WildcardUtil.Match(patch.GetIngredientCode(), x.Value.Code)).Select(x => x.Value))
                    {
                        any = true;
                        ingredient.Code = patch.GetNewCode();
                        ingredient.Name = patch.NewName;
                        ingredient.AllowedVariants = patch.NewAllowedVariants;
                        ingredient.SkipVariants = patch.NewSkipVariants;
                    }

                    if (any)
                    {
                        newRecipe.Output.Code = patch.GetNewOutputCode();
                    }
                    return newRecipe;
                }
            case EnumRecipePatchType.NewIngredientOnly:
                {
                    newRecipe.RecipeGroup = 1;
                    foreach (CraftingRecipeIngredient ingredient in newRecipe.Ingredients.Where(x => WildcardUtil.Match(patch.GetIngredientCode(), x.Value.Code)).Select(x => x.Value))
                    {
                        ingredient.Code = patch.GetNewCode();
                    }
                    return newRecipe;
                }
            default: return null;
        }
    }
}