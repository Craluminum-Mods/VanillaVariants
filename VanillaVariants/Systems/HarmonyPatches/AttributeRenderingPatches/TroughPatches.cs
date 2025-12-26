using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace VanillaVariants;

[HarmonyPatch]
public static partial class AttributeRenderingPatches
{
    [HarmonyPatchCategory("Client")]
    [HarmonyPatch(typeof(BlockEntityTrough), nameof(BlockEntityTrough.OnTesselation))]
    public static class Trough_OnTesselation_Patch
    {
        public static bool Prefix(BlockEntityTrough __instance, ref bool __result, ITerrainMeshPool mesher, ITesselatorAPI tesselator)
        {
            if ((__instance.Block is BlockTroughDoubleBlock && Core.Config?.TroughLarge == false) ||
                (__instance.Block is BlockTrough && Core.Config?.TroughSmall == false))
            {
                return true;
            }
            for (int i = 0; i < __instance.Behaviors.Count; i++)
            {
                __instance.Behaviors[i].OnTesselation(mesher, tesselator);
            }
            __result = true;
            return false;
        }
    }

    [HarmonyPatchCategory("Universal")]
    [HarmonyPatch(typeof(BlockTroughDoubleBlock), nameof(BlockTroughDoubleBlock.GetDrops))]
    public static class Trough_GetDrops_Patch
    {
        public static bool Prefix(BlockTroughDoubleBlock __instance, ref ItemStack[] __result, IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
        {
            if (Core.Config?.TroughLarge == false)
            {
                return true;
            }
            Block newBlock = world.BlockAccessor.GetBlock(__instance.CodeWithVariants([ "part", "side" ], ["large-head", "north"]));
            var newBehavior = newBlock?.GetBehavior<AttributeRenderingLibrary.BlockBehaviorShapeTexturesFromAttributes>();
            if (newBehavior == null)
            {
                return true;
            }
            var tempHandling = EnumHandling.PassThrough;
            __result = newBehavior.GetDrops(world, pos, byPlayer, ref dropQuantityMultiplier, ref tempHandling);
            return false;
        }
    }

    [HarmonyPatchCategory("Universal")]
    [HarmonyPatch(typeof(BlockTroughDoubleBlock), nameof(BlockTroughDoubleBlock.OnPickBlock))]
    public static class Trough_OnPickBlock_Patch
    {
        public static bool Prefix(BlockTroughDoubleBlock __instance, ref ItemStack __result, IWorldAccessor world, BlockPos pos)
        {
            if (Core.Config?.TroughLarge == false)
            {
                return true;
            }
            Block newBlock = world.BlockAccessor.GetBlock(__instance.CodeWithVariants([ "part", "side" ], ["large-head", "north"]));
            var newBehavior = newBlock?.GetBehavior<AttributeRenderingLibrary.BlockBehaviorShapeTexturesFromAttributes>();
            if (newBehavior == null)
            {
                return true;
            }
            var tempHandling = EnumHandling.PassThrough;
            __result = newBehavior.OnPickBlock(world, pos, ref tempHandling);
            return false;
        }
    }
}