using Vintagestory.API.MathTools;
using Vintagestory.GameContent.Mechanics;

namespace VanillaVariants;

public static class BlockAxle_IsOrientedTo_Patch
{
    public static bool Prefix(BlockAxle __instance, ref bool __result, BlockFacing facing)
    {
        string dirs = __instance.Variant["rotation"];
        if (dirs[0] != facing.Code[0])
        {
            if (dirs.Length > 1)
            {
                __result = dirs[1] == facing.Code[0];
                return false;
            }
            __result = false;
            return false;
        }
        __result = true;
        return false;
    }
}