using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace VanillaVariants;

public static class BlockBasketTrap_GetDrops_Patch
{
    public static void Postfix(BlockBasketTrap __instance, ref ItemStack[] __result, IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
    {
        BlockEntityBasketTrap be = __instance.GetBlockEntity<BlockEntityBasketTrap>(pos);
        if (be != null && be.TrapState == EnumTrapState.Destroyed)
        {
            JsonItemStack drop = __instance?.Attributes?["drop"]?.AsObject<JsonItemStack>();
            if (drop != null && drop.Resolve(world, ""))
            {
                ItemStack newStack = drop.ResolvedItemstack;
                newStack.StackSize = 6 + world.Rand.Next(8);
                __result = new ItemStack[1] { newStack };
            }
        }
    }
}