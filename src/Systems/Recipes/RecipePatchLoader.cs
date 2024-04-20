using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace VanillaVariants;

public class RecipePatchLoader : ModSystem
{
    public static List<RecipePatch> ReplacePatches { get; set; } = new();
    public static List<RecipePatch> CopyPatches { get; set; } = new();

    public override bool ShouldLoad(EnumAppSide forSide) => forSide.IsServer();
    public override double ExecuteOrder() => 0.06;

    public override void AssetsLoaded(ICoreAPI api)
    {
        long elapsedMilliseconds = api.World.ElapsedMilliseconds;

        ITreeAttribute worldConfig = api.World.Config ?? new TreeAttribute();
        List<IAsset> many = api.Assets.GetMany("config/recipepatches/");

        HashSet<string> loadedModIds = new HashSet<string>(api.ModLoader.Mods.Select((Mod m) => m.Info.ModID).ToList());
        foreach (IAsset asset in many)
        {

            RecipePatch[] patches = null;
            try
            {
                patches = asset.ToObject<RecipePatch[]>(null);
            }
            catch (Exception e)
            {
                api.Logger.Error($"[Vanilla Variants] Failed loading recipe patches file {asset.Location}:");
                api.Logger.Error(e);
            }

            for (int i = 0; patches != null && i < patches.Length; i++)
            {
                RecipePatch patch = patches[i];

                NewGridRecipeLoader.TotalCount++;

                if (!patch.CanApply(api, i, asset.Location, worldConfig, loadedModIds))
                {
                    NewGridRecipeLoader.UnmetConditionCount++;
                    continue;
                }

                switch (patch.Type)
                {
                    case EnumRecipePatchType.Replace:
                        ReplacePatches.Add(patch);
                        break;
                    case EnumRecipePatchType.Copy:
                    case EnumRecipePatchType.CopyReplaceIngredients:
                        CopyPatches.Add(patch);
                        break;
                }
            }
        }

        NewGridRecipeLoader.ElapsedMilliseconds += api.World.ElapsedMilliseconds - elapsedMilliseconds;
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
                    if (patch.ChangeQuantity)
                    {
                        recipe.Output.Quantity = patch.QuantityNew;
                    }
                    if (patch.RecipeGroup != null)
                    {
                        recipe.RecipeGroup = (int)patch.RecipeGroup;
                    }
                    newRecipe = null;
                    return true;
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
                        if (patch.ChangeQuantity)
                        {
                            newRecipe.Output.Quantity = patch.QuantityNew;
                        }
                        if (patch.RecipeGroup != null)
                        {
                            newRecipe.RecipeGroup = (int)patch.RecipeGroup;
                        }
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
                    if (patch.RecipeGroup != null)
                    {
                        newRecipe.RecipeGroup = (int)patch.RecipeGroup;
                    }
                    return any;
                }
        }

        newRecipe = null;
        return false;
    }
}