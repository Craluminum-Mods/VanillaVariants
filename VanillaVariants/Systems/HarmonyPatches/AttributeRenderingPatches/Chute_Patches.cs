using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace VanillaVariants;

[HarmonyPatch]
public static partial class AttributeRenderingPatches
{
    [HarmonyPatchCategory("Universal")]
    [HarmonyPatch(typeof(BlockChute), nameof(BlockChute.GetDrops))]
    public static class Chute_GetDrops_Patch
    {
        public static bool Prefix(BlockChute __instance, ref ItemStack[] __result, IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier)
        {
            if (Core.Config?.Chute == false) return true;

            Block block = null;
            if (__instance.Type == "elbow" || __instance.Type == "3way")
            {
                block = world.GetBlock(__instance.CodeWithVariants(new string[2] { "vertical", "side" }, new string[2] { "down", "east" }));
            }
            else if (__instance.Type == "t" || __instance.Type == "straight")
            {
                block = world.GetBlock(__instance.CodeWithVariant("side", "ns"));
            }
            else if (__instance.Type == "cross")
            {
                block = world.GetBlock(__instance.CodeWithVariant("side", "ground"));
            }

            var tempHandling = EnumHandling.PassThrough;
            __result = block?.GetBehavior<AttributeRenderingLibrary.BlockBehaviorShapeTexturesFromAttributes>()?.GetDrops(world, pos, byPlayer, ref dropQuantityMultiplier, ref tempHandling);

            if (__result == null || __result.Length <= 0 || __result[0] == null)
            {
                return true;
            }
            return false;
        }
    }

    [HarmonyPatchCategory("Universal")]
    [HarmonyPatch(typeof(BlockChute), nameof(BlockChute.TryPlaceBlock))]
    public static class Chute_TryPlaceBlock_Patch
    {
        public static void Postfix(BlockChute __instance, ItemStack itemstack, BlockSelection blockSel)
        {
            if (Core.Config?.Chute == false) return;

            __instance?.GetBEBehavior<AttributeRenderingLibrary.BlockEntityBehaviorShapeTexturesFromAttributes>(blockSel.Position)?.OnBlockPlaced(itemstack);
        }
    }
}