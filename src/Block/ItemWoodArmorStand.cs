using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.GameContent;

namespace VanillaVariants
{
    public class ItemWoodArmorStand : ItemArmorStand
    {
        public override string GetHeldItemName(ItemStack stack) => GetName();

        public string GetName()
        {
            var material = Variant["wood"];
            var part = Lang.Get($"material-{material}");
            return $"{Lang.Get("game:item-armorstand")} ({part})";
        }
    }
}