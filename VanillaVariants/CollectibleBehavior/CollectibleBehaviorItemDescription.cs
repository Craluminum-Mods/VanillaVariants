using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;

namespace VanillaVariants;

public class CollectibleBehaviorItemDescription : CollectibleBehavior
{
    private List<string> parts;

    public CollectibleBehaviorItemDescription(CollectibleObject collObj) : base(collObj) { }

    public override void Initialize(JsonObject properties)
    {
        base.Initialize(properties);

        parts = properties["parts"].AsObject<List<string>>();
        if (parts != null && parts.Any())
        {
            parts = parts.Select(x => Lang.GetMatching(x)).ToList();
        }
    }

    public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo) => ConstructDescription(dsc);

    private void ConstructDescription(StringBuilder sb)
    {
        if (parts != null && parts.Any())
        {
            sb.AppendLine();
            sb.AppendLine(string.Join("", parts));
        }
    }
}