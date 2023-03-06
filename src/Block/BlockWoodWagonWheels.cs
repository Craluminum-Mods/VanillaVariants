using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace VanillaVariants
{
    public class BlockWoodWagonWheels : Block
    {
        public override string GetHeldItemName(ItemStack stack) => GetName();
        public override string GetPlacedBlockName(IWorldAccessor world, BlockPos pos) => GetName();

        public string GetName()
        {
            var type = Variant["type"];

            if (type == "spoked-longaxle") type = "spoked";
            if (type == "solid-longaxle") type = "solid";

            var material = Variant["material"];
            var part = Lang.Get($"material-{material}");
            return string.Format($"{Lang.GetMatching($"game:block-wagonwheels-{type}-*")} ({part})");
        }
    }
}