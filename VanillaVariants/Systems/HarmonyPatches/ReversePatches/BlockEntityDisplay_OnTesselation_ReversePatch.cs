using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.GameContent;

namespace VanillaVariants;

public static class BlockEntityDisplay_OnTesselation_ReversePatch
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static bool Base(this BlockEntityDisplay instance, ITerrainMeshPool mesher, ITesselatorAPI tessThreadTesselator)
    {
        return default;
    }
}
