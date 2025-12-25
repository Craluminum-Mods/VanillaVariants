using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent.Mechanics;

namespace VanillaVariants;

public static class BlockAngledGears_OnNeighbourBlockChange_Patch
{
    public static bool Prefix(BlockAngledGears __instance, IWorldAccessor world, BlockPos pos, BlockPos neibpos)
    {
        string orients = __instance.Orientation;
        if (orients.Length == 2 && orients[0] == orients[1])
        {
            orients = orients[0].ToString() ?? "";
        }
        BlockFacing[] obj = (orients.Length != 1) ? new BlockFacing[2]
        {
            BlockFacing.FromFirstLetter(orients[0]),
            BlockFacing.FromFirstLetter(orients[1])
        } : new BlockFacing[1] { BlockFacing.FromFirstLetter(orients[0]) };
        List<BlockFacing> lostFacings = new List<BlockFacing>();
        BlockFacing[] array = obj;
        foreach (BlockFacing facing in array)
        {
            BlockPos npos = pos.AddCopy(facing);
            if (world.BlockAccessor.GetBlock(npos) is IMechanicalPowerBlock nblock && nblock.HasMechPowerConnectorAt(world, npos, facing.Opposite))
            {
                BlockEntity blockEntity = world.BlockAccessor.GetBlockEntity(pos);
                if (blockEntity == null || blockEntity.GetBehavior<BEBehaviorMPBase>()?.disconnected != true)
                {
                    continue;
                }
            }
            lostFacings.Add(facing);
        }
        if (lostFacings.Count == orients.Length)
        {
            world.BlockAccessor.BreakBlock(pos, null);
        }
        else if (lostFacings.Count > 0)
        {
            orients = orients.Replace(lostFacings[0].Code[0].ToString() ?? "", "");
            BlockMPBase blockMpBase = world.GetBlock(__instance.CodeWithVariant("orientation", orients)) as BlockMPBase;
            blockMpBase.CallMethod("ExchangeBlockAt", world, pos);
            world.BlockAccessor.GetBlockEntity(pos)?.GetBehavior<BEBehaviorMPBase>()?.LeaveNetwork();
            BlockFacing firstFace = BlockFacing.FromFirstLetter(orients[0]);
            BlockPos firstPos = pos.AddCopy(firstFace);
            BlockEntity blockEntity2 = world.BlockAccessor.GetBlockEntity(firstPos);
            IMechanicalPowerBlock neighbour = blockEntity2?.Block as IMechanicalPowerBlock;
            if (blockEntity2?.GetBehavior<BEBehaviorMPAxle>() == null || BEBehaviorMPAxle.IsAttachedToBlock(world.BlockAccessor, neighbour as Block, firstPos))
            {
                neighbour?.DidConnectAt(world, firstPos, firstFace.Opposite);
                __instance.WasPlaced(world, pos, firstFace);
            }
        }
        return false;
    }
}
