using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace VanillaVariants
{
    public class BlockWoodPan : BlockPan
    {
        public override string GetHeldItemName(ItemStack stack) => GetName();
        public override string GetPlacedBlockName(IWorldAccessor world, BlockPos pos) => GetName();

        public string GetName()
        {
            var material = Variant["type"];
            var part = Lang.Get($"material-{material}");
            return string.Format($"{Lang.Get("game:block-pan-wooden")} ({part})");
        }
    }
}