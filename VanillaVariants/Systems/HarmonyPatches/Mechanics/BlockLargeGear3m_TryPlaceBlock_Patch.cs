using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent.Mechanics;

namespace VanillaVariants;

public static class BlockLargeGear3m_TryPlaceBlock_Patch
{
    public static bool Prefix(BlockLargeGear3m __instance, ref bool __result, IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
    {
        try
        {
            List<BlockPos> smallGears = new();
            if (!__instance.CallMethodWithTypeArgs<bool>(
                method: "CanPlaceBlock",
                typeArgs: new Type[] { typeof(IWorldAccessor), typeof(IPlayer), typeof(BlockSelection), typeof(string).MakeByRefType(), typeof(List<BlockPos>) },
                world,
                byPlayer,
                blockSel,
                failureCode,
                smallGears))
            {
                __result = false;
                return false;
            }
            bool ok = __instance.DoPlaceBlock(world, byPlayer, blockSel, itemstack);
            if (ok)
            {
                BlockEntity beOwn = world.BlockAccessor.GetBlockEntity(blockSel.Position);
                List<BlockFacing> connections = new();
                foreach (BlockPos smallGear in smallGears)
                {
                    int dx = smallGear.X - blockSel.Position.X;
                    int dz = smallGear.Z - blockSel.Position.Z;
                    char orient = 'n';
                    switch (dx)
                    {
                        case 1:
                            orient = 'e';
                            break;
                        case -1:
                            orient = 'w';
                            break;
                        default:
                            if (dz == 1)
                            {
                                orient = 's';
                            }
                            break;
                    }
                    Block smallGearBlock = world.BlockAccessor.GetBlock(smallGear);
                    BlockMPBase obj = world.GetBlock(smallGearBlock.CodeWithVariant("orientation", $"{orient}{orient}")) as BlockMPBase;
                    BlockFacing bf = BlockFacing.FromFirstLetter(orient);
                    obj.CallMethod("ExchangeBlockAt", world, smallGear);
                    obj.DidConnectAt(world, smallGear, bf.Opposite);
                    connections.Add(bf);
                }
                __instance.CallMethod("PlaceFakeBlocks", world, blockSel.Position, smallGears);
                BEBehaviorMPBase beMechBase = beOwn?.GetBehavior<BEBehaviorMPBase>();
                BlockPos pos = blockSel.Position.DownCopy(1);
                if (world.BlockAccessor.GetBlock(pos) is IMechanicalPowerBlock block2 && block2.HasMechPowerConnectorAt(world, pos, BlockFacing.UP))
                {
                    block2.DidConnectAt(world, pos, BlockFacing.UP);
                    connections.Add(BlockFacing.DOWN);
                }
                else
                {
                    pos = blockSel.Position.UpCopy(1);
                    if (world.BlockAccessor.GetBlock(pos) is IMechanicalPowerBlock block && block.HasMechPowerConnectorAt(world, pos, BlockFacing.DOWN))
                    {
                        block.DidConnectAt(world, pos, BlockFacing.DOWN);
                        connections.Add(BlockFacing.UP);
                    }
                }
                foreach (BlockFacing face in connections)
                {
                    beMechBase?.WasPlaced(face);
                }
            }
            __result = ok;
            return false;
        }
        catch (Exception e)
        {
            world.Logger.Error("[Vanilla Variants] Something would have gone horribly wrong if placed:");
            world.Logger.Error(e);
            __result = false;
            return false;
        }
    }
}