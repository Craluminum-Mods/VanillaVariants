using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace VanillaVariants;

public static class CollectibleObjectPatches
{
    public static bool HasFromModAttribute(this CollectibleObject obj)
    {
        return obj?.Attributes?["fromVanVarMod"]?.AsBool() == true;
    }

    public static bool IsFromMod(this CollectibleObject obj)
    {
        return obj?.Code?.Domain == "vanvar" || HasFromModAttribute(obj);
    }

    public static void TryAddModDescription(this ICoreAPI api, CollectibleObject obj)
    {
        if (api.Side.IsServer() && HasFromModAttribute(obj))
        {
            obj.CollectibleBehaviors = obj.CollectibleBehaviors.Append(new CollectibleBehaviorModDescription(obj));
        }
    }

    public static void PatchPitKiln(this ICoreAPI api, Block block)
    {
        Item[] firewoodItems = Array.Empty<Item>();
        firewoodItems = firewoodItems
            .Concat(api.World.SearchItems(new AssetLocation("vanvar:firewood-*")))
            .Concat(api.World.SearchItems(new AssetLocation("wildcrafttree:firewood-*")))
            .ToArray();

        if (!firewoodItems.Any())
        {
            return;
        }

        JsonItemStackBuildStage[] fuel = block?.Attributes?["buildMats"]?["fuel"]?.AsObject<JsonItemStackBuildStage[]>();
        if (fuel == null)
        {
            return;
        }

        JsonItemStackBuildStage firewoodJsonStack = fuel.FirstOrDefault(x => x.Code.ToString().Contains("firewood"));
        if (firewoodJsonStack == null)
        {
            return;
        }

        foreach (Item item in firewoodItems)
        {
            JsonItemStackBuildStage newJsonStack = new()
            {
                Type = item.ItemClass,
                Code = item.Code,
                Quantity = firewoodJsonStack.Quantity,
                EleCode = firewoodJsonStack.EleCode,
                BurnTimeHours = firewoodJsonStack.BurnTimeHours
            };

            fuel = fuel.Append(newJsonStack);
        }

        block.Attributes.Token["buildMats"]["fuel"] = JToken.FromObject(fuel);
    }

    public static void PatchQuern(this ICoreAPI api, Block block)
    {
        BlockBehaviorUnstableFalling behavior = block.GetBehavior<BlockBehaviorUnstableFalling>();
        JsonObject properties = new JsonObject(JObject.Parse(behavior.propertiesAtString));
        AssetLocation[] exceptions = properties?["exceptions"]?.AsObject(new AssetLocation[0], block.Code.Domain);
        if (exceptions == null)
        {
            return;
        }

        Block[] axleBlocks = Array.Empty<Block>();
        axleBlocks = axleBlocks
            .Concat(api.World.SearchBlocks(new AssetLocation("vanvar:woodenaxle-*-ud")))
            .Concat(api.World.SearchBlocks(new AssetLocation("wildcrafttree:woodenaxle-*-ud")))
            .ToArray();

        if (!axleBlocks.Any())
        {
            return;
        }

        block.BlockBehaviors = block.BlockBehaviors.Remove(behavior);
        block.CollectibleBehaviors = block.CollectibleBehaviors.Remove(behavior);

        exceptions = exceptions.Concat(axleBlocks.Select(x => x.Code)).ToArray();
        properties.Token["exceptions"] = JToken.FromObject(exceptions);
        behavior.Initialize(properties);

        block.BlockBehaviors = block.BlockBehaviors.Append(behavior);
        block.CollectibleBehaviors = block.CollectibleBehaviors.Append(behavior);
    }
}