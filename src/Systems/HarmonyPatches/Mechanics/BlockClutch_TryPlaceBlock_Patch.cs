using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent.Mechanics;

namespace VanillaVariants;

public static class BlockClutch_TryPlaceBlock_Patch
{
    public static bool Prefix(BlockClutch __instance, ref bool __result, IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
    {
        if (!__instance.CanPlaceBlock(world, byPlayer, blockSel, ref failureCode))
        {
            __result = false;
            return false;
        }
        BlockFacing frontFacing = Block.SuggestedHVOrientation(byPlayer, blockSel)[0];
        BlockFacing bestFacing = frontFacing;
        if (!(world.BlockAccessor.GetBlock(blockSel.Position.AddCopy(frontFacing)) is BlockTransmission))
        {
            BlockFacing leftFacing = BlockFacing.HORIZONTALS_ANGLEORDER[GameMath.Mod(frontFacing.HorizontalAngleIndex - 1, 4)];
            if (world.BlockAccessor.GetBlock(blockSel.Position.AddCopy(leftFacing)) is BlockTransmission)
            {
                bestFacing = leftFacing;
            }
            else
            {
                BlockFacing rightFacing = leftFacing.Opposite;
                if (world.BlockAccessor.GetBlock(blockSel.Position.AddCopy(rightFacing)) is BlockTransmission)
                {
                    bestFacing = rightFacing;
                }
                else
                {
                    BlockFacing backFacing = frontFacing.Opposite;
                    if (world.BlockAccessor.GetBlock(blockSel.Position.AddCopy(backFacing)) is BlockTransmission)
                    {
                        bestFacing = backFacing;
                    }
                }
            }
        }
        __result = world.BlockAccessor.GetBlock(__instance.CodeWithVariant("side", bestFacing.Code)).DoPlaceBlock(world, byPlayer, blockSel, itemstack);
        return false;
    }
}