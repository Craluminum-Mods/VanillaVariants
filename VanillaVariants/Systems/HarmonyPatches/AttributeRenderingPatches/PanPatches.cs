using AttributeRenderingLibrary;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace VanillaVariants;

[HarmonyPatch]
public static partial class AttributeRenderingPatches
{
    [HarmonyPatchCategory("Client")]
    [HarmonyPatch(typeof(BlockPan), nameof(BlockPan.OnBeforeRender))]
    public static class Pan_OnBeforeRender_Patch
    {
        public static bool Prefix(BlockPan __instance, ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
        {
            if (Core.Config?.WoodenPan == false) return true;

            var behavior = __instance.GetBehavior<BlockBehaviorShapeTexturesFromAttributes>();
            if (behavior == null) return true;

            Variants variants = Variants.FromStack(itemstack);
            if (!variants.Any) return true;

            string blockMaterialCode = __instance.GetBlockMaterialCode(itemstack);
            if (blockMaterialCode == null)
            {
                behavior.OnBeforeRender(capi, itemstack, target, ref renderinfo);
                return false;
            }

            string key = __instance.GetMeshCacheKey(itemstack) + target;

            renderinfo.ModelRef = ObjectCacheUtil.GetOrCreate(capi, key, () =>
            {
                MeshData mesh = __instance.GenMesh(itemstack, capi.BlockTextureAtlas, null);
                return capi.Render.UploadMultiTextureMesh(mesh);
            });
            return false;
        }
    }

    [HarmonyPatchCategory("Client")]
    [HarmonyPatch(typeof(BlockPan), nameof(BlockPan.GenMesh))]
    public static class Pan_GenMesh_Patch
    {
        public static bool Prefix(BlockPan __instance, ref MeshData __result, ItemStack itemstack, ITextureAtlasAPI targetAtlas, BlockPos atBlockPos, ICoreAPI ___api, ref ITexPositionSource ___ownTextureSource, ref TextureAtlasPosition ___matTexPosition, AssetLocation ___shapeEmpty, AssetLocation ___shapeFull)
        {
            if (Core.Config?.WoodenPan == false) return true;

            var bebehavior = __instance.GetBehavior<BlockBehaviorShapeTexturesFromAttributes>();
            if (bebehavior == null) return true;

            Variants variants = Variants.FromStack(itemstack);
            if (!variants.Any) return true;

            string blockMaterialCode = __instance.GetBlockMaterialCode(itemstack);

            ICoreClientAPI capi = ___api as ICoreClientAPI;
            AssetLocation shapeloc = blockMaterialCode != null ? ___shapeFull : ___shapeEmpty;

            Shape shape = Shape.TryGet(capi, shapeloc);

            Block blockMat = null;
            if (blockMaterialCode != null)
            {
                blockMat = capi.World.GetBlock(new AssetLocation(blockMaterialCode));
            }

            __instance.AtlasSize = capi.BlockTextureAtlas.Size;

            if (blockMat != null)
            {
                ___matTexPosition = capi.BlockTextureAtlas.GetPosition(blockMat, "up");
            }

            ___ownTextureSource = capi.Tesselator.GetTextureSource(__instance);

            __result = bebehavior.GetOrCreateMesh(variants).Clone();

            if (blockMat != null)
            {
                MeshData contentMesh;
                TesselationMetaData contentMetaData = new()
                {
                    TypeForLogging = "filledpan",
                    TexSource = __instance
                };

                capi.Tesselator.TesselateShape(contentMetaData, shape, out contentMesh);

                __result.AddMeshData(contentMesh);
            }
            return false;
        }
    }

    [HarmonyPatchCategory("Client")]
    [HarmonyPatch(typeof(BlockPan), nameof(BlockPan.GetMeshCacheKey))]
    public static class Pan_GetMeshCacheKey_Patch
    {
        public static void Postfix(BlockPan __instance, ref string __result, ItemStack itemstack)
        {
            if (Core.Config?.WoodenPan == false) return;

            var behavior = __instance.GetBehavior<BlockBehaviorShapeTexturesFromAttributes>();
            if (behavior == null) return;

            string cacheKey = behavior.GetMeshCacheKey(itemstack);

            string blockMaterialCode = __instance.GetBlockMaterialCode(itemstack);
            if (blockMaterialCode == null)
            {
                __result = cacheKey;
            }
            else
            {
                __result = string.Concat(cacheKey, "pan-filled-", blockMaterialCode);
            }
        }
    }
}