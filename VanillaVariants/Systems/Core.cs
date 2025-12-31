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
        api.Network.GetChannel("vanvar").SendPacket(Config, byPlayer);
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
                AddRenderingBehavior(block);
            }
        }

        foreach (Item item in api.World.Items)
        {
            api.TryAddModDescription(item);

            if (api.Side.IsServer())
            {
                AddRenderingBehavior(item);
            }
        }
    }

    private void AddRenderingBehavior(CollectibleObject obj)
    {
        if (obj == null || obj.Code == null) return;
        if (obj.Code.Domain != "game") return;

        AddRenderingBehaviorIfTrue(Config.Toolrack && obj is BlockToolRack, obj, behaviorProps["toolrack"]);
        AddRenderingBehaviorIfTrue(Config.DisplayCase && obj is BlockDisplayCase, obj, behaviorProps["displayCase"]);
        AddRenderingBehaviorIfTrue(Config.Ladder && obj.Code.PathStartsWith("ladder-wood"), obj, behaviorProps["ladder"]);
        AddRenderingBehaviorIfTrue(Config.Shelf && obj is BlockShelf && obj.Code.PathStartsWith("shelf-normal"), obj, behaviorProps["shelf"]);
        AddRenderingBehaviorIfTrue(Config.Sign && obj is BlockSign && obj.Code.PathStartsWith("sign"), obj, behaviorProps["sign"]);
        AddRenderingBehaviorIfTrue(Config.Signpost && obj is BlockSignPost && obj.Code.PathStartsWith("signpost"), obj, behaviorProps["signpost"]);
        AddRenderingBehaviorIfTrue(Config.Moldrack && obj is BlockMoldRack && obj.Code.PathStartsWith("moldrack"), obj, behaviorProps["moldrack"]);
        AddRenderingBehaviorIfTrue(Config.Henbox && obj is BlockHenbox, obj, behaviorProps["henbox"]);
        AddRenderingBehaviorIfTrue(Config.Sieve && obj.Code.PathStartsWith("sieve"), obj, behaviorProps["sieve"]);
        AddRenderingBehaviorIfTrue(Config.OmokTabletop && obj is BlockOmokTable, obj, behaviorProps["omoktabletop"]);
        AddRenderingBehaviorIfTrue(Config.Forge && obj is BlockForge, obj, behaviorProps["forge"]);
        AddRenderingBehaviorIfTrue(Config.Table && obj.Code.PathStartsWith("table-normal"), obj, behaviorProps["table"]);
        AddRenderingBehaviorIfTrue(Config.Table && obj.Code.PathStartsWith("table-whitemarble"), obj, behaviorProps["table-whitemarble"]);
        AddRenderingBehaviorIfTrue(Config.Table && obj.Code.PathStartsWith("table-redmarble"), obj, behaviorProps["table-redmarble"]);
        AddRenderingBehaviorIfTrue(Config.Table && obj.Code.PathStartsWith("table-greenmarble"), obj, behaviorProps["table-greenmarble"]);
        AddRenderingBehaviorIfTrue(Config.Chair && obj.Code.PathStartsWith("chair"), obj, behaviorProps["chair"]);
        AddRenderingBehaviorIfTrue(Config.TroughLarge && obj is BlockTroughDoubleBlock, obj, behaviorProps["trough-large"]);
        AddRenderingBehaviorIfTrue(Config.TroughSmall && obj is BlockTrough, obj, behaviorProps["trough-small"]);
        AddRenderingBehaviorIfTrue(Config.WoodenRails && obj is BlockRails, obj, behaviorProps["woodenrails"]);
        AddRenderingBehaviorIfTrue(Config.Barrel && obj is BlockBarrel, obj, behaviorProps["barrel"]);
        AddRenderingBehaviorIfTrue(Config.WoodBucket && obj is BlockBucket, obj, behaviorProps["bucket"]);
        AddRenderingBehaviorIfTrue(Config.Bed && obj is BlockBed && obj.Code.PathStartsWith("bed-wood"), obj, behaviorProps["bed"]);
        AddRenderingBehaviorIfTrue(Config.Cage && obj.Code.PathStartsWith("cage"), obj, behaviorProps["cage"]);
    }

    private void AddRenderingBehaviorIfTrue(bool condition, CollectibleObject obj, JsonObject props)
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

            block.BlockEntityBehaviors = block.BlockEntityBehaviors.Append(new BlockEntityBehaviorType
            {
                Name = "AttributeRenderingLibrary.ShapeTexturesFromAttributes",
                properties = null
            });

            block.EntityClass ??= "Generic";
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
        if (block.HasBehavior<Vintagestory.GameContent.BlockBehaviorHorizontalAttachable>())
        {
            Vintagestory.GameContent.BlockBehaviorHorizontalAttachable _prevBehavior = block.GetBehavior<Vintagestory.GameContent.BlockBehaviorHorizontalAttachable>();
            AttributeRenderingLibrary.BlockBehaviorHorizontalAttachable _newBehavior = new(block);
            _newBehavior.Initialize(properties: JsonObject.FromJson(_prevBehavior.propertiesAtString));

            int index = block.CollectibleBehaviors.IndexOf(b => b is Vintagestory.GameContent.BlockBehaviorHorizontalAttachable);
            block.CollectibleBehaviors = block.CollectibleBehaviors.RemoveAt(index);
            block.BlockBehaviors = block.BlockBehaviors.RemoveAt(index);

            block.CollectibleBehaviors = block.CollectibleBehaviors.InsertAt(_newBehavior, index);
            block.BlockBehaviors = block.BlockBehaviors.InsertAt(_newBehavior, index);
        }

        if (block.HasBehavior<Vintagestory.GameContent.BlockBehaviorNWOrientable>())
        {
            Vintagestory.GameContent.BlockBehaviorNWOrientable _prevBehavior = block.GetBehavior<Vintagestory.GameContent.BlockBehaviorNWOrientable>();
            AttributeRenderingLibrary.BlockBehaviorNWOrientable _newBehavior = new(block);
            _newBehavior.Initialize(properties: JsonObject.FromJson(_prevBehavior.propertiesAtString));

            int index = block.CollectibleBehaviors.IndexOf(b => b is Vintagestory.GameContent.BlockBehaviorNWOrientable);
            block.CollectibleBehaviors = block.CollectibleBehaviors.RemoveAt(index);
            block.BlockBehaviors = block.BlockBehaviors.RemoveAt(index);

            block.CollectibleBehaviors = block.CollectibleBehaviors.InsertAt(_newBehavior, index);
            block.BlockBehaviors = block.BlockBehaviors.InsertAt(_newBehavior, index);
        }

        if (block.HasBehavior<Vintagestory.GameContent.BlockBehaviorHorizontalOrientable>())
        {
            Vintagestory.GameContent.BlockBehaviorHorizontalOrientable _prevBehavior = block.GetBehavior<Vintagestory.GameContent.BlockBehaviorHorizontalOrientable>();
            AttributeRenderingLibrary.BlockBehaviorHorizontalOrientable _newBehavior = new(block);
            _newBehavior.Initialize(properties: JsonObject.FromJson(_prevBehavior.propertiesAtString));

            int _index = block.CollectibleBehaviors.IndexOf(b => b is Vintagestory.GameContent.BlockBehaviorHorizontalOrientable);
            block.CollectibleBehaviors = block.CollectibleBehaviors.RemoveAt(_index);
            block.BlockBehaviors = block.BlockBehaviors.RemoveAt(_index);

            block.CollectibleBehaviors = block.CollectibleBehaviors.InsertAt(_newBehavior, _index);
            block.BlockBehaviors = block.BlockBehaviors.InsertAt(_newBehavior, _index);
        }
    }

    private FastSmallDictionary<string, JsonObject> behaviorProps;

    public override void AssetsLoaded(ICoreAPI api)
    {
        if (api.Side.IsClient()) return;

        behaviorProps = new FastSmallDictionary<string, JsonObject>(19);
        behaviorProps["toolrack"] = LoadProperties(api, "toolrack");
        behaviorProps["displayCase"] = LoadProperties(api, "displaycase");
        behaviorProps["ladder"] = LoadProperties(api, "ladder");
        behaviorProps["shelf"] = LoadProperties(api, "shelf");
        behaviorProps["sign"] = LoadProperties(api, "sign");
        behaviorProps["signpost"] = LoadProperties(api, "signpost");
        behaviorProps["moldrack"] = LoadProperties(api, "moldrack");
        behaviorProps["henbox"] = LoadProperties(api, "henbox");
        behaviorProps["sieve"] = LoadProperties(api, "sieve");
        behaviorProps["omoktabletop"] = LoadProperties(api, "omoktabletop");
        behaviorProps["forge"] = LoadProperties(api, "forge");
        behaviorProps["table"] = LoadProperties(api, "table");
        behaviorProps["table-whitemarble"] = LoadProperties(api, "table-whitemarble");
        behaviorProps["table-redmarble"] = LoadProperties(api, "table-redmarble");
        behaviorProps["table-greenmarble"] = LoadProperties(api, "table-greenmarble");
        behaviorProps["chair"] = LoadProperties(api, "chair");
        behaviorProps["trough-large"] = LoadProperties(api, "trough-large");
        behaviorProps["trough-small"] = LoadProperties(api, "trough-small");
        behaviorProps["woodenrails"] = LoadProperties(api, "woodenrails");
        behaviorProps["barrel"] = LoadProperties(api, "barrel");
        behaviorProps["bucket"] = LoadProperties(api, "bucket");
        behaviorProps["bed"] = LoadProperties(api, "bed");
        behaviorProps["cage"] = LoadProperties(api, "cage");
    }

    public JsonObject LoadProperties(ICoreAPI api, string pathEnding)
    {
        return JsonObject.FromJson(api.Assets.TryGet(AssetLocation.Create($"vanvar:config/behaviorproperties/{pathEnding}.json")).ToText());
    }

    public override void Dispose()
    {
        behaviorProps = null;
    }
}
