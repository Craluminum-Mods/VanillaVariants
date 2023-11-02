using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace VanillaVariants;

public class BlockBehaviorName : BlockBehavior
{
    private string translationName;
    private readonly List<string> translationParts = new();
    private string newName;

    public BlockBehaviorName(Block block) : base(block) { }

    public override void Initialize(JsonObject properties)
    {
        base.Initialize(properties);

        if (properties["translationName"].Exists)
        {
            translationName = properties["translationName"].AsString();
        }
        if (properties["translationParts"].Exists)
        {
            translationParts.AddRange(properties["translationParts"].AsObject<List<string>>());
        }
        else if (properties["translationPart"].Exists)
        {
            translationParts.Add(properties["translationPart"].AsString());
        }

        newName = Lang.GetMatchingIfExists(translationName);

        if (translationParts?.Count != 0)
        {
            newName += " " + string.Join(" ", translationParts.ConvertAll(x => $"({Lang.Get(x)})"));
        }
    }

    public override void GetHeldItemName(StringBuilder sb, ItemStack itemStack)
    {
        sb.Clear();
        sb.Append(newName);
    }

    public override void GetPlacedBlockName(StringBuilder sb, IWorldAccessor world, BlockPos pos)
    {
        sb.Clear();
        sb.Append(newName);
    }
}