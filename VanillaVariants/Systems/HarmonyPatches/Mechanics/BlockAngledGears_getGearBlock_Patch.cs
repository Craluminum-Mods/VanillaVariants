using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent.Mechanics;

namespace VanillaVariants;

public static class BlockAngledGears_getGearBlock_Patch
{
    public static bool Prefix(BlockAngledGears __instance, ref Block __result, IWorldAccessor world, bool cageGear, BlockFacing facing, BlockFacing adjFacing = null)
    {
        if (adjFacing == null)
        {
            char orient = facing.Code[0];
            __result = world.GetBlock(__instance.CodeWithVariant("orientation", cageGear ? $"{orient}{orient}" : $"{orient}"));
            return false;
        }
        Block toPlaceBlock = world.GetBlock(__instance.CodeWithVariant("orientation", $"{adjFacing.Code[0]}{facing.Code[0]}"))
            ?? world.GetBlock(__instance.CodeWithVariant("orientation", $"{facing.Code[0]}{adjFacing.Code[0]}"));
        __result = toPlaceBlock;
        return false;
    }
}
