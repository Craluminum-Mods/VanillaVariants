using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent.Mechanics;

namespace VanillaVariants;

public static class BEBehaviorMPArchimedesScrew_getHullMesh_Patch
{
    public static bool Prefix(BEBehaviorMPArchimedesScrew __instance, ref MeshData __result)
    {
        ICoreClientAPI capi = __instance.GetField<ICoreClientAPI>("capi");

        CompositeShape cshape = __instance.properties["staticShapePart"].AsObject<CompositeShape>(null, __instance.Block.Code.Domain);
        if (cshape == null)
        {
            __result = null;
            return false;
        }
        cshape.Base.WithPathPrefixOnce("shapes/").WithPathAppendixOnce(".json");
        __result = ObjectCacheUtil.GetOrCreate(__instance.Api, "archimedesscrew-mesh-" + __instance.Block.Code + cshape.Base.Path + "-" + cshape.rotateX + "-" + cshape.rotateY + "-" + cshape.rotateZ, delegate
        {
            Shape shape = Shape.TryGet(capi, cshape.Base);
            capi.Tesselator.TesselateShape(__instance.Block, shape, out var modeldata);
            modeldata.Rotate(new Vec3f(0.5f, 0.5f, 0.5f), cshape.rotateX * ((float)Math.PI / 180f), cshape.rotateY * ((float)Math.PI / 180f), cshape.rotateZ * ((float)Math.PI / 180f));
            return modeldata;
        });

        return false;
    }
}
