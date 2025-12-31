using AttributeRenderingLibrary;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace VanillaVariants;

[HarmonyPatch]
public static partial class AttributeRenderingPatches
{
    [HarmonyPatchCategory("Universal")]
    [HarmonyPatch(typeof(BlockBed), nameof(BlockBed.OnPickBlock))]
    public static class Bed_OnPickBlock_Patch
    {
        public static void Postfix(BlockBed __instance, ref ItemStack __result, IWorldAccessor world, BlockPos pos)
        {
            if (Core.Config?.Bed == true && __result != null)
            {
                __instance.GetBEBehavior<BlockEntityBehaviorShapeTexturesFromAttributes>(pos)?.Variants.ToStack(__result);
            }
        }
    }

    [HarmonyPatchCategory("Universal")]
    [HarmonyPatch(typeof(BlockBed), nameof(BlockBed.GetDrops))]
    public static class Bed_GetDrops_Patch
    {
        public static void Postfix(BlockBed __instance, ref ItemStack[] __result, IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
        {
            if (Core.Config?.Bed == true && __result != null && __result.Length > 0)
            {
                __instance.GetBEBehavior<BlockEntityBehaviorShapeTexturesFromAttributes>(pos)?.Variants.ToStack(__result[0]);
            }
        }
    }
}