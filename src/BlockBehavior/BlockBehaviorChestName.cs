using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace VanillaVariants;

public class BlockBehaviorChestName : BlockBehavior
{
    private List<string> parts;
    private string replacePart;
    private string defaultName;

    public BlockBehaviorChestName(Block block) : base(block) { }

    public override void Initialize(JsonObject properties)
    {
        base.Initialize(properties);

        parts = properties["parts"].AsObject<List<string>>();
        replacePart = properties["replacePart"].AsString("");
        defaultName = properties["defaultName"].AsString("");
    }

    public override void GetPlacedBlockName(StringBuilder sb, IWorldAccessor world, BlockPos pos) => ConstructName(sb);

    public override void GetHeldItemName(StringBuilder sb, ItemStack itemStack) => ConstructName(sb);

    public string ConstructName(StringBuilder sb, ItemStack stack = null, IWorldAccessor world = null, BlockPos pos = null)
    {
        sb ??= new StringBuilder();

        string newName = Lang.GetMatching(defaultName);
        string type = GetType(stack, world, pos);

        if (parts != null && parts.Any())
        {
            if (!string.IsNullOrEmpty(type))
            {
                newName = string.Join("", parts.Select(x => Lang.GetMatching(x.Replace(replacePart, type))));
            }

            sb.Clear();
            sb.Append(newName);
            return sb.ToString();
        }

        return defaultName;
    }

    private static string GetType(ItemStack stack = null, IWorldAccessor world = null, BlockPos pos = null)
    {
        if (stack != null)
        {
            return stack.Attributes.GetString("type");
        }
        else if (world != null && pos != null && world.BlockAccessor.GetBlock(pos) is BlockGenericTypedContainer chest)
        {
            return chest.GetType(world.BlockAccessor, pos);
        }

        return null;
    }
}