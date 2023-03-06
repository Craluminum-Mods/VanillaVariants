using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace VanillaVariants
{
    public class BlockStoneForge : BlockForge
    {
        public override string GetHeldItemName(ItemStack stack) => GetName();
        public override string GetPlacedBlockName(IWorldAccessor world, BlockPos pos) => GetName();

        public string GetName()
        {
            var material = Variant["rock"];
            var part = Lang.Get($"rock-{material}");
            return string.Format($"{Lang.Get("game:block-forge")} ({part})");
        }
    }
}