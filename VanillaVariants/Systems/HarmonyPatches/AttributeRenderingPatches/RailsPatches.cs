using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace VanillaVariants;

[HarmonyPatch]
public static partial class AttributeRenderingPatches
{
    /// <summary>
    /// Exactly the same as BlockRails.TryPlaceBlock, but with corrected TryAttachPlaceToHoriontal that calls DoPlaceBlock returning itemstack.
    /// DoPlaceBlock should always return itemstack, otherwise attribute rendering won't work
    /// </summary>
    [HarmonyPatchCategory("Universal")]
    [HarmonyPatch(typeof(BlockRails), nameof(BlockRails.TryPlaceBlock))]
    public static class Rails_TryPlaceBlock_Patch
    {
        public static bool Prefix(BlockRails __instance, ref bool __result, IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
        {
            if (Core.Config?.WoodenRails == false)
            {
                return true;
            }

            if (!__instance.CanPlaceBlock(world, byPlayer, blockSel, ref failureCode))
            {
                return true;
            }

            BlockFacing blockFacing = Block.SuggestedHVOrientation(byPlayer, blockSel)[0];
            Block block = null;
            for (int i = 0; i < BlockFacing.HORIZONTALS.Length; i++)
            {
                BlockFacing toFacing = BlockFacing.HORIZONTALS[i];
                if (TryAttachPlaceToHoriontal(__instance, world, byPlayer, blockSel.Position, toFacing, blockFacing, itemstack))
                {
                    __result = true;
                    return false;
                }
            }

            block ??= (blockFacing.Axis != EnumAxis.Z) ? world.GetBlock(__instance.CodeWithParts("flat_we")) : world.GetBlock(__instance.CodeWithParts("flat_ns"));

            block.DoPlaceBlock(world, byPlayer, blockSel, itemstack);
            __result = true;
            return false;
        }
    }

    /// <summary>
    /// Same as BlockRails.TryAttachPlaceToHoriontal, but with DoPlaceBlock returning itemstack.
    /// DoPlaceBlock should always return itemstack, otherwise attribute rendering won't work
    /// </summary>
    private static bool TryAttachPlaceToHoriontal(BlockRails block, IWorldAccessor world, IPlayer byPlayer, BlockPos position, BlockFacing toFacing, BlockFacing targetFacing, ItemStack byItemstack)
    {
        BlockPos blockPos = position.AddCopy(toFacing);
        Block placedBlock = world.BlockAccessor.GetBlock(blockPos);
        if (placedBlock is not BlockRails)
        {
            return false;
        }

        BlockFacing opposite = toFacing.Opposite;
        BlockFacing[] facingsFromType = block.CallMethod<BlockFacing[]>("getFacingsFromType", placedBlock.Variant["type"]);
        if (world.BlockAccessor.GetBlock(blockPos.AddCopy(facingsFromType[0])) is BlockRails && world.BlockAccessor.GetBlock(blockPos.AddCopy(facingsFromType[1])) is BlockRails)
        {
            return false;
        }

        BlockFacing openedEndedFace = block.CallMethod<BlockFacing>("getOpenedEndedFace", facingsFromType, world, position.AddCopy(toFacing));
        if (openedEndedFace == null)
        {
            return false;
        }

        Block railBlock = block.CallMethod<Block>("getRailBlock", world, "curved_", toFacing, targetFacing);
        if (railBlock != null)
        {
            return placeIfSuitable(world, byPlayer, railBlock, position, byItemstack);
        }

        string text = placedBlock.Variant["type"].Split('_')[1];
        BlockFacing dir = (text[0] == openedEndedFace.Code[0]) ? BlockFacing.FromFirstLetter(text[1]) : BlockFacing.FromFirstLetter(text[0]);
        Block railBlock2 = block.CallMethod<Block>("getRailBlock", world, "curved_", dir, opposite);
        if (railBlock2 == null)
        {
            return false;
        }

        // it is very important to set previous variants here
        ItemStack newItemstack2 = byItemstack.Clone();
        BlockPos newPos = position.AddCopy(toFacing);
        AttributeRenderingLibrary.Variants variants2 = world.BlockAccessor.GetBlockEntity(newPos)?.GetBehavior<AttributeRenderingLibrary.BlockEntityBehaviorShapeTexturesFromAttributes>()?.Variants;
        variants2?.ToStack(newItemstack2);

        railBlock2.DoPlaceBlock(world, byPlayer, new BlockSelection
        {
            Position = newPos,
            Face = BlockFacing.UP
        }, newItemstack2);
        return false;
    }

    /// <summary>
    /// Same as BlockRails.placeIfSuitable, but with DoPlaceBlock returning itemstack.
    /// DoPlaceBlock should always return itemstack, otherwise attribute rendering won't work
    /// </summary>
    private static bool placeIfSuitable(IWorldAccessor world, IPlayer byPlayer, Block block, BlockPos pos, ItemStack byItemstack)
    {
        string failureCode = "";
        BlockSelection blockSel = new BlockSelection
        {
            Position = pos,
            Face = BlockFacing.UP
        };
        if (block.CanPlaceBlock(world, byPlayer, blockSel, ref failureCode))
        {
            block.DoPlaceBlock(world, byPlayer, blockSel, byItemstack);
            return true;
        }
        return false;
    }
}