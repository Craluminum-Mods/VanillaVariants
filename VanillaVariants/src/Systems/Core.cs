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
        IDictionary<string, CompositeTexture> largeTroughTextures = api.World.GetBlock(new AssetLocation("trough-genericwood-large-head-north")).Textures;
        IDictionary<string, CompositeTexture> smallTroughTextures = api.World.GetBlock(new AssetLocation("trough-genericwood-small-ns")).Textures;

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
                case BlockTroughDoubleBlock:
                    {
                        List<string> ignoreTextures = block.Attributes["ignoreTextures"].AsObject<List<string>>(new());

                        foreach ((string key, CompositeTexture val) in largeTroughTextures)
                        {
                            switch (block.Textures.ContainsKey(key))
                            {
                                case false:
                                    block.Textures.Add(key, val);
                                    break;
                                case true when !ignoreTextures.Contains(key):
                                    block.Textures[key] = val;
                                    break;
                            }
                        }

                        break;
                    }
                case BlockTrough when block.Code.ToString().Contains("small"):
                    {
                        List<string> ignoreTextures = block.Attributes["ignoreTextures"].AsObject<List<string>>(new());

                        foreach ((string key, CompositeTexture val) in smallTroughTextures)
                        {
                            switch (block.Textures.ContainsKey(key))
                            {
                                case false:
                                    block.Textures.Add(key, val);
                                    break;
                                case true when !ignoreTextures.Contains(key):
                                    block.Textures[key] = val;
                                    break;
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
}