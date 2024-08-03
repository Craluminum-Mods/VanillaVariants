using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.GameContent;

namespace VanillaVariants;

public static class BlockEntityHenBox_TryAddEgg_Patch
{
    public static bool Prefix(BlockEntityHenBox __instance, ref bool __result, Entity entity, string chickCode, double incubationTime)
    {
        string fullCode = __instance.GetField<string>("fullCode");
        double timeToIncubate = __instance.GetField<double>("timeToIncubate");
        double occupiedTimeLast = __instance.GetField<double>("occupiedTimeLast");
        int[] parentGenerations = __instance.GetField<int[]>("parentGenerations");
        AssetLocation[] chickNames = __instance.GetField<AssetLocation[]>("chickNames");

        if (__instance.Block.LastCodePart() == fullCode)
        {
            if (timeToIncubate == 0.0)
            {
                timeToIncubate = incubationTime;
                occupiedTimeLast = entity.World.Calendar.TotalDays;
            }

            __instance.MarkDirty();
            __result = false;
            return false;
        }

        timeToIncubate = 0.0;
        int eggs = __instance.CallMethod<int>("CountEggs");
        parentGenerations[eggs] = entity.WatchedAttributes.GetInt("generation");
        chickNames[eggs] = (chickCode == null) ? null : entity.Code.CopyWithPath(chickCode);
        eggs++;

        Block replacementBlock = __instance.Api.World.GetBlock(__instance.Block.CodeWithVariant("eggCount", eggs + ((eggs > 1) ? "eggs" : "egg")));

        if (replacementBlock == null)
        {
            __instance.Api.Logger.Error("[Vanilla Variants] HenboxFixed: Can't update henbox variant for {0} block", __instance.Block.Code.ToString());
            __result = false;
            return false;
        }

        __instance.Api.World.BlockAccessor.ExchangeBlock(replacementBlock.Id, __instance.Pos);
        __instance.Block = replacementBlock;
        __instance.MarkDirty();
        __result = true;
        return false;
    }
}