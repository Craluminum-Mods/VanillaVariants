using Vintagestory.API.Config;
using Vintagestory.GameContent;

namespace VanillaVariants
{
    public class EntityWoodArmorStand : EntityArmorStand
    {
        public override string GetName()
        {
            var material = Code.EndVariant();
            var part = Lang.Get($"material-{material}");
            return $"{Lang.Get("game:item-armorstand")} ({part})";
        }
    }
}