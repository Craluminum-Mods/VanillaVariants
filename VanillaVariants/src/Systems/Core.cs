using AttributeRenderingLibrary;
using System.Collections.Generic;
using VanillaVariants.Configuration;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

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

        api.RegisterEntity("VV_EntityWoodArmorStand", typeof(EntityWoodArmorStand));

        api.World.Logger.Event("started '{0}' mod", Mod.Info.Name);
    }

    public override void AssetsFinalize(ICoreAPI api)
    {
        IDictionary<string, CompositeTexture> smallTroughTextures = api.World.GetBlock(new AssetLocation("trough-genericwood-small-ns")).Textures;
        IDictionary<string, CompositeTexture> largeTroughTextures = api.World.GetBlock(new AssetLocation("trough-genericwood-large-head-north")).Textures;

        foreach (Block block in api.World.Blocks)
        {
            api.TryAddModDescription(block);
            //block.PatchSteelProduction(); // TODO: Why this code was commented? Read WHY in CollectibleObjectPatches.PatchSteelProduction
            api.PatchQuern(block);
            api.PatchChest(block);
            block.PatchTrough(smallTroughTextures, largeTroughTextures);
            block.PatchChute();

            if (api.Side.IsServer())
            {
                PatchWithBehavior(block);
            }
        }

        foreach (Item item in api.World.Items)
        {
            api.TryAddModDescription(item);

            if (api.Side.IsServer())
            {
                PatchWithBehavior(item);
            }
        }
    }

    private void PatchWithBehavior(CollectibleObject obj)
    {
        AddBehaviorWithPropertiesIfTrue(Config.Toolrack && obj is BlockToolRack && obj.Code.Domain == "game", obj, toolrackProps);
    }

    private void AddBehaviorWithPropertiesIfTrue(bool condition, CollectibleObject obj, JsonObject props)
    {
        if (!condition) return;
        if (props == null) return;
        if (obj is Block block)
        {
            BlockBehaviorShapeTexturesFromAttributes behavior = new(block);
            behavior.Initialize(props);
            block.CollectibleBehaviors = block.CollectibleBehaviors.Append(behavior);
            block.BlockBehaviors = block.BlockBehaviors.Append(behavior);

            BlockEntityBehaviorType bebehavior = new BlockEntityBehaviorType
            {
                Name = "AttributeRenderingLibrary.ShapeTexturesFromAttributes",
                properties = null
            };
            block.BlockEntityBehaviors = block.BlockEntityBehaviors.Append(bebehavior);
        }
        else
        {
            CollectibleBehaviorShapeTexturesFromAttributes behavior = new(obj);
            behavior.Initialize(props);
            obj.CollectibleBehaviors = obj.CollectibleBehaviors.Append(behavior);
        }
    }

    //private void AddCreativeInventoryStacks(CollectibleObject obj, JsonObject props)
    //{
    //    string[] prevTabs = obj.CreativeInventoryTabs;
    //    obj.CreativeInventoryTabs = [];
    //    obj.CreativeInventoryStacks = obj.CreativeInventoryStacks.Append(new CreativeTabAndStackList()
    //    {
    //        Tabs = prevTabs,
    //        Stacks = []
    //    });
    //}

    #region Behavior Properties
    private JsonObject toolrackProps;
    #endregion

    public override void AssetsLoaded(ICoreAPI api)
    {
        toolrackProps = JsonObject.FromJson(api.Assets.TryGet(AssetLocation.Create("vanvar:config/forcedpatches/toolrack-properties.json")).ToText());
    }

    public override void Dispose()
    {
        toolrackProps = null;
    }
}
