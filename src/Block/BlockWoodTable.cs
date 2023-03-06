using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace VanillaVariants
{
    public class BlockWoodTable : Block
    {
        public override string GetHeldItemName(ItemStack stack) => GetName();
        public override string GetPlacedBlockName(IWorldAccessor world, BlockPos pos) => GetName();

        public string GetName()
        {
            var material = Variant["material"];
            var type = Variant["type"];
            var part = Lang.Get($"material-{material}");
            return string.Format($"{Lang.GetMatching($"game:block-table-{type}")} ({part})");
        }
    }
}