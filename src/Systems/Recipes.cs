using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Util;
using Vintagestory.ServerMods;
using static VanillaVariants.Core;

namespace VanillaVariants;

public class Recipes : ModSystem
{
    public GridRecipeLoader GridRecipeLoader { get; set; }
    public static AssetLocation PlankCodes { get; } = new("plank-*");
    public static AssetLocation CobblestoneCodes { get; } = new("cobblestone-*");
    public static AssetLocation LogCodes { get; } = new("log-*");
    public static AssetLocation ClothCodes { get; } = new("cloth-*");

    public override bool ShouldLoad(EnumAppSide forSide)
    {
        return forSide == EnumAppSide.Server;
    }

    public override double ExecuteOrder()
    {
        return 1.01;
    }

    public override void AssetsLoaded(ICoreAPI api)
    {
        if (api.Side != EnumAppSide.Server || !Config.OverrideVanillaRecipes)
        {
            return;
        }

        GridRecipeLoader = api.ModLoader.GetModSystem<GridRecipeLoader>();

        List<GridRecipe> newRecipes = new();

        Item armorStand = api.World.GetItem(new AssetLocation("armorstand"));
        Block bed = api.World.GetBlock(new AssetLocation("bed-wood-head-north"));
        Block displayCase = api.World.GetBlock(new AssetLocation("displaycase-generic"));
        Block forge = api.World.GetBlock(new AssetLocation("forge"));
        Block henbox = api.World.GetBlock(new AssetLocation("henbox-empty"));
        Block ladder = api.World.GetBlock(new AssetLocation("ladder-wood-north"));
        Block moldrack = api.World.GetBlock(new AssetLocation("moldrack-normal-east"));
        Block roof = api.World.GetBlock(new AssetLocation("slantedroofing-sod-east-free"));
        Block roofCornerInner = api.World.GetBlock(new AssetLocation("slantedroofingcornerinner-sod-east-free"));
        Block roofCornerOuter = api.World.GetBlock(new AssetLocation("slantedroofingcornerouter-sod-east-free"));
        Block roofRidge = api.World.GetBlock(new AssetLocation("slantedroofingridge-sod-ns-free"));
        Block roofTip = api.World.GetBlock(new AssetLocation("slantedroofingtip-sod-free"));
        Block shelf = api.World.GetBlock(new AssetLocation("shelf-normal-east"));
        Block sign = api.World.GetBlock(new AssetLocation("sign-ground-north"));
        Block signpost = api.World.GetBlock(new AssetLocation("signpost"));
        Block tableNormal = api.World.GetBlock(new AssetLocation("table-normal"));
        Block trapdoor = api.World.GetBlock(new AssetLocation("trapdoor-closed-up-north"));
        Block troughLarge = api.World.GetBlock(new AssetLocation("trough-genericwood-large-head-north"));
        Block troughSmall = api.World.GetBlock(new AssetLocation("trough-genericwood-small-ns"));
        Block woodbucket = api.World.GetBlock(new AssetLocation("woodbucket"));

        Block woodenpan = api.World.GetBlock(new AssetLocation("pan-wooden"));
        const string chairWildcard = "game:chair-*";

        foreach (GridRecipe recipe in api.World.GridRecipes)
        {
            CollectibleObject output = recipe.Output.ResolvedItemstack?.Collectible;

            if (Config.ArmorStand && output == armorStand)
            {
                HandleRecipe(api, newRecipes, recipe, new VariantTypeWildcard("oak", "wood", PlankCodes), outputCode: new("vanvar:armorstand-{wood}"));
            }
            if (Config.Bed && output == bed)
            {
                HandleRecipe(api, newRecipes, recipe, new VariantTypeWildcard("oak", "wood", PlankCodes), outputCode: new("vanvar:bed-{wood}-head-north"));
            }
            if (Config.Chair && WildcardUtil.Match(chairWildcard, output.Code.ToString()))
            {
                HandleChairRecipe(api, newRecipes, recipe);
            }
            if (Config.DisplayCase && output == displayCase)
            {
                HandleRecipe(api, newRecipes, recipe, new VariantTypeWildcard("oak", "wood", PlankCodes), outputCode: new("vanvar:displaycase-{wood}"));
            }
            if (Config.Forge && output == forge)
            {
                HandleRecipe(api, newRecipes, recipe, new VariantTypeWildcard("granite", "stone", CobblestoneCodes), outputCode: new("vanvar:forge-{stone}"));
            }
            if (Config.Henbox && output == henbox)
            {
                HandleRecipe(api, newRecipes, recipe, new VariantTypeWildcard("oak", "wood", PlankCodes), outputCode: new("vanvar:henbox-{wood}-empty"));
            }
            if (Config.Ladder && output == ladder)
            {
                HandleRecipe(api, newRecipes, recipe, new VariantTypeWildcard("oak", "wood", PlankCodes), outputCode: new("game:ladder-{wood}-north"));
            }
            if (Config.Moldrack && output == moldrack)
            {
                HandleRecipe(api, newRecipes, recipe, new VariantTypeWildcard("oak", "wood", PlankCodes), outputCode: new("game:moldrack-{wood}-east"));
            }
            if (Config.WoodenPan && output == woodenpan)
            {
                HandleWoodenPanRecipe(api, newRecipes, recipe);
            }
            if (Config.SodRooding)
            {
                if (output == roof) { HandleRecipe(api, newRecipes, recipe, new VariantTypeWildcard("aged", "wood", PlankCodes), outputCode: new("vanvar:slantedroofing-sod-{wood}-east-free")); }
                if (output == roofRidge) { HandleRecipe(api, newRecipes, recipe, new VariantTypeWildcard("aged", "wood", PlankCodes), outputCode: new("vanvar:slantedroofingridge-sod-{wood}-ns-free")); }
                if (output == roofTip) { HandleRecipe(api, newRecipes, recipe, new VariantTypeWildcard("aged", "wood", PlankCodes), outputCode: new("vanvar:slantedroofingtip-sod-{wood}-free")); }
                if (output == roofCornerInner) { HandleRecipe(api, newRecipes, recipe, new VariantTypeWildcard("aged", "wood", PlankCodes), outputCode: new("vanvar:slantedroofingcornerinner-sod-{wood}-east-free")); }
                if (output == roofCornerOuter) { HandleRecipe(api, newRecipes, recipe, new VariantTypeWildcard("aged", "wood", PlankCodes), outputCode: new("vanvar:slantedroofingcornerouter-sod-{wood}-east-free")); }
            }
            if (Config.Shelf && output == shelf)
            {
                HandleRecipe(api, newRecipes, recipe, new VariantTypeWildcard("oak", "wood", PlankCodes), outputCode: new("game:shelf-{wood}-east"));
            }
            if (Config.Sign && output == sign)
            {
                HandleRecipe(api, newRecipes, recipe, new VariantTypeWildcard("oak", "wood", PlankCodes), outputCode: new("vanvar:sign-{wood}-ground-north"));
            }
            if (Config.Signpost && output == signpost)
            {
                HandleRecipe(api, newRecipes, recipe, new VariantTypeWildcard("oak", "wood", PlankCodes), outputCode: new("vanvar:signpost-{wood}"));
            }
            if (Config.Table && output == tableNormal)
            {
                HandleRecipe(api, newRecipes, recipe, new VariantTypeWildcard("oak", "wood", PlankCodes), new("vanvar:table-{wood}-normal"));
            }
            if (Config.Trapdoor && output == trapdoor)
            {
                HandleRecipe(api, newRecipes, recipe, new VariantTypeWildcard("oak", "wood", PlankCodes), outputCode: new("vanvar:trapdoor-{wood}-closed-up-north"));
            }
            if (Config.TroughLarge && output == troughLarge)
            {
                HandleRecipe(api, newRecipes, recipe, new VariantTypeWildcard("oak", "wood", PlankCodes), outputCode: new("game:trough-{wood}-large-head-north"));
            }
            if (Config.TroughSmall && output == troughSmall)
            {
                HandleRecipe(api, newRecipes, recipe, new VariantTypeWildcard("oak", "wood", PlankCodes), outputCode: new("game:trough-{wood}-small-ns"));
            }
            if (Config.WoodBucket && output == woodbucket)
            {
                HandleRecipe(api, newRecipes, recipe, new VariantTypeWildcard("oak", "wood", PlankCodes), outputCode: new("vanvar:woodbucket-{wood}"));
            }
        }

        foreach (GridRecipe recipe in newRecipes)
        {
            GridRecipeLoader.LoadRecipe(null, recipe);
        }
    }

