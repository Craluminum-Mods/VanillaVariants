using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.ServerMods;

namespace VanillaVariants;

public class NewGridRecipeLoader : ModSystem
{
    public static List<GridRecipe> NewRecipes { get; set; } = new();

    internal static long ElapsedMilliseconds { get; set; } = 0;

    public static int TotalCount { get; set; } = 0;
    public static int ReplacedCount { get; set; } = 0;
    public static int NewCount { get; set; } = 0;
    public static int UnmetConditionCount { get; set; } = 0;

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
        api.Logger.Notification($" replaced {ReplacedCount} recipes, {NewCount} new recipes");

        StringBuilder sb = new();
        sb.Append("[Vanilla Variants] RecipePatch Loader: ");

        if (TotalCount == 0)
        {
            sb.Append(string.Format("[Vanilla Variants] Nothing to patch"));
        }
        else
        {
            sb.Append($"completed in {ElapsedMilliseconds} ms");
            sb.Append($", {TotalCount} patches total");
            if (ReplacedCount > 0)
            {
                sb.Append($", successfully replaced {ReplacedCount} recipes");
            }
            if (NewCount > 0)
            {
                sb.Append($", successfully created {NewCount} recipes");
            }
            if (UnmetConditionCount > 0)
            {
                sb.Append($", unmet conditions on {UnmetConditionCount} patches");
            }
        }
        api.Logger.Notification(sb.ToString());
        api.Logger.VerboseDebug("[Vanilla Variants] Patchloader finished");
    }

    public override void Dispose()
    {
        NewRecipes.Clear();

        ElapsedMilliseconds = 0;

        TotalCount = 0;
        ReplacedCount = 0;
        NewCount = 0;
        UnmetConditionCount = 0;
    }
}
