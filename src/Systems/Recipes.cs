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
            foreach (RecipePatch patch in patches.Where(x => WildcardUtil.Match(x.GetOutputCode(), recipe.Output.Code)))
            {
                HandleRecipe(api, newRecipes, recipe, patch);
                count++;
            }

            HandleLiquidContainerRecipe(api, newRecipes, recipe);
            HandleSignRecipe(api, newRecipes, recipe);
        }

        api.Logger.Notification($"[{Mod.Info.Name}] RecipePatch Loader: {{0}} patches total", count);

        foreach (GridRecipe recipe in newRecipes)
        {
            gridRecipeLoader.LoadRecipe(null, recipe);
        }
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
                ingredient.Code = patch.GetNewCode();
                ingredient.Name = patch.NewName;
                ingredient.AllowedVariants = patch.NewAllowedVariants;
            }

            if (newIngredients?.Count != 0)
            {
                newRecipe.Output.Code = patch.GetNewOutputCode();
            }

            newRecipe.ResolveIngredients(api.World);
            newRecipes.Add(newRecipe);
        }
    }

    private static void HandleLiquidContainerRecipe(ICoreAPI api, List<GridRecipe> newRecipes, GridRecipe recipe)
    {
        GridRecipe newRecipe = recipe.Clone();
        newRecipe.RecipeGroup = 1;

        CraftingRecipeIngredient ingredient1 = newRecipe.Ingredients.FirstOrDefault(x => WildcardUtil.Match(new AssetLocation("woodbucket"), x.Value.Code)).Value;
        if (Core.Config.WoodBucket && ingredient1 != null)
        {
            ingredient1.Code = new AssetLocation("vanvar:woodbucket-*");
            newRecipe.ResolveIngredients(api.World);
            newRecipes.Add(newRecipe);
        }
    }

    private static void HandleSignRecipe(ICoreAPI api, List<GridRecipe> newRecipes, GridRecipe recipe)
    {
        GridRecipe newRecipe = recipe.Clone();
        newRecipe.RecipeGroup = 1;

        CraftingRecipeIngredient ingredient1 = newRecipe.Ingredients.FirstOrDefault(x => WildcardUtil.Match(new AssetLocation("sign-ground-north"), x.Value.Code)).Value;
        if (Core.Config.Sign && ingredient1 != null)
        {
            ingredient1.Code = new AssetLocation("vanvar:sign-*-ground-north");
            newRecipe.ResolveIngredients(api.World);
            newRecipes.Add(newRecipe);
        }
    }
}