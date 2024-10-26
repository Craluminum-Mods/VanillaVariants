using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using VanillaVariants.Configuration;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

[assembly: ModInfo(name: "Vanilla Variants", modID: "vanvar")]

namespace VanillaVariants;

public class Core : ModSystem
{
    public static Config Config { get; set; }

    public override void StartPre(ICoreAPI api)
    {
        Config = ModConfig.ReadConfig(api);

        if (api.ModLoader.IsModEnabled("configlib"))
        {
            _ = new ConfigLibCompatibility(api);
        }
    }

    public override void Start(ICoreAPI api)
    {
        api.RegisterBlockBehaviorClass("VanillaVariants.BlockName", typeof(BlockBehaviorName));
        api.RegisterCollectibleBehaviorClass("VanillaVariants.ItemName", typeof(CollectibleBehaviorName));

        api.RegisterCollectibleBehaviorClass("VanillaVariants.ModDescription", typeof(CollectibleBehaviorModDescription));

        api.RegisterBlockBehaviorClass("VanillaVariants.BlockDescription", typeof(BlockBehaviorBlockDescription));
        api.RegisterCollectibleBehaviorClass("VanillaVariants.ItemDescription", typeof(CollectibleBehaviorItemDescription));

        api.RegisterBlockBehaviorClass("VanillaVariants.ChestName", typeof(BlockBehaviorChestName));
        api.RegisterBlockBehaviorClass("VanillaVariants.ItemFlowDescription", typeof(BlockBehaviorItemFlowDescription));

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

        foreach (Block block in api.World.Blocks)
        {
            api.TryAddModDescription(block);

            switch (block)
            {
                case BlockPitkiln:
                    {
                        api.PatchPitKiln(block);
                        break;
                    }
                case BlockQuern:
                    {
                        if (Config.ResolveQuernAndAxleRelationship)
                        {
                            api.PatchQuern(block);
                        }
                        break;
                    }
                    // TODO: Why this code was commented? Read WHY in CollectibleObjectPatches.PatchSteelProduction
                    // case BlockStoneCoffinSection:
                    //     {
                    //         if (Config.MetalDoor && Config.OverrideMetalDoorsForSteelProduction)
                    //         {
                    //             block.PatchSteelProduction();
                    //         }
                    //         break;
                    //     }
            }

            if (Config.ResolveQuernAndAxleRelationship && block?.Attributes?["patchQuernExceptions"]?.AsBool() == true)
            {
                api.PatchQuern(block);
            }

            if (block?.Attributes?["chestType"]?.AsString() != null)
            {
                api.PatchChest(block);
            }

            if (!block.IsFromMod())
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

            if (block?.Attributes?["configurableChute"]?.AsBool() == true)
            {
                block.PatchChute();
            }
        }

        foreach (Item item in api.World.Items)
        {
            api.TryAddModDescription(item);
        }
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