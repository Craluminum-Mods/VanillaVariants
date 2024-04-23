using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent.Mechanics;

namespace VanillaVariants;

public static class BEPulverizer_OnTesselation_Patch
{
    public static bool Prefix(BEPulverizer __instance, ref bool __result, ITerrainMeshPool mesher, ITesselatorAPI tesselator)
    {
        float rotateY = __instance.GetField<float>("rotateY");

        BlockEntityDisplay_OnTesselation_ReversePatch.Base(__instance, mesher, tesselator);

        ICoreClientAPI capi = __instance.Api as ICoreClientAPI;
        MeshData meshTop = ObjectCacheUtil.GetOrCreate(capi, "pulverizertopmesh-" + __instance.Block.Code + rotateY, delegate
        {
            Shape shape2 = Shape.TryGet(capi, "shapes/block/wood/mechanics/pulverizer-top.json");
            capi.Tesselator.TesselateShape(__instance.Block, shape2, out MeshData modeldata2, new Vec3f(0f, rotateY, 0f));
            return modeldata2;
        });
        MeshData meshBase = ObjectCacheUtil.GetOrCreate(capi, "pulverizerbasemesh-" + __instance.Block.Code + rotateY, delegate
        {
            Shape shape = Shape.TryGet(capi, "shapes/block/wood/mechanics/pulverizer-base.json");
            capi.Tesselator.TesselateShape(__instance.Block, shape, out MeshData modeldata, new Vec3f(0f, rotateY, 0f));
            return modeldata;
        });
        mesher.AddMeshData(meshTop);
        mesher.AddMeshData(meshBase);
        for (int i = 0; i < __instance.Behaviors.Count; i++)
        {
            __instance.Behaviors[i].OnTesselation(mesher, tesselator);
        }
        __result = true;
        return false;
    }
}