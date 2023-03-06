using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace VanillaVariants
{
    public class BlockWoodChair : Block
    {
        public override string GetHeldItemName(ItemStack stack) => GetName();
        public override string GetPlacedBlockName(IWorldAccessor world, BlockPos pos) => GetName();

        public string GetName()
        {
            var wood = Lang.Get($"material-{Variant["type"]}");
            var color = Variant["color"];
            return string.Format($"{Lang.GetMatching($"game:block-chair-{color}")} ({wood})");
        }
    }
}