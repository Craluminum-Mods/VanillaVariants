using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace VanillaVariants;

public class BlockBehaviorName : BlockBehavior
{
    private string translationName;
    private string translationPart;
    private string newName;

    public BlockBehaviorName(Block block) : base(block) { }

    public override void Initialize(JsonObject properties)
    {
        base.Initialize(properties);

        translationName = properties["translationName"].AsString();
        translationPart = properties["translationPart"].AsString();

        newName = Lang.GetMatchingIfExists(translationName) + " (" + Lang.Get(translationPart) + ")";
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