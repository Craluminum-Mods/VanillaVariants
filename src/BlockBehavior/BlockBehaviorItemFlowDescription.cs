using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace VanillaVariants;

public class BlockBehaviorItemFlowDescription : BlockBehavior
{
    public BlockBehaviorItemFlowDescription(Block block) : base(block) { }

    public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo) => ConstructDescription(world, dsc);

    public override string GetPlacedBlockInfo(IWorldAccessor world, BlockPos pos, IPlayer forPlayer) => ConstructDescription(world);

    private string ConstructDescription(IWorldAccessor world, StringBuilder sb = null)
    {
        sb ??= new StringBuilder();
        if (world.Api is ICoreClientAPI capi && capi.World.Player.WorldData.CurrentGameMode == EnumGameMode.Creative)
        {
            sb.AppendLine(Lang.Get("item-flowrate") + ": " + block?.Attributes?["item-flowrate"]?.AsFloat());
            sb.AppendLine(Lang.Get("item-checkrateMs") + ": " + block?.Attributes?["item-checkrateMs"]?.AsInt());
            sb.AppendLine(Lang.Get("quantitySlots") + ": " + block?.Attributes?["quantitySlots"]?.AsInt());
        }
        return sb.ToString();
    }
}