using AttributeRenderingLibrary;
using HarmonyLib;
using System.Collections.Generic;
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
    [HarmonyPatch(typeof(BlockLiquidContainerTopOpened), nameof(BlockLiquidContainerTopOpened.OnBeforeRender))]
    public static class Bucket_OnBeforeRender_Patch
    {
        public static bool Prefix(BlockLiquidContainerTopOpened __instance, ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
        {
            if (Core.Config?.WoodBucket == false || __instance is not BlockBucket || !Variants.FromStack(itemstack).Any)
            {
                return true;
            }

            var behavior = __instance.GetBehavior<BlockBehaviorShapeTexturesFromAttributes>();
            if (behavior == null)
            {
                return true;
            }

            string meshRefsCacheKey = __instance.Code.ToShortString() + "extraMeshRefs";

            Dictionary<string, MultiTextureMeshRef> meshrefs;

            if (capi.ObjectCache.TryGetValue(meshRefsCacheKey, out object obj))
            {
                meshrefs = obj as Dictionary<string, MultiTextureMeshRef>;
            }
            else
            {
                capi.ObjectCache[meshRefsCacheKey] = meshrefs = new Dictionary<string, MultiTextureMeshRef>();
            }

            ItemStack contentStack = __instance.GetContent(itemstack);
            if (contentStack == null)
            {
                behavior.OnBeforeRender(capi, itemstack, target, ref renderinfo);
                return false;
            }

            string meshCacheKey = behavior.GetMeshCacheKey(itemstack) + __instance.CallMethod<int>("GetStackCacheHashCode", contentStack);
            if (!meshrefs.TryGetValue(meshCacheKey, out MultiTextureMeshRef meshRef))
            {
                MeshData meshdata = GenBucketMesh(capi, __instance, itemstack, contentStack);
                meshRef = capi.Render.UploadMultiTextureMesh(meshdata);
                meshrefs[meshCacheKey] = meshRef;
            }

            renderinfo.ModelRef = meshRef;
            return false;
        }
    }

    private static MeshData GenBucketMesh(ICoreClientAPI capi, BlockLiquidContainerTopOpened block, ItemStack ownStack, ItemStack contentStack, BlockPos forBlockPos = null)
    {
        var behavior = block.GetBehavior<BlockBehaviorShapeTexturesFromAttributes>();
        if (behavior == null)
        {
            return RenderExtensions.GenEmptyMesh();
        }

        BlockEntityBehaviorShapeTexturesFromAttributes bebehavior = null;
        if (forBlockPos != null)
        {
            bebehavior = block.GetBEBehavior<BlockEntityBehaviorShapeTexturesFromAttributes>(forBlockPos);
        }

        LiquidTopOpenContainerProps Props = block.GetField<LiquidTopOpenContainerProps>("Props");
        float liquidMaxYTranslate = Props.LiquidMaxYTranslate;
        float liquidYTranslatePerLitre = liquidMaxYTranslate / Props.CapacityLitres;
        AssetLocation emptyShapeLoc = Props.EmptyShapeLoc.Clone();
        AssetLocation contentShapeLoc = Props.OpaqueContentShapeLoc.Clone();
        AssetLocation liquidContentShapeLoc = Props.LiquidContentShapeLoc.Clone();
        emptyShapeLoc.WithPathPrefixOnce("shapes/").WithPathAppendixOnce(".json");
        contentShapeLoc.WithPathPrefixOnce("shapes/").WithPathAppendixOnce(".json");
        liquidContentShapeLoc.WithPathPrefixOnce("shapes/").WithPathAppendixOnce(".json");

        Variants variants = bebehavior != null ? bebehavior.Variants : Variants.FromStack(ownStack);
        MeshData containerMesh = behavior.GetOrCreateMesh(variants, new() { Base = emptyShapeLoc }, forBlockPos, "emptyShape").Clone();

        if (contentStack != null)
        {
            WaterTightContainableProps props = BlockLiquidContainerBase.GetContainableProps(contentStack);
            if (props == null)
            {
                capi.World.Logger.Error("Contents ('{0}') has no liquid properties, contents of liquid container {1} will be invisible.", contentStack.GetName(), block.Code);
                return containerMesh;
            }

            ContainerTextureSource contentSource = new ContainerTextureSource(capi, contentStack, props.Texture);

            AssetLocation loc = props.IsOpaque ? contentShapeLoc : liquidContentShapeLoc;
            Shape shape = Shape.TryGet(capi, loc);
            if (shape == null)
            {
                capi.World.Logger.Error("Content shape {0} not found. Contents of liquid container {1} will be invisible.", loc, block.Code);
                return containerMesh;
            }

            capi.Tesselator.TesselateShape(block.GetType().Name, shape, out MeshData contentMesh, contentSource, new Vec3f(), props.GlowLevel);

            contentMesh.Translate(0, GameMath.Min(liquidMaxYTranslate, contentStack.StackSize / props.ItemsPerLitre * liquidYTranslatePerLitre), 0);

            if (props.ClimateColorMap != null)
            {
                int col;
                if (forBlockPos != null)
                {
                    col = capi.World.ApplyColorMapOnRgba(props.ClimateColorMap, null, ColorUtil.WhiteArgb, forBlockPos.X, forBlockPos.Y, forBlockPos.Z, false);
                }
                else
                {
                    col = capi.World.ApplyColorMapOnRgba(props.ClimateColorMap, null, ColorUtil.WhiteArgb, 196, 128, false);
                }

                byte[] rgba = ColorUtil.ToBGRABytes(col);

                for (int i = 0; i < contentMesh.Rgba.Length; i++)
                {
                    contentMesh.Rgba[i] = (byte)((contentMesh.Rgba[i] * rgba[i % 4]) / 255);
                }
            }

            for (int i = 0; i < contentMesh.FlagsCount; i++)
            {
                contentMesh.Flags[i] = contentMesh.Flags[i] & ~(1 << 12); // Remove water waving flag
            }

            containerMesh.AddMeshData(contentMesh);

            // Water flags
            if (forBlockPos != null)
            {
                containerMesh.CustomInts = new CustomMeshDataPartInt(containerMesh.FlagsCount);
                containerMesh.CustomInts.Count = containerMesh.FlagsCount;
                containerMesh.CustomInts.Values.Fill(VertexFlags.LiquidWeakFoamBitMask); // light foam only

                containerMesh.CustomFloats = new CustomMeshDataPartFloat(containerMesh.FlagsCount * 2);
                containerMesh.CustomFloats.Count = containerMesh.FlagsCount * 2;
            }
        }
        return containerMesh;
    }

    [HarmonyPatchCategory("Client")]
    [HarmonyPatch(typeof(BlockLiquidContainerTopOpened), nameof(BlockLiquidContainerTopOpened.OnUnloaded))]
    public static class Bucket_OnUnloaded_Patch
    {
        public static void Postfix(BlockLiquidContainerTopOpened __instance, ICoreAPI api)
        {
            if (Core.Config?.WoodBucket == false || __instance is not BlockBucket)
            {
                return;
            }
            if (api is ICoreClientAPI capi)
            {
                string meshRefsCacheKey = __instance.Code.ToShortString() + "extraMeshRefs";
                Dictionary<string, MultiTextureMeshRef> meshRefs = ObjectCacheUtil.TryGet<Dictionary<string, MultiTextureMeshRef>>(api, meshRefsCacheKey);
                meshRefs?.Foreach(meshRef => meshRef.Value?.Dispose());
                ObjectCacheUtil.Delete(api, meshRefsCacheKey);
            }
        }
    }

    [HarmonyPatchCategory("Client")]
    [HarmonyPatch(typeof(BlockLiquidContainerTopOpened), nameof(BlockLiquidContainerTopOpened.GenMesh), [typeof(ItemStack), typeof(ITextureAtlasAPI), typeof(BlockPos)])]
    public static class Bucket_GenMesh_ContainedSource_Patch
    {
        public static bool Prefix(BlockLiquidContainerTopOpened __instance, ref MeshData __result, ItemStack itemstack, ITextureAtlasAPI targetAtlas, BlockPos forBlockPos, ICoreAPI ___api)
        {
            if (Core.Config?.WoodBucket == false || __instance is not BlockBucket || !Variants.FromStack(itemstack).Any)
            {
                return true;
            }

            ItemStack content = __instance.GetContent(itemstack);
            __result = GenBucketMesh(___api as ICoreClientAPI, __instance, itemstack, content, forBlockPos);
            return false;
        }
    }

    [HarmonyPatchCategory("Client")]
    [HarmonyPatch(typeof(BlockLiquidContainerTopOpened), nameof(BlockLiquidContainerTopOpened.GetMeshCacheKey))]
    public static class Bucket_GetMeshCacheKey_ContainedSource_Patch
    {
        public static bool Prefix(BlockLiquidContainerTopOpened __instance, ref string __result, ItemStack itemstack)
        {
            if (Core.Config?.WoodBucket == false || __instance is not BlockBucket || !Variants.FromStack(itemstack).Any)
            {
                return true;
            }

            var behavior = __instance.GetBehavior<BlockBehaviorShapeTexturesFromAttributes>();
            if (behavior == null)
            {
                return true;
            }

            ItemStack contentStack = __instance.GetContent(itemstack);
            __result = behavior.GetMeshCacheKey(itemstack) + "-";
            __result += itemstack.Collectible.Code.ToShortString() + "-" + contentStack?.StackSize + "x" + contentStack?.Collectible.Code.ToShortString();
            return false;
        }
    }

    [HarmonyPatchCategory("Client")]
    [HarmonyPatch(typeof(BlockEntityBucket), "GenMesh")]
    public static class BEBucket_GenMesh_Patch
    {
        public static bool Prefix(BlockEntityBucket __instance, ref MeshData __result, BlockBucket ___ownBlock)
        {
            if (Core.Config?.WoodBucket == false || ___ownBlock == null)
            {
                return true;
            }

            var bebehavior = __instance.GetBehavior<BlockEntityBehaviorShapeTexturesFromAttributes>();
            if (bebehavior == null || !bebehavior.Variants.Any)
            {
                return true;
            }

            MeshData mesh = GenBucketMesh(__instance.Api as ICoreClientAPI, ___ownBlock, null, __instance.GetContent(), __instance.Pos);
            if (mesh.CustomInts != null)
            {
                for (int i = 0; i < mesh.CustomInts.Count; i++)
                {
                    mesh.CustomInts.Values[i] |= VertexFlags.LiquidWeakWaveBitMask; // Enable weak water wavy
                    mesh.CustomInts.Values[i] |= VertexFlags.LiquidWeakFoamBitMask; // Enabled weak foamd
                }
            }

            __result = mesh;
            return false;
        }
    }
}