    private static void HandleWoodenPanRecipe(ICoreAPI api, List<GridRecipe> newRecipes, GridRecipe recipe)
    {
        GridRecipe newRecipe = recipe.Clone();
        newRecipe.RecipeGroup = 1;

        List<CraftingRecipeIngredient> ingredients = recipe.Ingredients.Where(x => (x.Value.Code == LogCodes) || WildcardUtil.Match(LogCodes, x.Value.Code)).Select(x => x.Value).ToList();
        foreach (CraftingRecipeIngredient value in ingredients)
        {
            if (value != null)
            {
                value.Code = LogCodes;
                value.AllowedVariants = new[] { "placed-oak-ud" };
            }
        }
        _ = recipe.ResolveIngredients(api.World);

        List<CraftingRecipeIngredient> newIngredients = newRecipe.Ingredients.Where(x => (x.Value.Code == LogCodes) || WildcardUtil.Match(LogCodes, x.Value.Code)).Select(x => x.Value).ToList();
        foreach (CraftingRecipeIngredient value in newIngredients)
        {
            if (value != null)
            {
                value.Code = new AssetLocation("log-placed-*-ud");
                value.Name = "wood";
                value.AllowedVariants = null;
                value.SkipVariants = new[] { "oak" };
            }
        }

        if (newIngredients?.Count != 0)
        {
            newRecipe.Output.Code = new AssetLocation("game:pan-{wood}");
        }

        _ = newRecipe.ResolveIngredients(api.World);
        newRecipes.Add(newRecipe);
    }

