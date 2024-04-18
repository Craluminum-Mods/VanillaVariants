using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using VanillaVariants.Configuration;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

[assembly: ModInfo(name: "Vanilla Variants", modID: "vanvar")]

namespace VanillaVariants;

public class Core : ModSystem
{
    public static Config Config { get; private set; }

    public override void StartPre(ICoreAPI api) => Config = ModConfig.ReadConfig(api);

    public override void Start(ICoreAPI api)
    {
        api.RegisterBlockBehaviorClass("VanillaVariants.BbName", typeof(BlockBehaviorName));
        api.RegisterCollectibleBehaviorClass("VanillaVariants.CbName", typeof(CollectibleBehaviorName));
        api.RegisterCollectibleBehaviorClass("VanillaVariants.ModDescription", typeof(CollectibleBehaviorModDescription));

        api.RegisterBlockClass("VanillaVariants.BlockWoodBucket", typeof(BlockWoodBucket));
        api.RegisterEntity("VV_EntityWoodArmorStand", typeof(EntityWoodArmorStand));

        api.World.Logger.Event("started '{0}' mod", Mod.Info.Name);
    }

    public override void AssetsFinalize(ICoreAPI api) => ApplyPatches(api);

    private static void ApplyPatches(ICoreAPI api)
    {
        GetDefaultPanAttributes(api, out object panningDrops);
        GetDefaultLargeTroughAttributes(api, out object largeContentConfig, out object largeUnsuitableFor, out IDictionary<string, CompositeTexture> largeTroughTextures);
        GetDefaultSmallTroughAttributes(api, out object smallContentConfig, out object smallUnsuitableFor, out IDictionary<string, CompositeTexture> smallTroughTextures);
        GetDefaultPitKilnAttributes(api, out JsonItemStackBuildStage[] fuel);

        foreach (Block block in api.World.Blocks)
        {
            switch (block)
            {
                case BlockPitkiln:
                    {
                        PatchPitKiln(api, block);
                        break;
                    }
            }

            bool isFromMod = block?.Attributes?["fromVanVarMod"]?.AsBool() == true;
            if (api.Side.IsServer() && isFromMod)
            {
                block.CollectibleBehaviors = block.CollectibleBehaviors.Append(new CollectibleBehaviorModDescription(block));
            }

            if (block?.Code?.Domain != "vanvar" && !isFromMod)
            {
                continue;
            }

            switch (block)
            {
                case BlockPan:
                    {
                        block.Attributes.Token["panningDrops"] = JToken.FromObject(panningDrops);
                        break;
                    }
                case BlockTroughDoubleBlock:
                    {
                        if (largeContentConfig != null) block.Attributes.Token["contentConfig"] = JToken.FromObject(largeContentConfig);
                        if (largeUnsuitableFor != null) block.Attributes.Token["unsuitableFor"] = JToken.FromObject(largeUnsuitableFor);

                        foreach ((string key, CompositeTexture val) in largeTroughTextures)
                        {
                            if (!block.Textures.ContainsKey(key))
                            {
                                block.Textures.Add(key, val);
                            }
                        }

                        break;
                    }
                case BlockTrough when block.Code.ToString().Contains("small"):
                    {
                        if (smallContentConfig != null) block.Attributes.Token["contentConfig"] = JToken.FromObject(smallContentConfig);
                        if (smallUnsuitableFor != null) block.Attributes.Token["unsuitableFor"] = JToken.FromObject(smallUnsuitableFor);

                        foreach ((string key, CompositeTexture val) in smallTroughTextures)
                        {
                            if (!block.Textures.ContainsKey(key))
                            {
                                block.Textures.Add(key, val);
                            }
                        }

                        break;
                    }
            }
        }

        foreach (Item item in api.World.Items)
        {
            bool isFromMod = item?.Attributes?["fromVanVarMod"]?.AsBool() == true;
            if (api.Side.IsServer() && isFromMod)
            {
                item.CollectibleBehaviors = item.CollectibleBehaviors.Append(new CollectibleBehaviorModDescription(item));
            }
        }
    }

    private static void PatchPitKiln(ICoreAPI api, Block block)
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

    private static void GetDefaultPitKilnAttributes(ICoreAPI api, out JsonItemStackBuildStage[] fuel)
    {
        Block block = api.World.GetBlock(new AssetLocation("pitkiln"));
        fuel = block?.Attributes?["buildMats"]?["fuel"]?.AsObject<JsonItemStackBuildStage[]>();
    }

    private static void GetDefaultPanAttributes(ICoreAPI api, out object panningDrops)
    {
        Block block = api.World.GetBlock(new AssetLocation("pan-wooden"));
        panningDrops = block?.Attributes?["panningDrops"]?.AsObject<object>();
    }

    private static void GetDefaultLargeTroughAttributes(ICoreAPI api, out object largeContentConfig, out object largeUnsuitableFor, out IDictionary<string, CompositeTexture> textures)
    {
        Block block = api.World.GetBlock(new AssetLocation("trough-genericwood-large-head-north"));
        largeContentConfig = block?.Attributes?["contentConfig"]?.AsObject<object>();
        largeUnsuitableFor = block?.Attributes?["unsuitableFor"]?.AsObject<object>();
        textures = block.Textures;
    }

    private static void GetDefaultSmallTroughAttributes(ICoreAPI api, out object smallContentConfig, out object smallUnsuitableFor, out IDictionary<string, CompositeTexture> textures)
    {
        Block block = api.World.GetBlock(new AssetLocation("trough-genericwood-small-ns"));
        smallContentConfig = block?.Attributes?["contentConfig"]?.AsObject<object>();
        smallUnsuitableFor = block?.Attributes?["unsuitableFor"]?.AsObject<object>();
        textures = block.Textures;
    }
}