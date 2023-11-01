using Newtonsoft.Json.Linq;
using VanillaVariants.Configuration;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

[assembly: ModInfo(name: "Vanilla Variants", modID: "vanvar")]

namespace VanillaVariants;

public class Core : ModSystem
{
    private string[][] ItemCodesArray { get; } = new string[][] {
        new string[] { "@ladder-(?!wood|rope)(.*)", "block-ladder-wood-*", "material" },
        new string[] { "@moldrack-(?!normal)(.*)", "block-moldrack-*", "type" },
        new string[] { "@pan-(?!wooden)(.*)", "block-pan-wooden", "type" },
        new string[] { "@shelf-(?!normal)(.*)", "block-shelf-normal-*", "type" },
        new string[] { "@trough-(?!genericwood)(.*large)-(.*)", "block-trough-genericwood-large-head-*", "material" },
        new string[] { "@trough-(?!genericwood)(.*small)-(.*)", "block-trough-genericwood-small-*", "material" }
    };

    public static Config Config { get; private set; }

    public override void StartPre(ICoreAPI api)
    {
        base.StartPre(api);
        Config = ModConfig.ReadConfig(api);
    }

    public override void Start(ICoreAPI api)
    {
        base.Start(api);

        api.RegisterEntity("VV_EntityWoodArmorStand", typeof(EntityWoodArmorStand));
        api.RegisterBlockBehaviorClass("VanillaVariants.BbName", typeof(BlockBehaviorName));
        api.RegisterCollectibleBehaviorClass("VanillaVariants.CbName", typeof(CollectibleBehaviorName));
        api.World.Logger.Event("started '{0}' mod", Mod.Info.ModID);
    }

    public override void AssetsFinalize(ICoreAPI api)
    {
        AppendBlockBehaviors(api);
    }

    private void AppendBlockBehaviors(ICoreAPI api)
    {
        foreach (Block block in api.World.Blocks)
        {
            bool matched = false;
            object properties = null;

            foreach (string[] s in ItemCodesArray)
            {
                if (block.WildCardMatch(s[0]))
                {
                    matched = true;
                    properties = new
                    {
                        translationName = s[1],
                        translationPart = "material-" + block.Variant[s[2]]
                    };
                    break;
                }
            }

            if (matched)
            {
                AppendBlockBehavior(new BlockBehaviorName(block), properties);
            }
        }
    }

    private static void AppendBlockBehavior<T>(T instance, object propertiesFromJson) where T : BlockBehavior
    {
        instance.Initialize(new JsonObject(JToken.FromObject(propertiesFromJson)));
        instance.block.CollectibleBehaviors = instance.block.CollectibleBehaviors.Append(instance);
        instance.block.BlockBehaviors = instance.block.BlockBehaviors.Append(instance);
    }
}