    private static void HandleChairRecipe(ICoreAPI api, List<GridRecipe> newRecipes, GridRecipe recipe)
    {
        GridRecipe newRecipe = recipe.Clone();
        newRecipe.RecipeGroup = 1;

        List<CraftingRecipeIngredient> ingredients = recipe.Ingredients.Where(x => (x.Value.Code == PlankCodes) || WildcardUtil.Match(PlankCodes, x.Value.Code)).Select(x => x.Value).ToList();
        foreach (CraftingRecipeIngredient value in ingredients)
        {
            if (value != null)
            {
                value.Code = PlankCodes;
                value.AllowedVariants = new[] { "oak" };
            }
        }
        _ = recipe.ResolveIngredients(api.World);

        List<CraftingRecipeIngredient> newIngredients = newRecipe.Ingredients.Where(x => (x.Value.Code == PlankCodes) || WildcardUtil.Match(PlankCodes, x.Value.Code)).Select(x => x.Value).ToList();
        foreach (CraftingRecipeIngredient value in newIngredients)
        {
            if (value != null)
            {
                value.SkipVariants = new[] { "oak" };

                string wildcardValue = WildcardUtil.GetWildcardValue(PlankCodes, value.Code);
                if (wildcardValue != "*")
                {
                    value.Code = new AssetLocation(value.Code.ToString().Replace(wildcardValue, "*"));
                }
            }
        }

        if (newIngredients?.Count != 0)
        {
            string color = newRecipe?.Output?.ResolvedItemstack?.Collectible?.VariantStrict?["type"];
            newRecipe.Output.Code = new AssetLocation("vanvar:chair-{wood}-" + color);
        }

        _ = newRecipe.ResolveIngredients(api.World);
        newRecipes.Add(newRecipe);
    }

    private static void HandleRecipe(ICoreAPI api, List<GridRecipe> newRecipes, GridRecipe recipe, VariantTypeWildcard val, AssetLocation outputCode)
    {
        GridRecipe newRecipe = recipe.Clone();
        newRecipe.RecipeGroup = 1;

        ReplaceIngredientsWithDefault(api, recipe, val);
        ReplaceIngredientsWithNew(api, newRecipe, val, outputCode: outputCode);

        newRecipes.Add(newRecipe);
    }

    private static void ReplaceIngredientsWithDefault(ICoreAPI api, GridRecipe recipe, VariantTypeWildcard val)
    {
        foreach (CraftingRecipeIngredient value in recipe.Ingredients.Where(x => (x.Value.Code == val.Wildcard) || WildcardUtil.Match(val.Wildcard, x.Value.Code)).Select(x => x.Value).ToList())
        {
            if (value != null)
            {
                value.AllowedVariants = new[] { val.DefaultVariant };
            }
        }

        _ = recipe.ResolveIngredients(api.World);
    }

    private static void ReplaceIngredientsWithNew(ICoreAPI api, GridRecipe recipe, VariantTypeWildcard val, AssetLocation outputCode)
    {
        List<CraftingRecipeIngredient> ingredients = recipe.Ingredients.Where(x => (x.Value.Code == val.Wildcard) || WildcardUtil.Match(val.Wildcard, x.Value.Code)).Select(x => x.Value).ToList();
        foreach (CraftingRecipeIngredient value in ingredients)
        {
            if (value != null)
            {
                value.Name = val.Type;

                value.SkipVariants = new[] { val.DefaultVariant };
            }

            string wildcardValue = WildcardUtil.GetWildcardValue(val.Wildcard, value.Code);
            if (wildcardValue != "*")
            {
                value.Code = new AssetLocation(value.Code.ToString().Replace(wildcardValue, "*"));
            }
        }

        if (ingredients?.Count != 0)
        {
            recipe.Output.Code = outputCode;
        }

        _ = recipe.ResolveIngredients(api.World);
    }
}