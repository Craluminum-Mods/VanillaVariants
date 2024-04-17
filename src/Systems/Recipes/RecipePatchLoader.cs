using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Util;

namespace VanillaVariants;

public class RecipePatchLoader : ModSystem
{
    public static List<RecipePatch> ReplacePatches { get; set; } = new();
    public static List<RecipePatch> CopyPatches { get; set; } = new();

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
                    ReplacePatches.AddRange(loadedAsset.Where(x => x.CanApply(api) && x.Type == EnumRecipePatchType.Replace));
                    CopyPatches.AddRange(loadedAsset.Where(x => x.CanApply(api) && (x.Type == EnumRecipePatchType.Copy || x.Type == EnumRecipePatchType.CopyReplaceIngredients)));
                }
            }
            catch (Exception e)
            {
                api.Logger.Error($"[{Mod.Info.Name}] Failed loading patches file {asset.Location}");
                api.Logger.Error(e);
            }
        }
    }

    public override void Dispose()
    {
        ReplacePatches.Clear();
        CopyPatches.Clear();
    }

    public static bool HandleRecipe(GridRecipe recipe, RecipePatch patch, out GridRecipe newRecipe)
    {
        newRecipe = recipe.Clone();

        switch (patch.Type)
        {
            case EnumRecipePatchType.Replace:
                {
                    foreach (IngredientPatch ingredPatch in patch.Ingredients)
                    {
                        foreach (CraftingRecipeIngredient ingredient in recipe.Ingredients.Where(x => WildcardUtil.Match(ingredPatch.GetIngredientCode(), x.Value.Code)).Select(x => x.Value))
                        {
                            ingredient.Code = ingredPatch.GetCode();
                            ingredient.AllowedVariants = ingredPatch.AllowedVariants;
                            ingredient.SkipVariants = ingredPatch.SkipVariants;
                        }
                    }
                    newRecipe = null;
                    return false;
                }
            case EnumRecipePatchType.Copy:
                {
                    newRecipe.RecipeGroup = recipe.RecipeGroup;
                    bool any = false;
                    foreach (IngredientPatch ingredPatch in patch.Ingredients)
                    {
                        foreach (CraftingRecipeIngredient ingredient in newRecipe.Ingredients.Where(x => WildcardUtil.Match(ingredPatch.GetIngredientCode(), x.Value.Code)).Select(x => x.Value))
                        {
                            any = true;
                            ingredient.Code = ingredPatch.GetCode();
                            ingredient.Name = ingredPatch.Name;
                            ingredient.AllowedVariants = ingredPatch.AllowedVariants;
                            ingredient.SkipVariants = ingredPatch.SkipVariants;
                        }
                    }

                    if (any)
                    {
                        newRecipe.Output.Code = patch.GetOutputCodeNew();
                    }
                    return any;
                }
            case EnumRecipePatchType.CopyReplaceIngredients:
                {
                    newRecipe.RecipeGroup = recipe.RecipeGroup;
                    bool any = false;
                    foreach (IngredientPatch ingredPatch in patch.Ingredients)
                    {
                        foreach (CraftingRecipeIngredient ingredient in newRecipe.Ingredients.Where(x => WildcardUtil.Match(ingredPatch.GetIngredientCode(), x.Value.Code)).Select(x => x.Value))
                        {
                            any = true;
                            ingredient.Code = ingredPatch.GetCode();
                        }
                    }
                    return any;
                }
        }

        newRecipe = null;
        return false;
    }
}