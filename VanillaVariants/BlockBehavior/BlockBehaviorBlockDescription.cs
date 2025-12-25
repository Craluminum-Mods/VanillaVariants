using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace VanillaVariants;

public class BlockBehaviorBlockDescription : BlockBehavior
{
    private List<string> parts;

    public BlockBehaviorBlockDescription(Block block) : base(block) { }

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

    public override string GetPlacedBlockInfo(IWorldAccessor world, BlockPos pos, IPlayer forPlayer) => ConstructDescription();

    private void ConstructDescription(StringBuilder sb)
    {
        if (parts != null && parts.Any())
        {
            sb.AppendLine();
            sb.AppendLine(string.Join("", parts));
        }
    }

    private string ConstructDescription()
    {
        StringBuilder sb = new();
        if (parts != null && parts.Any())
        {
            sb.Append(string.Join("", parts));
        }
        return sb.ToString();
    }
}