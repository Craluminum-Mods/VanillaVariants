using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.ServerMods;

namespace VanillaVariants;

public class HarmonyPatches : ModSystem
{
    private Harmony HarmonyInstance => new Harmony(Mod.Info.ModID);

    public override void Start(ICoreAPI api)
    {
        if (Core.Config.ExperimentalOverlayTest)
        {
            HarmonyInstance.Patch(original: typeof(ColorBlend).GetMethod(nameof(ColorBlend.Overlay)), prefix: typeof(Overlay_Patch).GetMethod(nameof(Overlay_Patch.Prefix)));
        }

        HarmonyInstance.Patch(original: typeof(GridRecipeLoader).GetMethod(nameof(GridRecipeLoader.LoadRecipe)), prefix: typeof(GridRecipeLoader_LoadRecipe_Patch).GetMethod(nameof(GridRecipeLoader_LoadRecipe_Patch.Prefix)));
    }

    public override void Dispose()
    {
        if (Core.Config.ExperimentalOverlayTest)
        {
            HarmonyInstance.Unpatch(original: typeof(ColorBlend).GetMethod(nameof(ColorBlend.Overlay)), HarmonyPatchType.All, HarmonyInstance.Id);
        }

        HarmonyInstance.Unpatch(original: typeof(GridRecipeLoader).GetMethod(nameof(GridRecipeLoader.LoadRecipe)), HarmonyPatchType.All, HarmonyInstance.Id);
    }
}