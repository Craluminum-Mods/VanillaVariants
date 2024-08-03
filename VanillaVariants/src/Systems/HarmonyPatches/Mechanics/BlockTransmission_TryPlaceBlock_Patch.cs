using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent.Mechanics;

namespace VanillaVariants;

public static class BlockTransmission_TryPlaceBlock_Patch
{
    public static bool Prefix(BlockTransmission __instance, ref bool __result, IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
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
            if (!(world.BlockAccessor.GetBlock(pos) is IMechanicalPowerBlock block2))
            {
                continue;
            }
            BlockFacing faceOpposite = face.Opposite;
            if (block2.HasMechPowerConnectorAt(world, pos, faceOpposite) && world.GetBlock((face != BlockFacing.EAST && face != BlockFacing.WEST) ? __instance.CodeWithVariant("orientation", "ns") : __instance.CodeWithVariant("orientation", "we")).DoPlaceBlock(world, byPlayer, blockSel, itemstack))
            {
                block2.DidConnectAt(world, pos, faceOpposite);
                __instance.WasPlaced(world, blockSel.Position, face);
                pos = blockSel.Position.AddCopy(faceOpposite);
                if (world.BlockAccessor.GetBlock(pos) is IMechanicalPowerBlock block && block.HasMechPowerConnectorAt(world, pos, face))
                {
                    block.DidConnectAt(world, pos, face);
                    __instance.WasPlaced(world, blockSel.Position, faceOpposite);
                }
                __result = true;
                return false;
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