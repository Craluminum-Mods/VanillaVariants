using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent.Mechanics;

namespace VanillaVariants;

public static class BlockToggle_TryPlaceBlock_Patch
{
    public static bool Prefix(BlockToggle __instance, ref bool __result, IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
    {
        if (!__instance.CanPlaceBlock(world, byPlayer, blockSel, ref failureCode))
        {
            __result = false;
            return false;
        }
        BlockFacing[] hORIZONTALS = BlockFacing.HORIZONTALS;
        foreach (BlockFacing face in hORIZONTALS)
        {
            BlockPos pos = blockSel.Position.AddCopy(face);
            if (world.BlockAccessor.GetBlock(pos) is IMechanicalPowerBlock block && block.HasMechPowerConnectorAt(world, pos, face.Opposite))
            {
                Block toPlaceBlock = world.GetBlock(__instance.CodeWithVariant("orientation", $"{face.Opposite.Code[0]}{face.Code[0]}"))
                    ?? world.GetBlock(__instance.CodeWithVariant("orientation", $"{face.Code[0]}{face.Opposite.Code[0]}"));

                if (toPlaceBlock.DoPlaceBlock(world, byPlayer, blockSel, itemstack))
                {
                    block.DidConnectAt(world, pos, face.Opposite);
                    __instance.WasPlaced(world, blockSel.Position, face);
                    __result = true;
                    return false;
                }
            }
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