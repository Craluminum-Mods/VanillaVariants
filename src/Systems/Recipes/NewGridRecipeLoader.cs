using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.ServerMods;

namespace VanillaVariants;

public class NewGridRecipeLoader : ModSystem
{
    internal static long ElapsedMilliseconds { get; set; } = 0;
    public static int Count { get; set; } = 0;
    public static int ReplacedCount { get; set; } = 0;
    public static int NewCount { get; set; } = 0;
    public static List<GridRecipe> NewRecipes { get; set; } = new();

    public override bool ShouldLoad(EnumAppSide forSide) => forSide.IsServer();
    public override double ExecuteOrder() => 1.01;

    public override void AssetsLoaded(ICoreAPI api)
    {
        GridRecipeLoader gridRecipeLoader = api.ModLoader.GetModSystem<GridRecipeLoader>();

        foreach (GridRecipe recipe in NewRecipes)
        {
            gridRecipeLoader.LoadRecipe(new AssetLocation("skippatch"), recipe);
            NewCount++;
        }
    }

    public override void AssetsFinalize(ICoreAPI api)
    {
        api.Logger.Notification($"[{Mod.Info.Name}] RecipePatch Loader: Completed in: {ElapsedMilliseconds} ms, {Count} patches total, replaced {ReplacedCount} recipes, {NewCount} new recipes");
    }

    public override void Dispose()
    {
        ElapsedMilliseconds = 0;
        Count = 0;
        ReplacedCount = 0;
        NewCount = 0;
        NewRecipes.Clear();
    }
}
