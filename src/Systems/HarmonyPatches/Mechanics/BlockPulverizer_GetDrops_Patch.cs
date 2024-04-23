using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent.Mechanics;

namespace VanillaVariants;

public static class BlockPulverizer_GetDrops_Patch
{
    public static bool Prefix(BlockPulverizer __instance, ref ItemStack[] __result, IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
    {
        ItemStack pulvFrame = new ItemStack(world.BlockAccessor.GetBlock(__instance.CodeWithVariant("side", "north")));
        if (!(world.BlockAccessor.GetBlockEntity(pos) is BEPulverizer bep))
        {
            __result = new ItemStack[1] { pulvFrame };
            return false;
        }
        __result = bep.getDrops(world, pulvFrame);
        return false;
    }
}
