using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent.Mechanics;

namespace VanillaVariants;

public static class BlockBrake_TryPlaceBlock_Patch
{
    public static bool Prefix(BlockBrake __instance, ref bool __result, IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
    {
        if (!__instance.CanPlaceBlock(world, byPlayer, blockSel, ref failureCode))
        {
            __result = false;
            return false;
        }
        AssetLocation blockCode = __instance.CodeWithVariant("side", Block.SuggestedHVOrientation(byPlayer, blockSel)[0].Code);
        Block orientedBlock = world.BlockAccessor.GetBlock(blockCode);
        BlockFacing ownFacing = BlockFacing.FromCode(orientedBlock.Variant["side"]);
        BlockFacing leftFacing = BlockFacing.HORIZONTALS_ANGLEORDER[GameMath.Mod(ownFacing.HorizontalAngleIndex - 1, 4)];
        BlockFacing rightFacing = BlockFacing.HORIZONTALS_ANGLEORDER[GameMath.Mod(ownFacing.HorizontalAngleIndex + 1, 4)];
        if (world.BlockAccessor.GetBlock(blockSel.Position.AddCopy(leftFacing)) is IMechanicalPowerBlock leftBlock)
        {
            __result = __instance.CallMethod<bool>("DoPlaceMechBlock", world, byPlayer, itemstack, blockSel, orientedBlock, leftBlock, leftFacing);
            return false;
        }
        if (world.BlockAccessor.GetBlock(blockSel.Position.AddCopy(rightFacing)) is IMechanicalPowerBlock rightBlock)
        {
            __result = __instance.CallMethod<bool>("DoPlaceMechBlock", world, byPlayer, itemstack, blockSel, orientedBlock, rightBlock, rightFacing);
            return false;
        }
        BlockFacing frontFacing = ownFacing;
        BlockFacing backFacing = ownFacing.Opposite;
        Block rotBlock = world.GetBlock(orientedBlock.CodeWithVariant("side", leftFacing.Code));
        if (world.BlockAccessor.GetBlock(blockSel.Position.AddCopy(frontFacing)) is IMechanicalPowerBlock frontBlock)
        {
            __result = __instance.CallMethod<bool>("DoPlaceMechBlock", world, byPlayer, itemstack, blockSel, rotBlock, frontBlock, frontFacing);
            return false;
        }
        if (world.BlockAccessor.GetBlock(blockSel.Position.AddCopy(backFacing)) is IMechanicalPowerBlock backBlock)
        {
            __result = __instance.CallMethod<bool>("DoPlaceMechBlock", world, byPlayer, itemstack, blockSel, rotBlock, backBlock, backFacing);
            return false;
        }

        if (Block_TryPlaceBlock_ReversePatch.Base(__instance, world, byPlayer, itemstack, blockSel, ref failureCode))
        {
            __instance.WasPlaced(world, blockSel.Position, null);
            __result = true;
            return false;
        }
        __result = false;
        return false;
    }
}