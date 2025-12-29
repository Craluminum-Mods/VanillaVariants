using AttributeRenderingLibrary;
using HarmonyLib;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace VanillaVariants;

[HarmonyPatch]
public static partial class AttributeRenderingPatches
{
    [HarmonyPatchCategory("Client")]
    [HarmonyPatch(typeof(BlockBarrel), nameof(BlockBarrel.OnBeforeRender))]
    public static class Barrel_OnBeforeRender_Patch
    {
        public static bool Prefix(BlockBarrel __instance, ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
        {
            if (Core.Config?.Barrel == false)
            {
                return true;
            }

            var behavior = __instance.GetBehavior<BlockBehaviorShapeTexturesFromAttributes>();
            if (behavior == null)
            {
                return true;
            }

            Dictionary<string, MultiTextureMeshRef> meshrefs;

            if (capi.ObjectCache.TryGetValue("barrelMeshRefs" + __instance.Code, out object obj))
            {
                meshrefs = obj as Dictionary<string, MultiTextureMeshRef>;
            }
            else
            {
                capi.ObjectCache["barrelMeshRefs" + __instance.Code] = meshrefs = [];
            }

            ItemStack[] contentStacks = __instance.GetContents(capi.World, itemstack);
            if (contentStacks == null || contentStacks.Length == 0)
            {
                behavior.OnBeforeRender(capi, itemstack, target, ref renderinfo);
                return false;
            }

            bool issealed = itemstack.Attributes.GetBool("sealed");

            string meshkey = __instance.GetBarrelMeshkey(contentStacks[0], contentStacks.Length > 1 ? contentStacks[1] : null);
            meshkey += behavior.GetMeshCacheKey(itemstack);

            if (!meshrefs.TryGetValue(meshkey, out MultiTextureMeshRef meshRef))
            {
                MeshData meshdata = GenBarrelMesh(__instance, itemstack, contentStacks[0], contentStacks.Length > 1 ? contentStacks[1] : null, issealed);
                meshrefs[meshkey] = meshRef = capi.Render.UploadMultiTextureMesh(meshdata);
            }

            renderinfo.ModelRef = meshRef;
            return false;
        }
    }

    private static MeshData GenBarrelMesh(BlockBarrel block, ItemStack ownStack, ItemStack contentStack, ItemStack liquidContentStack, bool issealed, BlockPos forBlockPos = null)
    {
        MeshData barrelMesh = RenderExtensions.GenEmptyMesh();

        if (Core.Config?.Barrel == false)
        {
            return barrelMesh;
        }

        var behavior = block.GetBehavior<BlockBehaviorShapeTexturesFromAttributes>();
        if (behavior == null)
        {
            return barrelMesh;
        }

        BlockEntityBehaviorShapeTexturesFromAttributes bebehavior = null;
        if (forBlockPos != null)
        {
            bebehavior = block.GetBEBehavior<BlockEntityBehaviorShapeTexturesFromAttributes>(forBlockPos);
        }

        if (issealed)
        {
            CompositeShape sealedShape = new() { Base = block.sealedShape };
            string extraCacheKey = sealedShape.Base.ToString();

            Variants variants = bebehavior != null ? bebehavior.Variants : Variants.FromStack(ownStack);
            barrelMesh = behavior.GetOrCreateMesh(variants, sealedShape, forBlockPos, extraCacheKey, null);
        }
        else
        {
            CompositeShape emptyShape = new() { Base = block.emptyShape };
            string extraCacheKey = emptyShape.Base.ToString();

            Variants variants = bebehavior != null ? bebehavior.Variants : Variants.FromStack(ownStack);
            barrelMesh = behavior.GetOrCreateMesh(variants, emptyShape, forBlockPos, extraCacheKey, null);

            JsonObject containerProps = liquidContentStack?.ItemAttributes?["waterTightContainerProps"];

            MeshData contentMesh = 
                block.CallMethod<MeshData>("getContentMeshFromAttributes", contentStack, liquidContentStack, forBlockPos) ?? 
                block.CallMethod<MeshData>("getContentMeshLiquids",contentStack, liquidContentStack, forBlockPos, containerProps) ??
                block.CallMethod<MeshData>("getContentMesh", contentStack, forBlockPos, block.contentsShape);

            if (contentMesh != null)
            {
                barrelMesh = barrelMesh.Clone();
                barrelMesh.AddMeshData(contentMesh);
                if (forBlockPos != null)
                {
                    barrelMesh.CustomInts = new CustomMeshDataPartInt(barrelMesh.FlagsCount);
                    barrelMesh.CustomInts.Values.Fill(268435456);
                    barrelMesh.CustomInts.Count = barrelMesh.FlagsCount;
                    barrelMesh.CustomFloats = new CustomMeshDataPartFloat(barrelMesh.FlagsCount * 2);
                    barrelMesh.CustomFloats.Count = barrelMesh.FlagsCount * 2;
                }
            }
        }
        return barrelMesh;
    }

    [HarmonyPatchCategory("Client")]
    [HarmonyPatch(typeof(BlockEntityBarrel), "GenMesh")]
    public static class BEBarrel_GenMesh_Patch
    {
        public static bool Prefix(BlockEntityBarrel __instance, ref MeshData __result, BlockBarrel ___ownBlock)
        {
            if (Core.Config?.Barrel == false || ___ownBlock == null)
            {
                return true;
            }

            var bebehavior = __instance.GetBehavior<BlockEntityBehaviorShapeTexturesFromAttributes>();
            if (bebehavior == null || !bebehavior.Variants.Any)
            {
                /*
                ---------------------------------------------------------------------------
                === for variant-less barrel: draw variant-less barrel invisible (incorrect)
                === for variant barrel: expected behavior
                    
                    __result = null;
                    return false;
                ---------------------------------------------------------------------------
                === for variant-less barrel: expected behavior
                === for variant barrel: draws variant-less barrel first (incorrect), then after delay draws variant barrel (correct)

                    return true;
                ---------------------------------------------------------------------------
                 */

                return true;
            }

            MeshData mesh = GenBarrelMesh(___ownBlock, null, __instance.Inventory[0].Itemstack, __instance.Inventory[1].Itemstack, __instance.Sealed, __instance.Pos);
            if (mesh.CustomInts != null)
            {
                int[] CustomInts = mesh.CustomInts.Values;
                int count = mesh.CustomInts.Count;
                for (int i = 0; i < CustomInts.Length; i++)
                {
                    if (i >= count) break;
                    CustomInts[i] |= VertexFlags.LiquidWeakWaveBitMask  // Enable weak water wavy
                                    | VertexFlags.LiquidWeakFoamBitMask;  // Enabled weak foam
                }
            }

            __result = mesh;
            return false;
        }
    }
}