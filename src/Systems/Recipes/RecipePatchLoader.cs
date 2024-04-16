using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Util;

namespace VanillaVariants;

public class RecipePatchLoader : ModSystem
{
    /// <summary>
    /// Apply when recipes are not resolved yet
    /// </summary>
    public static List<RecipePatch> prePatches = new();

    /// <summary>
    /// Copy existing recipes, apply patches and add to newRecipes
    /// </summary>
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

    public static bool HandleRecipe(GridRecipe recipe, RecipePatch patch, out GridRecipe newRecipe)
    {
        newRecipe = recipe.Clone();

        switch (patch.Type)
        {
            case EnumRecipePatchType.Original:
                foreach (CraftingRecipeIngredient ingredient in recipe.Ingredients.Where(x => WildcardUtil.Match(patch.GetIngredientCode(), x.Value.Code)).Select(x => x.Value))
                {
                    ingredient.Code = patch.GetOldCode();
                    ingredient.AllowedVariants = patch.OldAllowedVariants;
                    ingredient.SkipVariants = patch.OldSkipVariants;
                }
                newRecipe = null;
                return false;
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
                    return true;
                }
            case EnumRecipePatchType.NewIngredientOnly:
                {
                    newRecipe.RecipeGroup = 1;
                    bool any = false;
                    foreach (CraftingRecipeIngredient ingredient in newRecipe.Ingredients.Where(x => WildcardUtil.Match(patch.GetIngredientCode(), x.Value.Code)).Select(x => x.Value))
                    {
                        any = true;
                        ingredient.Code = patch.GetNewCode();
                    }
                    return any;
                }
        }

        newRecipe = null;
        return false;
    }
}