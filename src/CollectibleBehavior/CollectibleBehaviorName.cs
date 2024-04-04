using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;

namespace VanillaVariants;

public class CollectibleBehaviorName : CollectibleBehavior
{
    private List<string> parts;

    public CollectibleBehaviorName(CollectibleObject collObj) : base(collObj) { }

    public override void Initialize(JsonObject properties)
    {
        base.Initialize(properties);

        parts = properties["parts"].AsObject<List<string>>();
        if (parts != null && parts.Count != 0)
        {
            parts = parts.Select(x => Lang.GetMatching(x)).ToList();
        }
    }

    public override void GetHeldItemName(StringBuilder sb, ItemStack itemStack) => ConstructName(sb);

    private void ConstructName(StringBuilder sb)
    {
        if (parts == null || parts.Count == 0)
        {
            return;
        }

        sb.Clear();
        sb.Append(string.Join("", parts));
    }
}