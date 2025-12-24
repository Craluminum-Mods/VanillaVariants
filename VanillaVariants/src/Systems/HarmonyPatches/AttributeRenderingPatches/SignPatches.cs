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
    [HarmonyPatch(typeof(BlockEntitySign), nameof(BlockEntitySign.OnTesselation))]
    public static class Sign_OnTesselation_Patch
    {
        public static bool Prefix(BlockEntitySign __instance, ref bool __result, ITerrainMeshPool mesher, ITesselatorAPI tessThreadTesselator)
        {
            if (Core.Config?.Sign == false)
            {
                return true;
            }

            if (__instance.Block.Variant["attachment"] != "ground")
            {
                return true;
            }

            var renderingBehavior = __instance.Block.GetBehavior<AttributeRenderingLibrary.BlockBehaviorShapeTexturesFromAttributes>();
            var variants = __instance.Block.GetBEBehavior<AttributeRenderingLibrary.BlockEntityBehaviorShapeTexturesFromAttributes>(__instance.Pos)?.Variants;
            if (renderingBehavior == null || variants == null || !variants.Any)
            {
                return true;
            }

            MeshData cachedMesh = renderingBehavior.GetOrCreateMesh(variants);
            mesher.AddMeshData(cachedMesh.Clone().Rotate(Vec3f.Half, 0f, __instance.MeshAngleRad, 0f));
            __result = true;
            return false;
        }
    }

    [HarmonyPatchCategory("Universal")]
    [HarmonyPatch(typeof(BlockSign), nameof(BlockSign.OnPickBlock))]
    public static class Sign_OnPickBlock_Patch
    {
        public static bool Prefix(BlockSign __instance, ref ItemStack __result, IWorldAccessor world, BlockPos pos)
        {
            if (Core.Config?.Sign == false)
            {
                return true;
            }

            Block newBlock = world.BlockAccessor.GetBlock(__instance.CodeWithParts("ground", "north"));
            if (newBlock == null)
            {
                newBlock = world.BlockAccessor.GetBlock(__instance.CodeWithParts("wall", "north"));
            }

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

    [HarmonyPatchCategory("Universal")]
    [HarmonyPatch(typeof(BlockSign), nameof(BlockSign.GetDrops))]
    public static class Sign_GetDrops_Patch
    {
        public static bool Prefix(BlockSign __instance, ref ItemStack[] __result, IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
        {
            if (Core.Config?.Sign == false)
            {
                return true;
            }

            Block newBlock = world.BlockAccessor.GetBlock(__instance.CodeWithParts("ground", "north"));
            if (newBlock == null)
            {
                newBlock = world.BlockAccessor.GetBlock(__instance.CodeWithParts("wall", "north"));
            }

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
    [HarmonyPatch(typeof(BlockSign), nameof(BlockSign.TryPlaceBlock))]
    public static class Sign_TryPlaceBlock_Patch
    {
        public static void Postfix(BlockSign __instance, ref bool __result, ItemStack itemstack, BlockSelection bs)
        {
            if (Core.Config?.Sign == false || !__result)
            {
                return;
            }
            (__instance?.GetBEBehavior<AttributeRenderingLibrary.BlockEntityBehaviorShapeTexturesFromAttributes>(bs.Position))?.OnBlockPlaced(itemstack);
        }
    }
}
