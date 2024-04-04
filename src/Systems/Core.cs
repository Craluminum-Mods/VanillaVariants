using Newtonsoft.Json.Linq;
using VanillaVariants.Configuration;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

[assembly: ModInfo(name: "Vanilla Variants", modID: "vanvar")]

namespace VanillaVariants;

public class Core : ModSystem
{
    public static Config Config { get; private set; }

    public override void StartPre(ICoreAPI api) => Config = ModConfig.ReadConfig(api);

    public override void Start(ICoreAPI api)
    {
        api.RegisterEntity("VV_EntityWoodArmorStand", typeof(EntityWoodArmorStand));
        api.RegisterBlockBehaviorClass("VanillaVariants.BbName", typeof(BlockBehaviorName));
        api.RegisterCollectibleBehaviorClass("VanillaVariants.CbName", typeof(CollectibleBehaviorName));
        api.RegisterBlockClass("VanillaVariants.BlockWoodBucket", typeof(BlockWoodBucket));

        api.World.Logger.Event("started '{0}' mod", Mod.Info.Name);
    }

    public override void AssetsFinalize(ICoreAPI api) => ApplyPatches(api);

    private static void ApplyPatches(ICoreAPI api)
    {
        GetDefaultPanAttributes(api, out object panningDrops);
        GetDefaultLargeTroughAttributes(api, out object largeContentConfig, out object largeUnsuitableFor);
        GetDefaultSmallTroughAttributes(api, out object smallContentConfig, out object smallUnsuitableFor);

        foreach (Block block in api.World.Blocks)
        {
            if (block?.Code?.Domain != "vanvar")
            {
                continue;
            }

            switch (block)
            {
                case BlockPan:
                    block.Attributes.Token["panningDrops"] = JToken.FromObject(panningDrops);
                    break;
                case BlockTroughDoubleBlock:
                    if (largeContentConfig != null) block.Attributes.Token["contentConfig"] = JToken.FromObject(largeContentConfig);
                    if (largeUnsuitableFor != null) block.Attributes.Token["unsuitableFor"] = JToken.FromObject(largeUnsuitableFor);
                    break;
                case BlockTrough when block.Code.ToString().Contains("small"):
                    if (smallContentConfig != null) block.Attributes.Token["contentConfig"] = JToken.FromObject(smallContentConfig);
                    if (smallUnsuitableFor != null) block.Attributes.Token["unsuitableFor"] = JToken.FromObject(smallUnsuitableFor);
                    break;
            }
        }
    }

    private static void GetDefaultPanAttributes(ICoreAPI api, out object panningDrops)
    {
        Block block = api.World.GetBlock(new AssetLocation("pan-wooden"));
        panningDrops = block?.Attributes?["panningDrops"]?.AsObject<object>();
    }

    private static void GetDefaultLargeTroughAttributes(ICoreAPI api, out object largeContentConfig, out object largeUnsuitableFor)
    {
        Block block = api.World.GetBlock(new AssetLocation("trough-genericwood-large-head-north"));
        largeContentConfig = block?.Attributes?["contentConfig"]?.AsObject<object>();
        largeUnsuitableFor = block?.Attributes?["unsuitableFor"]?.AsObject<object>();
    }

    private static void GetDefaultSmallTroughAttributes(ICoreAPI api, out object smallContentConfig, out object smallUnsuitableFor)
    {
        Block block = api.World.GetBlock(new AssetLocation("trough-genericwood-small-ns"));
        smallContentConfig = block?.Attributes?["contentConfig"]?.AsObject<object>();
        smallUnsuitableFor = block?.Attributes?["unsuitableFor"]?.AsObject<object>();
    }
}