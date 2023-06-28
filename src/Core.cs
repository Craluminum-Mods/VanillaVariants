using Vintagestory.API.Common;
using VanillaVariants.Configuration;
using Vintagestory.API.Util;
using Vintagestory.API.Datastructures;
using Newtonsoft.Json.Linq;

[assembly: ModInfo("Vanilla Variants")]

namespace VanillaVariants;

public class Core : ModSystem
{
    public override void Start(ICoreAPI api)
    {
        base.Start(api);
        ModConfig.ReadConfig(api);
        api.RegisterEntity("VV_EntityWoodArmorStand", typeof(EntityWoodArmorStand));
        api.RegisterBlockBehaviorClass("VanillaVariants.BbName", typeof(BlockBehaviorName));
        api.RegisterCollectibleBehaviorClass("VanillaVariants.CbName", typeof(CollectibleBehaviorName));
        api.World.Logger.Event("started 'Vanilla Variants' mod");
    }

    public override void AssetsFinalize(ICoreAPI api)
    {
        foreach (var block in api.World.Blocks)
        {
            bool matched = false;
            object properties = null;

            if (block.WildCardMatch("@ladder-(?!wood|rope)(.*)"))
            {
                matched = true;
                properties = new
                {
                    translationName = "block-ladder-wood-*",
                    translationPart = "material-" + block.Variant["material"]
                };
            }
            else if (block.WildCardMatch("@moldrack-(?!normal)(.*)"))
            {
                matched = true;
                properties = new
                {
                    translationName = "block-moldrack-*",
                    translationPart = "material-" + block.Variant["type"]
                };
            }
            else if (block.WildCardMatch("@pan-(?!wooden)(.*)"))
            {
                matched = true;
                properties = new
                {
                    translationName = "block-pan-wooden",
                    translationPart = "material-" + block.Variant["type"]
                };
            }
            else if (block.WildCardMatch("@shelf-(?!normal)(.*)"))
            {
                matched = true;
                properties = new
                {
                    translationName = "block-shelf-normal-*",
                    translationPart = "material-" + block.Variant["type"]
                };
            }
            else if (block.WildCardMatch("@trough-(?!genericwood)(.*large)-(.*)"))
            {
                matched = true;
                properties = new
                {
                    translationName = "block-trough-genericwood-large-head-*",
                    translationPart = "material-" + block.Variant["material"]
                };
            }
            else if (block.WildCardMatch("@trough-(?!genericwood)(.*small)-(.*)"))
            {
                matched = true;
                properties = new
                {
                    translationName = "block-trough-genericwood-small-*",
                    translationPart = "material-" + block.Variant["material"]
                };
            }

            if (matched) AppendBlockBehavior(new BlockBehaviorName(block), properties);
        }
    }

    private void AppendBlockBehavior<T>(T instance, object propertiesFromJson) where T : BlockBehavior
    {
        instance.Initialize(new JsonObject(JToken.FromObject(propertiesFromJson)));
        instance.block.CollectibleBehaviors = instance.block.CollectibleBehaviors.Append(instance);
        instance.block.BlockBehaviors = instance.block.BlockBehaviors.Append(instance);
    }
}