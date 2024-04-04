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

        Dictionary<string, List<RecipePatch>> handledRecipes = api.Assets.Get(new AssetLocation("vanvar:config/handledrecipes.json")).ToObject<Dictionary<string, List<RecipePatch>>>();

        foreach (GridRecipe recipe in api.World.GridRecipes)
        {
            foreach ((string configKey, List<RecipePatch> list) in handledRecipes.Where(x => api.World.Config.GetBool(x.Key)))
            {
                foreach (RecipePatch patch in list.Where(x => WildcardUtil.Match(x.GetOutputCode(), recipe.Output.Code)))
                {
                    HandleRecipe(api, newRecipes, recipe, patch);
                }
            }

            HandleLiquidContainerRecipe(api, newRecipes, recipe);
            HandleSignRecipe(api, newRecipes, recipe);
        }

        foreach (GridRecipe recipe in newRecipes)
        {
            gridRecipeLoader.LoadRecipe(null, recipe);
        }
    }

    private static void HandleRecipe(ICoreAPI api, List<GridRecipe> newRecipes, GridRecipe recipe, RecipePatch patch)
    {
        GridRecipe newRecipe = recipe.Clone();
        newRecipe.RecipeGroup = 1;

        if (!patch.NotReplaceDefault)
        {
            foreach (CraftingRecipeIngredient ingredient in recipe.Ingredients.Where(x => WildcardUtil.Match(patch.GetIngredientCode(), x.Value.Code)).Select(x => x.Value).ToList().Where(x => x != null))
            {
                ingredient.Code = patch.GetOldCode();
                ingredient.AllowedVariants = patch.OldAllowedVariants;
            }

            recipe.ResolveIngredients(api.World);
        }

        List<CraftingRecipeIngredient> newIngredients = newRecipe.Ingredients.Where(x => WildcardUtil.Match(patch.GetIngredientCode(), x.Value.Code)).Select(x => x.Value).ToList();
        foreach (CraftingRecipeIngredient ingredient in newIngredients.Where(x => x != null))
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