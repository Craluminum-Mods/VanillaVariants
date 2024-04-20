using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent.Mechanics;

namespace VanillaVariants;

public static class BlockAxle_TryPlaceBlock_Patch
{
    public static bool Prefix(BlockAxle __instance, ref bool __result, IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
    {
        if (!__instance.CanPlaceBlock(world, byPlayer, blockSel, ref failureCode))
        {
            __result = false;
            return false;
        }
        BlockFacing[] aLLFACES = BlockFacing.ALLFACES;
        foreach (BlockFacing face in aLLFACES)
        {
            BlockPos pos = blockSel.Position.AddCopy(face);
            if (!(world.BlockAccessor.GetBlock(pos) is IMechanicalPowerBlock block2))
            {
                continue;
            }
            BlockFacing faceOpposite = face.Opposite;
            if (!block2.HasMechPowerConnectorAt(world, pos, faceOpposite))
            {
                continue;
            }
            Block toPlaceBlock = world.GetBlock(__instance.CodeWithVariant("rotation", $"{faceOpposite.Code[0]}{face.Code[0]}"));
            if (toPlaceBlock == null)
            {
                toPlaceBlock = world.GetBlock(__instance.CodeWithVariant("rotation", $"{face.Code[0]}{faceOpposite.Code[0]}"));
            }
            if (toPlaceBlock.DoPlaceBlock(world, byPlayer, blockSel, itemstack))
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

        // if (base.TryPlaceBlock(world, byPlayer, itemstack, blockSel, ref failureCode))
        // {
        // 	this.WasPlaced(world, blockSel.Position, null);
        // 	__result = true;
        //     return false;
        // }
        return true;
    }
}