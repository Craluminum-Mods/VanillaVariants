using AttributeRenderingLibrary;
using System.Collections.Generic;
using VanillaVariants.Configuration;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Server;
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

    public override void StartServerSide(ICoreServerAPI api)
    {
        api.Event.PlayerJoin += (byPlayer) => Event_PlayerJoin(byPlayer, api);
    }

    private void Event_PlayerJoin(IServerPlayer byPlayer, ICoreServerAPI api)
    {
        api.Network.GetChannel("vanvar").SendPacket<VanillaVariants.Configuration.Config>(Config, byPlayer);
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
        if (obj == null || obj.Code == null) return;
        if (obj.Code.Domain != "game") return;

        AddBehaviorWithPropertiesIfTrue(Config.Toolrack && obj is BlockToolRack, obj, behaviorProps["toolrack"]);
        AddBehaviorWithPropertiesIfTrue(Config.DisplayCase && obj is BlockDisplayCase, obj, behaviorProps["displayCase"]);
        AddBehaviorWithPropertiesIfTrue(Config.Ladder && obj.Code.PathStartsWith("ladder-wood"), obj, behaviorProps["ladder"]);
        AddBehaviorWithPropertiesIfTrue(Config.Shelf && obj is BlockShelf && obj.Code.PathStartsWith("shelf-normal"), obj, behaviorProps["shelf"]);
        AddBehaviorWithPropertiesIfTrue(Config.Sign && obj is BlockSign && obj.Code.PathStartsWith("sign"), obj, behaviorProps["sign"]);
        AddBehaviorWithPropertiesIfTrue(Config.Signpost && obj is BlockSignPost && obj.Code.PathStartsWith("signpost"), obj, behaviorProps["signpost"]);
        AddBehaviorWithPropertiesIfTrue(Config.Moldrack && obj is BlockMoldRack && obj.Code.PathStartsWith("moldrack"), obj, behaviorProps["moldrack"]);
        AddBehaviorWithPropertiesIfTrue(Config.Henbox && obj is BlockHenbox, obj, behaviorProps["henbox"]);
        AddBehaviorWithPropertiesIfTrue(Config.Sieve && obj.Code.PathStartsWith("sieve"), obj, behaviorProps["sieve"]);
        AddBehaviorWithPropertiesIfTrue(Config.OmokTabletop && obj is BlockOmokTable, obj, behaviorProps["omoktabletop"]);
        AddBehaviorWithPropertiesIfTrue(Config.Forge && obj is BlockForge, obj, behaviorProps["forge"]);
    }

    private void AddBehaviorWithPropertiesIfTrue(bool condition, CollectibleObject obj, JsonObject props)
    {
        if (!condition) return;
        if (props == null) return;
        if (obj is Block block)
        {
            AddExtraBlockBehaviors(block);

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

    private static void AddExtraBlockBehaviors(Block block)
    {
        Vintagestory.GameContent.BlockBehaviorHorizontalAttachable prevAttachableBehavior = block.GetBehavior<Vintagestory.GameContent.BlockBehaviorHorizontalAttachable>();
        if (prevAttachableBehavior != null)
        {
            JsonObject clonedProps = JsonObject.FromJson(prevAttachableBehavior.propertiesAtString);
            AttributeRenderingLibrary.BlockBehaviorHorizontalAttachable newAttachableBehavior = new(block);
            newAttachableBehavior.Initialize(clonedProps);

            int index = block.CollectibleBehaviors.IndexOf(x => x is Vintagestory.GameContent.BlockBehaviorHorizontalAttachable);
            block.CollectibleBehaviors = block.CollectibleBehaviors.RemoveAt(index);
            block.BlockBehaviors = block.BlockBehaviors.RemoveAt(index);

            block.CollectibleBehaviors = block.CollectibleBehaviors.InsertAt(newAttachableBehavior, index);
            block.BlockBehaviors = block.BlockBehaviors.InsertAt(newAttachableBehavior, index);
        }
    }

    private Dictionary<string, JsonObject> behaviorProps;

    public override void AssetsLoaded(ICoreAPI api)
    {
        if (api.Side.IsClient()) return;

        behaviorProps = new Dictionary<string, JsonObject>(4);
        behaviorProps["toolrack"] = JsonObject.FromJson(api.Assets.TryGet(AssetLocation.Create("vanvar:config/forcedpatches/toolrack-properties.json")).ToText());
        behaviorProps["displayCase"] = JsonObject.FromJson(api.Assets.TryGet(AssetLocation.Create("vanvar:config/forcedpatches/displaycase-properties.json")).ToText());
        behaviorProps["ladder"] = JsonObject.FromJson(api.Assets.TryGet(AssetLocation.Create("vanvar:config/forcedpatches/ladder-properties.json")).ToText());
        behaviorProps["shelf"] = JsonObject.FromJson(api.Assets.TryGet(AssetLocation.Create("vanvar:config/forcedpatches/shelf-properties.json")).ToText());
        behaviorProps["sign"] = JsonObject.FromJson(api.Assets.TryGet(AssetLocation.Create("vanvar:config/forcedpatches/sign-properties.json")).ToText());
        behaviorProps["signpost"] = JsonObject.FromJson(api.Assets.TryGet(AssetLocation.Create("vanvar:config/forcedpatches/signpost-properties.json")).ToText());
        behaviorProps["moldrack"] = JsonObject.FromJson(api.Assets.TryGet(AssetLocation.Create("vanvar:config/forcedpatches/moldrack-properties.json")).ToText());
        behaviorProps["henbox"] = JsonObject.FromJson(api.Assets.TryGet(AssetLocation.Create("vanvar:config/forcedpatches/henbox-properties.json")).ToText());
        behaviorProps["sieve"] = JsonObject.FromJson(api.Assets.TryGet(AssetLocation.Create("vanvar:config/forcedpatches/sieve-properties.json")).ToText());
        behaviorProps["omoktabletop"] = JsonObject.FromJson(api.Assets.TryGet(AssetLocation.Create("vanvar:config/forcedpatches/omoktabletop-properties.json")).ToText());
        behaviorProps["forge"] = JsonObject.FromJson(api.Assets.TryGet(AssetLocation.Create("vanvar:config/forcedpatches/forge-properties.json")).ToText());
    }

    public override void Dispose()
    {
        behaviorProps = null;
    }
}
