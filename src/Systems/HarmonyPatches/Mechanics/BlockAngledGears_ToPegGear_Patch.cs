using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent.Mechanics;

namespace VanillaVariants;

public static class BlockAngledGears_ToPegGear_Patch
{
    public static bool Prefix(BlockAngledGears __instance, IWorldAccessor world, BlockPos pos)
    {
        string orient = __instance.Variant["orientation"];
        if (orient.Length == 2 && orient[1] == orient[0])
        {
            BlockMPBase blockMpBase = (BlockMPBase)world.GetBlock(__instance.CodeWithVariant("orientation", $"{orient[0]}"));
            blockMpBase.CallMethod("ExchangeBlockAt", world, pos);
            (world.BlockAccessor.GetBlockEntity(pos)?.GetBehavior<BEBehaviorMPAngledGears>())?.ClearLargeGear();
        }
        return false;
    }
}