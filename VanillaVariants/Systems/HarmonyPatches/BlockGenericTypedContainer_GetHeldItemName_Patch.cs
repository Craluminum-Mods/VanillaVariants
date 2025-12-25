using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace VanillaVariants;

public static class BlockGenericTypedContainer_GetHeldItemName_Patch
{
    public static void Postfix(BlockGenericTypedContainer __instance, ref string __result, ItemStack itemStack)
    {
        BlockBehaviorChestName behavior = __instance.GetBehavior<BlockBehaviorChestName>();
        if (behavior != null)
        {
            __result = behavior.ConstructName(null, itemStack);
        }
    }
}