using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent.Mechanics;

namespace VanillaVariants;

public static class BlockAngledGears_OnPickBlock_Patch
{
    public static bool Prefix(BlockAngledGears __instance, ref ItemStack __result, IWorldAccessor world, BlockPos pos)
    {
        __result = new ItemStack(world.GetBlock(__instance.CodeWithVariant("orientation", "s"))); ;
        return false;
    }
}
