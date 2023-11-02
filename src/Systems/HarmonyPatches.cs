using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace VanillaVariants;

public class HarmonyPatches : ModSystem
{
    public const string HarmonyID = "craluminum2413.vanvar";

    public override void Start(ICoreAPI api)
    {
        base.Start(api);
        if (Core.Config.ExperimentalOverlayTest)
        {
            new Harmony(HarmonyID).Patch(original: typeof(ColorBlend).GetMethod(nameof(ColorBlend.Overlay)), prefix: typeof(Overlay_Patch).GetMethod(nameof(Overlay_Patch.Prefix)));
        }
    }

    public override void Dispose()
    {
        if (Core.Config.ExperimentalOverlayTest)
        {
            new Harmony(HarmonyID).Unpatch(original: typeof(ColorBlend).GetMethod(nameof(ColorBlend.Overlay)), HarmonyPatchType.All, HarmonyID);
        }
        base.Dispose();
    }
}