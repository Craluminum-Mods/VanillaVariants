using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent.Mechanics;

namespace VanillaVariants;

public static class MechNetworkRenderer_AddDevice_Patch
{
    public static bool Prefix(MechNetworkRenderer __instance, IMechanicalPowerRenderable device)
    {
        MechanicalPowerMod mechanicalPowerMod = __instance.GetField<MechanicalPowerMod>("mechanicalPowerMod");
        ICoreClientAPI capi = __instance.GetField<ICoreClientAPI>("capi");
        IShaderProgram prog = __instance.GetField<IShaderProgram>("prog");
        List<MechBlockRenderer> MechBlockRenderer = __instance.GetField<List<MechBlockRenderer>>("MechBlockRenderer");
        Dictionary<int, int> MechBlockRendererByShape = __instance.GetField<Dictionary<int, int>>("MechBlockRendererByShape");

        if (device.Shape != null)
        {
            int index = -1;
            string rendererCode = "generic";
            JsonObject attributes = device.Block.Attributes;
            if (attributes != null && attributes["mechanicalPower"]?["renderer"].Exists == true)
            {
                rendererCode = device.Block.Attributes?["mechanicalPower"]?["renderer"].AsString("generic");
            }
            int hashCode = device.Block.Code.ToString().GetHashCode() + device.Shape.GetHashCode() + rendererCode.GetHashCode();
            if (!MechBlockRendererByShape.TryGetValue(hashCode, out index))
            {
                object obj = Activator.CreateInstance(MechNetworkRenderer.RendererByCode[rendererCode], capi, mechanicalPowerMod, device.Block, device.Shape);
                MechBlockRenderer.Add((MechBlockRenderer)obj);
                index = MechBlockRendererByShape[hashCode] = MechBlockRenderer.Count - 1;
            }
            MechBlockRenderer[index].AddDevice(device);
        }

        return false;
    }
}
