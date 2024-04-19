using Vintagestory.API.MathTools;
using Vintagestory.GameContent.Mechanics;

namespace VanillaVariants;

public static class BlockToggle_IsOrientedTo_Patch
{
    public static bool Prefix(BlockToggle __instance, ref bool __result, BlockFacing facing)
    {
        string dirs = __instance.Variant["orientation"];
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