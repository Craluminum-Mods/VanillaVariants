using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.GameContent;

namespace VanillaVariants;

[HarmonyPatch]
public static class AttributeRenderingPatches
{
    [HarmonyPatchCategory("Client")]
    [HarmonyPatch(typeof(BlockEntityToolrack), nameof(BlockEntityToolrack.OnTesselation))]
    public static class BlockGenericTypedContainer_BlockEntityToolrack_Patch
    {
        public static bool Prefix(BlockEntityToolrack __instance, ref bool __result, ITerrainMeshPool mesher, ITesselatorAPI tesselator, MeshData[] ___toolMeshes)
        {
            for (int i = 0; i < __instance.Behaviors.Count; i++)
            {
                __instance.Behaviors[i].OnTesselation(mesher, tesselator);
            }

            for (int i = 0; i < 4; i++)
            {
                if (___toolMeshes[i] != null)
                {
                    mesher.AddMeshData(___toolMeshes[i]);
                }
            }

            return false;
        }
    }
}
