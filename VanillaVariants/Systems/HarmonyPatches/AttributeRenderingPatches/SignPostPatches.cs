using HarmonyLib;
using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace VanillaVariants;

[HarmonyPatch]
public static partial class AttributeRenderingPatches
{
    [HarmonyPatchCategory("Client")]
    [HarmonyPatch(typeof(BlockEntitySignPost), nameof(BlockEntitySignPost.OnTesselation))]
    public static class SignPost_OnTesselation_Patch
    {
        public static bool Prefix(BlockEntitySignPost __instance, ref bool __result, ITerrainMeshPool mesher, ITesselatorAPI tessThreadTesselator, string[] ___textByCardinalDirection, MeshData ___signMesh)
        {
            if (Core.Config?.Signpost == false)
            {
                return true;
            }

            for (int i = 0; i < 8; i++)
            {
                if (___textByCardinalDirection[i].Length != 0)
                {
                    Cardinal obj = Cardinal.ALL[i];
                    float rotY = 0f;
                    switch (obj.Index)
                    {
                        case 0:
                            rotY = 180f;
                            break;
                        case 1:
                            rotY = 135f;
                            break;
                        case 2:
                            rotY = 90f;
                            break;
                        case 3:
                            rotY = 45f;
                            break;
                        case 5:
                            rotY = 315f;
                            break;
                        case 6:
                            rotY = 270f;
                            break;
                        case 7:
                            rotY = 225f;
                            break;
                    }
                    mesher.AddMeshData(___signMesh.Clone().Rotate(new Vec3f(0.5f, 0.5f, 0.5f), 0f, rotY * ((float)Math.PI / 180f), 0f));
                }
            }

            for (int i = 0; i < __instance.Behaviors.Count; i++)
            {
                __instance.Behaviors[i].OnTesselation(mesher, tessThreadTesselator);
            }

            __result = true;
            return false;
        }
    }

    [HarmonyPatchCategory("Universal")]
    [HarmonyPatch(typeof(BlockSignPost), nameof(BlockSignPost.TryPlaceBlock))]
    public static class SignPost_TryPlaceBlock_Patch
    {
        public static void Postfix(BlockSignPost __instance, ref bool __result, ItemStack itemstack, BlockSelection bs)
        {
            if (Core.Config?.Signpost == false || !__result)
            {
                return;
            }
            (__instance?.GetBEBehavior<AttributeRenderingLibrary.BlockEntityBehaviorShapeTexturesFromAttributes>(bs.Position))?.OnBlockPlaced(itemstack);
        }
    }
}
