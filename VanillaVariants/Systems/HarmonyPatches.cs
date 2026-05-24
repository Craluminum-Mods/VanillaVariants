using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace VanillaVariants;

public class HarmonyPatches : ModSystem
{
    private Harmony HarmonyInstance => new Harmony(Mod.Info.ModID);

    public override void Start(ICoreAPI api)
    {
        if (!Harmony.HasAnyPatches(HarmonyInstance.Id))
        {
            if (Core.Config.ResolveChestNames)
            {
                HarmonyInstance.Patch(original: typeof(BlockGenericTypedContainer).GetMethod(nameof(BlockGenericTypedContainer.GetHeldItemName)), postfix: typeof(BlockGenericTypedContainer_GetHeldItemName_Patch).GetMethod(nameof(BlockGenericTypedContainer_GetHeldItemName_Patch.Postfix)));
            }
            HarmonyInstance.PatchCategory("Universal");
        }

        if (api.Side.IsClient())
        {
            HarmonyInstance.PatchCategory("Client");
        }
        if (api.Side.IsServer())
        {
            HarmonyInstance.PatchCategory("Server");
        }
    }

    public override void Dispose()
    {
        HarmonyInstance.UnpatchAll(HarmonyInstance.Id);
    }
}