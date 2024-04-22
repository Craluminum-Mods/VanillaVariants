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
        HarmonyInstance.Patch(original: typeof(GridRecipeLoader).GetMethod(nameof(GridRecipeLoader.LoadRecipe)), prefix: typeof(GridRecipeLoader_LoadRecipe_Patch).GetMethod(nameof(GridRecipeLoader_LoadRecipe_Patch.Prefix)));

        if (Core.Config.ExperimentalOverlayTest)
        {
            HarmonyInstance.Patch(original: typeof(ColorBlend).GetMethod(nameof(ColorBlend.Overlay)), prefix: typeof(Overlay_Patch).GetMethod(nameof(Overlay_Patch.Prefix)));
        }
        if (Core.Config.ResolveHenboxImposter)
        {
            HarmonyInstance.Patch(original: typeof(BlockEntityHenBox).GetMethod(nameof(BlockEntityHenBox.TryAddEgg)), prefix: typeof(BlockEntityHenBox_TryAddEgg_Patch).GetMethod(nameof(BlockEntityHenBox_TryAddEgg_Patch.Prefix)));
        }
        if (Core.Config.ResolveMechanicalBlockIssues)
        {
            HarmonyInstance.Patch(original: AccessTools.IndexerGetter(typeof(BEHelveHammer)), prefix: AccessTools.Method(typeof(BEHelveHammer_TexturePosition_Patch), nameof(BEHelveHammer_TexturePosition_Patch.Prefix)));
            HarmonyInstance.Patch(original: typeof(BEBehaviorMPAxle).GetMethod("getStandMesh", AccessTools.all), prefix: typeof(BEBehaviorMPAxle_getStandMesh_Patch).GetMethod(nameof(BEBehaviorMPAxle_getStandMesh_Patch.Prefix)));
            HarmonyInstance.Patch(original: typeof(BlockAxle).GetMethod(nameof(BlockAxle.IsOrientedTo)), prefix: typeof(BlockAxle_IsOrientedTo_Patch).GetMethod(nameof(BlockAxle_IsOrientedTo_Patch.Prefix)));
            HarmonyInstance.Patch(original: typeof(BlockAxle).GetMethod(nameof(BlockAxle.TryPlaceBlock)), prefix: typeof(BlockAxle_TryPlaceBlock_Patch).GetMethod(nameof(BlockAxle_TryPlaceBlock_Patch.Prefix)));
            HarmonyInstance.Patch(original: typeof(BlockToggle).GetMethod(nameof(BlockToggle.IsOrientedTo)), prefix: typeof(BlockToggle_IsOrientedTo_Patch).GetMethod(nameof(BlockToggle_IsOrientedTo_Patch.Prefix)));
            HarmonyInstance.Patch(original: typeof(BlockToggle).GetMethod(nameof(BlockToggle.TryPlaceBlock)), prefix: typeof(BlockToggle_TryPlaceBlock_Patch).GetMethod(nameof(BlockToggle_TryPlaceBlock_Patch.Prefix)));
            HarmonyInstance.Patch(original: typeof(MechNetworkRenderer).GetMethod(nameof(MechNetworkRenderer.AddDevice)), prefix: typeof(MechNetworkRenderer_AddDevice_Patch).GetMethod(nameof(MechNetworkRenderer_AddDevice_Patch.Prefix)));
            HarmonyInstance.Patch(original: typeof(BlockAngledGears).GetMethod(nameof(BlockAngledGears.OnPickBlock)), prefix: typeof(BlockAngledGears_OnPickBlock_Patch).GetMethod(nameof(BlockAngledGears_OnPickBlock_Patch.Prefix)));
            HarmonyInstance.Patch(original: typeof(BlockAngledGears).GetMethod(nameof(BlockAngledGears.getGearBlock)), prefix: typeof(BlockAngledGears_getGearBlock_Patch).GetMethod(nameof(BlockAngledGears_getGearBlock_Patch.Prefix)));
            HarmonyInstance.Patch(original: typeof(BlockAngledGears).GetMethod(nameof(BlockAngledGears.OnNeighbourBlockChange)), prefix: typeof(BlockAngledGears_OnNeighbourBlockChange_Patch).GetMethod(nameof(BlockAngledGears_OnNeighbourBlockChange_Patch.Prefix)));
            HarmonyInstance.Patch(original: typeof(BlockAngledGears).GetMethod("ToPegGear", AccessTools.all), prefix: typeof(BlockAngledGears_ToPegGear_Patch).GetMethod(nameof(BlockAngledGears_ToPegGear_Patch.Prefix)));
            HarmonyInstance.Patch(original: typeof(BEBrake).GetMethod("GenOpenedMesh", AccessTools.all), prefix: typeof(BEBrake_GenOpenedMesh_Patch).GetMethod(nameof(BEBrake_GenOpenedMesh_Patch.Prefix)));
            HarmonyInstance.CreateReversePatcher(original: typeof(Block).GetMethod(nameof(Block.TryPlaceBlock)), standin: typeof(Block_TryPlaceBlock_ReversePatch).GetMethod(nameof(Block_TryPlaceBlock_ReversePatch.Base))).Patch(HarmonyReversePatchType.Original);
            HarmonyInstance.Patch(original: typeof(BlockBrake).GetMethod(nameof(BlockBrake.TryPlaceBlock)), prefix: typeof(BlockBrake_TryPlaceBlock_Patch).GetMethod(nameof(BlockBrake_TryPlaceBlock_Patch.Prefix)));
            HarmonyInstance.Patch(original: typeof(BlockTransmission).GetMethod(nameof(BlockTransmission.IsOrientedTo)), prefix: typeof(BlockTransmission_IsOrientedTo_Patch).GetMethod(nameof(BlockTransmission_IsOrientedTo_Patch.Prefix)));
            HarmonyInstance.Patch(original: typeof(BlockTransmission).GetMethod(nameof(BlockTransmission.TryPlaceBlock)), prefix: typeof(BlockTransmission_TryPlaceBlock_Patch).GetMethod(nameof(BlockTransmission_TryPlaceBlock_Patch.Prefix)));
            HarmonyInstance.Patch(original: typeof(BlockClutch).GetMethod(nameof(BlockClutch.TryPlaceBlock)), prefix: typeof(BlockClutch_TryPlaceBlock_Patch).GetMethod(nameof(BlockClutch_TryPlaceBlock_Patch.Prefix)));
            HarmonyInstance.Patch(original: typeof(BlockWindmillRotor).GetMethod(nameof(BlockWindmillRotor.TryPlaceBlock)), prefix: typeof(BlockWindmillRotor_TryPlaceBlock_Patch).GetMethod(nameof(BlockWindmillRotor_TryPlaceBlock_Patch.Prefix)));
            HarmonyInstance.Patch(original: typeof(BlockLargeGear3m).GetMethod(nameof(BlockLargeGear3m.TryPlaceBlock)), prefix: typeof(BlockLargeGear3m_TryPlaceBlock_Patch).GetMethod(nameof(BlockLargeGear3m_TryPlaceBlock_Patch.Prefix)));
        }
    }

    public override void Dispose()
    {
        HarmonyInstance.Unpatch(original: typeof(GridRecipeLoader).GetMethod(nameof(GridRecipeLoader.LoadRecipe)), HarmonyPatchType.All, HarmonyInstance.Id);

        if (Core.Config.ExperimentalOverlayTest)
        {
            HarmonyInstance.Unpatch(original: typeof(ColorBlend).GetMethod(nameof(ColorBlend.Overlay)), HarmonyPatchType.All, HarmonyInstance.Id);
        }
        if (Core.Config.ResolveHenboxImposter)
        {
            HarmonyInstance.Unpatch(original: typeof(BlockEntityHenBox).GetMethod(nameof(BlockEntityHenBox.TryAddEgg)), HarmonyPatchType.All, HarmonyInstance.Id);
        }
        if (Core.Config.ResolveMechanicalBlockIssues)
        {
            HarmonyInstance.Unpatch(original: AccessTools.IndexerGetter(typeof(BEHelveHammer)), HarmonyPatchType.All, HarmonyInstance.Id);
            HarmonyInstance.Unpatch(original: typeof(BEBehaviorMPAxle).GetMethod("getStandMesh", AccessTools.all), HarmonyPatchType.All, HarmonyInstance.Id);
            HarmonyInstance.Unpatch(original: typeof(BlockAxle).GetMethod(nameof(BlockAxle.IsOrientedTo)), HarmonyPatchType.All, HarmonyInstance.Id);
            HarmonyInstance.Unpatch(original: typeof(BlockAxle).GetMethod(nameof(BlockAxle.TryPlaceBlock)), HarmonyPatchType.All, HarmonyInstance.Id);
            HarmonyInstance.Unpatch(original: typeof(BlockToggle).GetMethod(nameof(BlockToggle.IsOrientedTo)), HarmonyPatchType.All, HarmonyInstance.Id);
            HarmonyInstance.Unpatch(original: typeof(BlockToggle).GetMethod(nameof(BlockToggle.TryPlaceBlock)), HarmonyPatchType.All, HarmonyInstance.Id);
            HarmonyInstance.Unpatch(original: typeof(MechNetworkRenderer).GetMethod(nameof(MechNetworkRenderer.AddDevice)), HarmonyPatchType.All, HarmonyInstance.Id);
            HarmonyInstance.Unpatch(original: typeof(BlockAngledGears).GetMethod(nameof(BlockAngledGears.OnPickBlock)), HarmonyPatchType.All, HarmonyInstance.Id);
            HarmonyInstance.Unpatch(original: typeof(BlockAngledGears).GetMethod(nameof(BlockAngledGears.getGearBlock)), HarmonyPatchType.All, HarmonyInstance.Id);
            HarmonyInstance.Unpatch(original: typeof(BlockAngledGears).GetMethod(nameof(BlockAngledGears.OnNeighbourBlockChange)), HarmonyPatchType.All, HarmonyInstance.Id);
            HarmonyInstance.Unpatch(original: typeof(BlockAngledGears).GetMethod("ToPegGear", AccessTools.all), HarmonyPatchType.All, HarmonyInstance.Id);
            HarmonyInstance.Unpatch(original: typeof(BEBrake).GetMethod("GenOpenedMesh", AccessTools.all), HarmonyPatchType.All, HarmonyInstance.Id);
            HarmonyInstance.Unpatch(original: typeof(Block).GetMethod(nameof(Block.TryPlaceBlock)), HarmonyPatchType.All, HarmonyInstance.Id);
            HarmonyInstance.Unpatch(original: typeof(BlockBrake).GetMethod(nameof(BlockBrake.TryPlaceBlock)), HarmonyPatchType.All, HarmonyInstance.Id);
            HarmonyInstance.Unpatch(original: typeof(BlockTransmission).GetMethod(nameof(BlockTransmission.IsOrientedTo)), HarmonyPatchType.All, HarmonyInstance.Id);
            HarmonyInstance.Unpatch(original: typeof(BlockTransmission).GetMethod(nameof(BlockTransmission.TryPlaceBlock)), HarmonyPatchType.All, HarmonyInstance.Id);
            HarmonyInstance.Unpatch(original: typeof(BlockClutch).GetMethod(nameof(BlockClutch.TryPlaceBlock)), HarmonyPatchType.All, HarmonyInstance.Id);
            HarmonyInstance.Unpatch(original: typeof(BlockWindmillRotor).GetMethod(nameof(BlockWindmillRotor.TryPlaceBlock)), HarmonyPatchType.All, HarmonyInstance.Id);
            HarmonyInstance.Unpatch(original: typeof(BlockLargeGear3m).GetMethod(nameof(BlockLargeGear3m.TryPlaceBlock)), HarmonyPatchType.All, HarmonyInstance.Id);
        }
    }
}