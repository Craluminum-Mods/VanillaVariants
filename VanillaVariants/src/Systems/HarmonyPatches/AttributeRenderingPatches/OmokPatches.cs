using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace VanillaVariants;

[HarmonyPatch]
public static partial class AttributeRenderingPatches
{
    [HarmonyPatchCategory("Client")]
    [HarmonyPatch(typeof(BlockEntityOmokTable), nameof(BlockEntityOmokTable.OnTesselation))]
    public static class Omok_OnTesselation_Patch
    {
        public static bool Prefix(BlockEntityOmokTable __instance, ref bool __result, ITerrainMeshPool mesher, ITesselatorAPI tessThreadTesselator, int ___size)
        {
            if (Core.Config?.OmokTabletop == false)
            {
                return true;
            }
            Matrixf mat = new Matrixf();
            for (int i = 0; i < ___size * ___size; i++)
            {
                ItemSlot slot = __instance.Inventory[i];
                if (!slot.Empty)
                {
                    mat.Identity();
                    int dx = i % ___size;
                    mat.Translate(z: (0.6f + (float)(i / ___size)) / 16f, x: (0.6f + (float)dx) / 16f, y: 0f);
                    mesher.AddMeshData(__instance.CallMethod<MeshData>("getMesh", slot.Itemstack), mat.Values);
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
}
