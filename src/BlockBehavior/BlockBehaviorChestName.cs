using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        replacePart = properties.KeyExists("replacePart") ? properties["replacePart"].AsString() : "";
        defaultName = properties.KeyExists("defaultName") ? properties["defaultName"].AsString() : "";
    }

    public override void GetPlacedBlockName(StringBuilder sb, IWorldAccessor world, BlockPos pos) => ConstructName(sb);

    public override void GetHeldItemName(StringBuilder sb, ItemStack itemStack) => ConstructName(sb);

    public string ConstructName(StringBuilder sb, ItemStack stack = null, IWorldAccessor world = null, BlockPos pos = null)
    {
        sb ??= new StringBuilder();

        string newName = Lang.GetMatching(defaultName);

        if (parts != null && parts.Any())
        {
            if (string.IsNullOrEmpty(replacePart))
            {
                newName = string.Join("", parts.Select(x => Lang.GetMatching(x)));
                sb.Clear();
                sb.Append(newName);
                return sb.ToString();
            }
            else if (GetType(out string type, stack, world, pos))
            {
                newName = string.Join("", parts.Select(x => Lang.GetMatching(x.Replace(replacePart, type))));
                sb.Clear();
                sb.Append(newName);
                return sb.ToString();
            }
        }
        return newName;
    }

    private static bool GetType(out string type, ItemStack stack = null, IWorldAccessor world = null, BlockPos pos = null)
    {
        if (stack != null)
        {
            type = stack.Attributes.GetString("type");
            return true;
        }
        else if (world != null && pos != null && world.BlockAccessor.GetBlock(pos) is BlockGenericTypedContainer chest)
        {
            type = chest.GetType(world.BlockAccessor, pos);
            return true;
        }
        type = null;
        return false;
    }
}