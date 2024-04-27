using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace VanillaVariants;

public static class Block_TryPlaceBlock_ReversePatch
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static bool Base(this Block instance, IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
    {
        return default;
    }
}
