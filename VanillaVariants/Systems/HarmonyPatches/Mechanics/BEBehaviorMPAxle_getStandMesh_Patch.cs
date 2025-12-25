using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Util;
using Vintagestory.GameContent.Mechanics;

namespace VanillaVariants;

public static class BEBehaviorMPAxle_getStandMesh_Patch
{
    public static bool Prefix(BEBehaviorMPAxle __instance, ref MeshData __result, string orient)
    {
        ICoreClientAPI capi = __instance.GetField<ICoreClientAPI>("capi");
        AssetLocation axleStandLocWest = __instance.GetField<AssetLocation>("axleStandLocWest");
        AssetLocation axleStandLocEast = __instance.GetField<AssetLocation>("axleStandLocEast");

        __result = ObjectCacheUtil.GetOrCreate(__instance.Api, __instance.Block.Code?.ToString() + "-" + orient + "-stand", delegate
        {
            Shape shape = Shape.TryGet(capi, (orient == "west") ? axleStandLocWest : axleStandLocEast);

            capi.Tesselator.TesselateShape(__instance.Block, shape, out MeshData modeldata);
            return modeldata;
        });

        return false;
    }
}