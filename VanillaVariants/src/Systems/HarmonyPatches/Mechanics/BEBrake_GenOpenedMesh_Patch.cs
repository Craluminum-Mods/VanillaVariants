using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent.Mechanics;

namespace VanillaVariants;

public static class BEBrake_GenOpenedMesh_Patch
{
    public static bool Prefix(BEBrake __instance, ref MeshData __result, ITesselatorAPI tesselator, float rotY)
    {
        Dictionary<string, MeshData> meshes = ObjectCacheUtil.GetOrCreate(__instance.Api, "mechbrakeOpenedMesh" + __instance.Block.Code.ToString(), () => new Dictionary<string, MeshData>());
        if (meshes.TryGetValue(rotY.ToString() ?? "", out MeshData mesh))
        {
            __result = mesh;
            return false;
        }

        tesselator.TesselateShape(
            shape: Shape.TryGet(shapePath: AssetLocation.Create("shapes/block/wood/mechanics/brake-stand-opened.json", __instance.Block.Code.Domain), api: __instance.Api),
            textureSourceCollectible: __instance.Block,
            modeldata: out mesh,
            meshRotationDeg: new Vec3f(0f, rotY, 0f));

        __result = meshes[rotY.ToString() ?? ""] = mesh;
        return false;
    }
}
