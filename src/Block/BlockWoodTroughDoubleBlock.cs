using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace VanillaVariants
{
    public class BlockWoodTroughDoubleBlock : BlockTroughDoubleBlock
    {
        public override string GetHeldItemName(ItemStack stack) => GetName();
        public override string GetPlacedBlockName(IWorldAccessor world, BlockPos pos) => GetName();

        public string GetName()
        {
            var material = Variant["material"];
            var part = Lang.Get($"material-{material}");
            return string.Format($"{Lang.GetMatching("game:block-trough-genericwood-large-head-*")} ({part})");
        }
    }
}