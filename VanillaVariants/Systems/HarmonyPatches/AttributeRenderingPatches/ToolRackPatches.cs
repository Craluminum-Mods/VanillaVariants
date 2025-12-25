using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.GameContent;

namespace VanillaVariants;

[HarmonyPatch]
public static partial class AttributeRenderingPatches
{
    [HarmonyPatchCategory("Client")]
    [HarmonyPatch(typeof(BlockEntityToolrack), nameof(BlockEntityToolrack.OnTesselation))]
    public static class Toolrack_OnTesselation_Patch
    {
        public static bool Prefix(BlockEntityToolrack __instance, ref bool __result, ITerrainMeshPool mesher, ITesselatorAPI tesselator, MeshData[] ___toolMeshes)
        {
            if (Core.Config?.Toolrack == false)
            {
                return true;
            }

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
