using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Util;
using Vintagestory.ServerMods;

namespace VanillaVariants;

public class Recipes : ModSystem
{
    public override bool ShouldLoad(EnumAppSide forSide) => forSide.IsServer();

    public override double ExecuteOrder() => 1.01;

    public override void AssetsLoaded(ICoreAPI api)
    {
        long elapsedMilliseconds = api.World.ElapsedMilliseconds;
        GridRecipeLoader gridRecipeLoader = api.ModLoader.GetModSystem<GridRecipeLoader>();

        List<GridRecipe> newRecipes = new();

        List<IAsset> many = api.Assets.GetMany("config/recipepatches/");
        IEnumerable<RecipePatch> patches = Array.Empty<RecipePatch>();
        foreach (IAsset asset in many)
        {
            try
            {
                patches = patches.Concat(asset.ToObject<RecipePatch[]>().Where(x => x.CanApply(api)));
            }
            catch (Exception e)
            {
                api.Logger.Error($"[{Mod.Info.Name}] Failed loading patches file {{0}}:", asset.Location);
                api.Logger.Error(e);
            }
        }

        int count = 0;

        foreach (GridRecipe recipe in api.World.GridRecipes)
        {
            foreach (RecipePatch patch in patches.Where(x => x.ReplaceOnlyIngredient || WildcardUtil.Match(x.GetOutputCode(), recipe.Output.Code)))
            {
                HandleRecipe(api, newRecipes, recipe, patch);
                count++;
            }
        }

        foreach (GridRecipe recipe in newRecipes)
        {
            gridRecipeLoader.LoadRecipe(null, recipe);
        }

        api.Logger.Notification($"[{Mod.Info.Name}] RecipePatch Loader: Completed in {{0}} ms. {{1}} patches total", api.World.ElapsedMilliseconds - elapsedMilliseconds, count);
    }

    private static void HandleRecipe(ICoreAPI api, List<GridRecipe> newRecipes, GridRecipe recipe, RecipePatch patch)
    {
        GridRecipe newRecipe = recipe.Clone();

        if (patch.ReplaceDefault)
        {
            foreach (CraftingRecipeIngredient ingredient in recipe.Ingredients.Where(x => WildcardUtil.Match(patch.GetIngredientCode(), x.Value.Code)).Select(x => x.Value).ToList())
            {
                ingredient.Code = patch.GetOldCode();
                ingredient.AllowedVariants = patch.OldAllowedVariants;
            }

            recipe.ResolveIngredients(api.World);
        }
        if (patch.CreateNew)
        {
            newRecipe.RecipeGroup = 1;
            List<CraftingRecipeIngredient> newIngredients = newRecipe.Ingredients.Where(x => WildcardUtil.Match(patch.GetIngredientCode(), x.Value.Code)).Select(x => x.Value).ToList();

            foreach (CraftingRecipeIngredient ingredient in newIngredients)
            {
                if (patch.ReplaceOnlyIngredient)
                {
                    ingredient.Code = patch.GetNewCode();
                }
                else
                {
                    ingredient.Code = patch.GetNewCode();
                    ingredient.Name = patch.NewName;
                    ingredient.AllowedVariants = patch.NewAllowedVariants;
                }
            }

            if (newIngredients?.Count != 0 && !patch.ReplaceOnlyIngredient)
            {
                newRecipe.Output.Code = patch.GetNewOutputCode();
            }

            newRecipe.ResolveIngredients(api.World);
            newRecipes.Add(newRecipe);
        }
    }
}