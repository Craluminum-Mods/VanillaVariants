using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.ServerMods;

namespace VanillaVariants;

public class NewGridRecipeLoader : ModSystem
{
    internal static long elapsedMilliseconds = 0;
    public static int count = 0;
    public static int newCount = 0;

    public static List<GridRecipe> newRecipes = new();

    public override bool ShouldLoad(EnumAppSide forSide) => forSide.IsServer();
    public override double ExecuteOrder() => 1.01;

    public override void AssetsLoaded(ICoreAPI api)
    {
        GridRecipeLoader gridRecipeLoader = api.ModLoader.GetModSystem<GridRecipeLoader>();

        foreach (GridRecipe recipe in newRecipes)
        {
            gridRecipeLoader.LoadRecipe(new AssetLocation("skippatch"), recipe);
            newCount++;
        }
    }

    public override void AssetsFinalize(ICoreAPI api)
    {
        api.Logger.Notification($"[{Mod.Info.Name}] RecipePatch Loader: Completed in: {elapsedMilliseconds} ms, {count} patches total, {newCount} new recipes");
    }
}
