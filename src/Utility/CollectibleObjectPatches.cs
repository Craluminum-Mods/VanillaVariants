using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;
using Vintagestory.GameContent;
using Vintagestory.ServerMods;

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

    public static void PatchChest(this ICoreAPI api, Block block)
    {
        if (!Enum.TryParse(block?.Attributes?["chestType"]?.AsString(), out EnumChestType chestType))
        {
            return;
        }

        switch (chestType)
        {
            case EnumChestType.Normal:
                {
                    Block fromBlock = api.World.GetBlock(new AssetLocation("chest-east"));
                    if (fromBlock == null)
                    {
                        return;
                    }

                    string[] types = api.LoadTypesFromBlocks(block);
                    if (!types.Any())
                    {
                        return;
                    }

                    string genericType = "normal-generic";
                    block.Attributes ??= new JsonObject(new JObject());
                    block.Attributes.Token["types"] = JToken.FromObject(types);
                    block.Attributes.Token["defaultType"] = JToken.FromObject(types[0]);
                    block.Attributes.Token["rotatatableInterval"] = JToken.FromObject(types.ToDictionary(key => key, value => fromBlock.Attributes["rotatatableInterval"][genericType].AsString()));
                    block.Attributes.Token["drop"] = JToken.FromObject(types.ToDictionary(key => key, value => fromBlock.Attributes["drop"][genericType].AsBool()));

                    Dictionary<string, int> newQuantitySlots = types.ToDictionary(key => key, value => fromBlock.Attributes["quantitySlots"][genericType].AsInt());
                    if (Core.Config.OverrideChestQuantitySlots)
                    {
                        foreach (string type in types.Where(Core.Config.ChestQuantitySlots.ContainsKey))
                        {
                            newQuantitySlots[type] = Core.Config.ChestQuantitySlots[type];
                        }
                    }
                    block.Attributes.Token["quantitySlots"] = JToken.FromObject(newQuantitySlots);

                    block.Attributes.Token["dialogTitleLangCode"] = JToken.FromObject(types.ToDictionary(key => key, value => fromBlock.Attributes["dialogTitleLangCode"][genericType].AsString()));
                    block.Attributes.Token["storageType"] = JToken.FromObject(types.ToDictionary(key => key, value => fromBlock.Attributes["storageType"][genericType].AsInt()));
                    block.Attributes.Token["retrieveOnly"] = JToken.FromObject(types.ToDictionary(key => key, value => false));
                    block.Attributes.Token["shape"] = JToken.FromObject(types.ToDictionary(key => key, value => "game:" + fromBlock.Attributes["shape"][genericType].AsString()));
                    block.Attributes.Token["typedOpenSound"] = JToken.FromObject(types.ToDictionary(key => key, value => "game:" + fromBlock.Attributes["typedOpenSound"][genericType].AsString()));
                    block.Attributes.Token["typedCloseSound"] = JToken.FromObject(types.ToDictionary(key => key, value => "game:" + fromBlock.Attributes["typedCloseSound"][genericType].AsString()));

                    if (block.Variant["side"] == "east")
                    {
                        api.AddChestToCreativeInventory(block, types);
                    }
                    break;
                }
            case EnumChestType.NormalLabeled:
                {
                    Block fromBlock = api.World.GetBlock(new AssetLocation("labeledchest-east"));
                    if (fromBlock == null)
                    {
                        return;
                    }
                    string[] types = api.LoadTypesFromBlocks(block);
                    if (!types.Any())
                    {
                        return;
                    }

                    string genericType = "normal-labeled";
                    block.Attributes ??= new JsonObject(new JObject());
                    block.Attributes.Token["types"] = JToken.FromObject(types);
                    block.Attributes.Token["defaultType"] = JToken.FromObject(types[0]);
                    block.Attributes.Token["drop"] = JToken.FromObject(types.ToDictionary(key => key, value => fromBlock.Attributes["drop"][genericType].AsBool()));

                    Dictionary<string, int> newQuantitySlots = types.ToDictionary(key => key, value => fromBlock.Attributes["quantitySlots"][genericType].AsInt());
                    if (Core.Config.OverrideChestQuantitySlots)
                    {
                        foreach (string type in types.Where(Core.Config.ChestQuantitySlots.ContainsKey))
                        {
                            newQuantitySlots[type] = Core.Config.ChestQuantitySlots[type];
                        }
                    }
                    block.Attributes.Token["quantitySlots"] = JToken.FromObject(newQuantitySlots);

                    block.Attributes.Token["dialogTitleLangCode"] = JToken.FromObject(types.ToDictionary(key => key, value => fromBlock.Attributes["dialogTitleLangCode"][genericType].AsString()));
                    block.Attributes.Token["storageType"] = JToken.FromObject(types.ToDictionary(key => key, value => fromBlock.Attributes["storageType"][genericType].AsInt()));
                    block.Attributes.Token["retrieveOnly"] = JToken.FromObject(types.ToDictionary(key => key, value => false));
                    block.Attributes.Token["shape"] = JToken.FromObject(types.ToDictionary(key => key, value => "game:" + fromBlock.Attributes["shape"][genericType].AsString()));
                    block.Attributes.Token["typedOpenSound"] = JToken.FromObject(types.ToDictionary(key => key, value => "game:" + fromBlock.Attributes["typedOpenSound"][genericType].AsString()));
                    block.Attributes.Token["typedCloseSound"] = JToken.FromObject(types.ToDictionary(key => key, value => "game:" + fromBlock.Attributes["typedCloseSound"][genericType].AsString()));

                    if (block.Variant["side"] == "east")
                    {
                        api.AddChestToCreativeInventory(block, types);
                    }
                    break;
                }
            case EnumChestType.Double:
                {
                    Block fromBlock = api.World.GetBlock(new AssetLocation("trunk-east"));
                    if (fromBlock == null)
                    {
                        return;
                    }
                    string[] types = api.LoadTypesFromBlocks(block);
                    if (!types.Any())
                    {
                        return;
                    }

                    string genericType = "normal-generic";
                    block.Attributes ??= new JsonObject(new JObject());
                    block.Attributes.Token["types"] = JToken.FromObject(types);
                    block.Attributes.Token["defaultType"] = JToken.FromObject(types[0]);
                    block.Attributes.Token["rotatatableInterval"] = JToken.FromObject(types.ToDictionary(key => key, value => fromBlock.Attributes["rotatatableInterval"][genericType].AsString()));
                    block.Attributes.Token["drop"] = JToken.FromObject(types.ToDictionary(key => key, value => fromBlock.Attributes["drop"][genericType].AsBool()));

                    Dictionary<string, int> newQuantitySlots = types.ToDictionary(key => key, value => fromBlock.Attributes["quantitySlots"][genericType].AsInt());
                    if (Core.Config.OverrideDoubleChestQuantitySlots)
                    {
                        foreach (string type in types.Where(Core.Config.DoubleChestQuantitySlots.ContainsKey))
                        {
                            newQuantitySlots[type] = Core.Config.DoubleChestQuantitySlots[type];
                        }
                    }
                    block.Attributes.Token["quantitySlots"] = JToken.FromObject(newQuantitySlots);

                    block.Attributes.Token["quantityColumns"] = JToken.FromObject(types.ToDictionary(key => key, value => fromBlock.Attributes["quantityColumns"][genericType].AsInt()));
                    block.Attributes.Token["dialogTitleLangCode"] = JToken.FromObject(types.ToDictionary(key => key, value => fromBlock.Attributes["dialogTitleLangCode"][genericType].AsString()));
                    block.Attributes.Token["storageType"] = JToken.FromObject(types.ToDictionary(key => key, value => fromBlock.Attributes["storageType"][genericType].AsInt()));
                    block.Attributes.Token["retrieveOnly"] = JToken.FromObject(types.ToDictionary(key => key, value => false));
                    block.Attributes.Token["shape"] = JToken.FromObject(types.ToDictionary(key => key, value => "game:" + fromBlock.Attributes["shape"][genericType].AsString()));
                    block.Attributes.Token["typedOpenSound"] = JToken.FromObject(types.ToDictionary(key => key, value => "game:" + fromBlock.Attributes["typedOpenSound"][genericType].AsString()));
                    block.Attributes.Token["typedCloseSound"] = JToken.FromObject(types.ToDictionary(key => key, value => "game:" + fromBlock.Attributes["typedCloseSound"][genericType].AsString()));

                    if (block.Variant["side"] == "east")
                    {
                        api.AddChestToCreativeInventory(block, types);
                    }
                    break;
                }
        }
    }

    public static string[] LoadTypesFromFile(this ICoreAPI api, string assetName)
    {
        TypeProperties typeProps = api.Assets.TryGet(assetName)?.ToObject<TypeProperties>();
        if (typeProps == null)
        {
            return Array.Empty<string>();
        }
        string[] types = api.GetTypes(new RegistryObjectVariantGroup() { LoadFromProperties = typeProps.GetLoadFromProperties(), States = typeProps.States }, typeProps.SkipVariants);
        return types;
    }

    public static string[] LoadTypesFromBlocks(this ICoreAPI api, params Block[] blocks)
    {
        string[] types = Array.Empty<string>();
        foreach (Block block in blocks.Where(block => block != null))
        {
            types = types.Concat(api.LoadTypesFromFile(block.Attributes?["loadTypePropertiesFrom"]?.AsString())).ToArray();
        }
        return types;
    }

    public static string[] GetTypes(this ICoreAPI api, RegistryObjectVariantGroup variantGroups, string[] skipTypes)
    {
        string[] types = variantGroups?.States ?? Array.Empty<string>();
        if (variantGroups?.LoadFromProperties != null)
        {
            IAsset asset = api.Assets.TryGet(variantGroups.LoadFromProperties.WithPathPrefixOnce("worldproperties/").WithPathAppendixOnce(".json"));
            if (asset == null)
            {
                return Array.Empty<string>();
            }
            types = (asset?
                .ToObject<StandardWorldProperty>(null)).Variants
                .Select((WorldPropertyVariant p) => p.Code.Path)
                .Except(skipTypes ?? Array.Empty<string>())
                .ToArray()
                .Append(types);
            return types;
        }
        return Array.Empty<string>();
    }

    public static void AddChestToCreativeInventory(this ICoreAPI api, Block block, string[] types)
    {
        string[] tabs = block.Attributes?["creativeTabs"]?.AsObject<string[]>();
        if (tabs?.Any() == false)
        {
            return;
        }

        List<JsonItemStack> stacks = new();
        foreach (string type in types)
        {
            JsonItemStack jsonItemStack = new()
            {
                Code = block.Code,
                Type = block.ItemClass,
                Attributes = new JsonObject(JToken.Parse($"{{ \"type\": \"{type}\" }}"))
            };
            JsonItemStack jstack = jsonItemStack;
            jstack.Resolve(api.World, block.Code?.ToString() + " type");
            stacks.Add(jstack);
        }
        block.CreativeInventoryStacks = new CreativeTabAndStackList[1]
        {
            new()
            {
                Stacks = stacks.ToArray(),
                Tabs = tabs
            }
        };
    }

    public static void PatchChute(this Block block)
    {
        string name = block.Attributes?["configurableName"]?.AsString();
        string variant = block.Variant[block.Attributes?["configurableType"]?.AsString()];

        if (name == null || variant == null)
        {
            return;
        }

        UpdateAttribute(block, "item-flowrate", Core.Config.ChuteFlowRates, name, variant);
        UpdateAttribute(block, "quantitySlots", Core.Config.ChuteQuantitySlots, name, variant);
        UpdateAttribute(block, "item-checkrateMs", Core.Config.ChuteCheckRateMs, name, variant);
    }

    private static void UpdateAttribute<T>(Block block, string attributeName, Dictionary<string, Dictionary<string, T>> dict, string name, string variant)
    {
        if (!dict.TryGetValue(name, out Dictionary<string, T> innerDict))
        {
            return;
        }

        if (!innerDict.TryGetValue(variant, out T value) && !innerDict.TryGetValue("default", out value))
        {
            return;
        }

        block.Attributes.Token[attributeName] = JToken.FromObject(value);
    }

    // TODO: Without joking, I spent whole day to find perfect regex, I DESERVE to comment this until I find regex that will actually work!!
    // public static void PatchSteelProduction(this Block block)
    // {
    //     List<string> allowedTypes = Core.Config.MetalDoorsForSteelProduction.Where(x => x.Value).Select(keyVal => keyVal.Key).ToList();
    //     if (!allowedTypes.Any())
    //     {
    //         return;
    //     }

    //     string allowedTypesAsString = string.Join("|", allowedTypes);
    //     string newKey = $"@(game:irondoor-(.*)|vanvar:door-1x2metal-({allowedTypesAsString}))";
    //     JToken value = block.Attributes.Token["multiblockStructure"]["blockNumbers"]["irondoor-*"];
    //     block.Attributes.Token["multiblockStructure"]["blockNumbers"]["irondoor-*"].Remove();
    //     block.Attributes.Token["multiblockStructure"]["blockNumbers"][newKey] = JToken.FromObject(value);
    // }
}
