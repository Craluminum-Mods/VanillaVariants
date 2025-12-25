using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace VanillaVariants;

public class CollectibleBehaviorModDescription : CollectibleBehavior
{
    public CollectibleBehaviorModDescription(CollectibleObject collObj) : base(collObj) { }

    public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
    {
        dsc.AppendLine(Lang.Get("Mod: {0}", world.Api.ModLoader.GetMod("vanvar").Info.Name));
    }
}