using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;
using Vintagestory.GameContent.Mechanics;
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
        if (Core.Config.FixHenboxBugs)
        {
            HarmonyInstance.Patch(original: typeof(BlockEntityHenBox).GetMethod(nameof(BlockEntityHenBox.TryAddEgg)), prefix: typeof(BlockEntityHenBox_TryAddEgg_Patch).GetMethod(nameof(BlockEntityHenBox_TryAddEgg_Patch.Prefix)));
        }
        if (Core.Config.FixMechanicalBlocksBugs)
        {
            HarmonyInstance.Patch(original: AccessTools.IndexerGetter(typeof(BEHelveHammer)), prefix: AccessTools.Method(typeof(HelveHammer_TexturePosition_Patch), nameof(HelveHammer_TexturePosition_Patch.Prefix)));
            HarmonyInstance.Patch(original: typeof(BEBehaviorMPAxle).GetMethod("getStandMesh", AccessTools.all), prefix: typeof(BEBehaviorMPAxle_getStandMesh_Patch).GetMethod(nameof(BEBehaviorMPAxle_getStandMesh_Patch.Prefix)));
            HarmonyInstance.Patch(original: typeof(BlockAxle).GetMethod(nameof(BlockAxle.IsOrientedTo)), prefix: typeof(BlockAxle_IsOrientedTo_Patch).GetMethod(nameof(BlockAxle_IsOrientedTo_Patch.Prefix)));
            HarmonyInstance.Patch(original: typeof(BlockAxle).GetMethod(nameof(BlockAxle.TryPlaceBlock)), prefix: typeof(BlockAxle_TryPlaceBlock_Patch).GetMethod(nameof(BlockAxle_TryPlaceBlock_Patch.Prefix)));
            HarmonyInstance.Patch(original: typeof(BlockToggle).GetMethod(nameof(BlockToggle.IsOrientedTo)), prefix: typeof(BlockToggle_IsOrientedTo_Patch).GetMethod(nameof(BlockToggle_IsOrientedTo_Patch.Prefix)));
            HarmonyInstance.Patch(original: typeof(BlockToggle).GetMethod(nameof(BlockToggle.TryPlaceBlock)), prefix: typeof(BlockToggle_TryPlaceBlock_Patch).GetMethod(nameof(BlockToggle_TryPlaceBlock_Patch.Prefix)));
            HarmonyInstance.Patch(original: typeof(MechNetworkRenderer).GetMethod(nameof(MechNetworkRenderer.AddDevice)), prefix: typeof(MechNetworkRenderer_AddDevice_Patch).GetMethod(nameof(MechNetworkRenderer_AddDevice_Patch.Prefix)));
        }
        HarmonyInstance.Patch(original: typeof(GridRecipeLoader).GetMethod(nameof(GridRecipeLoader.LoadRecipe)), prefix: typeof(GridRecipeLoader_LoadRecipe_Patch).GetMethod(nameof(GridRecipeLoader_LoadRecipe_Patch.Prefix)));
    }

    public override void Dispose()
    {
        if (Core.Config.ExperimentalOverlayTest)
        {
            HarmonyInstance.Unpatch(original: typeof(ColorBlend).GetMethod(nameof(ColorBlend.Overlay)), HarmonyPatchType.All, HarmonyInstance.Id);
        }
        if (Core.Config.FixHenboxBugs)
        {
            HarmonyInstance.Unpatch(original: typeof(BlockEntityHenBox).GetMethod(nameof(BlockEntityHenBox.TryAddEgg)), HarmonyPatchType.All, HarmonyInstance.Id);
        }
        if (Core.Config.FixMechanicalBlocksBugs)
        {
            HarmonyInstance.Unpatch(original: AccessTools.IndexerGetter(typeof(BEHelveHammer)), HarmonyPatchType.All, HarmonyInstance.Id);
            HarmonyInstance.Unpatch(original: typeof(BEBehaviorMPAxle).GetMethod("getStandMesh", AccessTools.all), HarmonyPatchType.All, HarmonyInstance.Id);
            HarmonyInstance.Unpatch(original: typeof(BlockAxle).GetMethod(nameof(BlockAxle.IsOrientedTo)), HarmonyPatchType.All, HarmonyInstance.Id);
            HarmonyInstance.Unpatch(original: typeof(BlockAxle).GetMethod(nameof(BlockAxle.TryPlaceBlock)), HarmonyPatchType.All, HarmonyInstance.Id);
            HarmonyInstance.Unpatch(original: typeof(BlockToggle).GetMethod(nameof(BlockToggle.IsOrientedTo)), HarmonyPatchType.All, HarmonyInstance.Id);
            HarmonyInstance.Unpatch(original: typeof(BlockToggle).GetMethod(nameof(BlockToggle.TryPlaceBlock)), HarmonyPatchType.All, HarmonyInstance.Id);
            HarmonyInstance.Unpatch(original: typeof(MechNetworkRenderer).GetMethod(nameof(MechNetworkRenderer.AddDevice)), HarmonyPatchType.All, HarmonyInstance.Id);
        }
        HarmonyInstance.Unpatch(original: typeof(GridRecipeLoader).GetMethod(nameof(GridRecipeLoader.LoadRecipe)), HarmonyPatchType.All, HarmonyInstance.Id);
    }
}