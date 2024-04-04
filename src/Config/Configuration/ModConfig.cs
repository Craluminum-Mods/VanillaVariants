using Vintagestory.API.Common;

namespace VanillaVariants.Configuration;

static class ModConfig
{
    private const string jsonConfig = "VanillaVariantsConfig.json";

    public static Config ReadConfig(ICoreAPI api)
    {
        Config config;

        try
        {
            config = LoadConfig(api);

            if (config == null)
            {
                GenerateConfig(api);
                config = LoadConfig(api);
            }
            else
            {
                GenerateConfig(api, config);
            }
        }
        catch
        {
            GenerateConfig(api);
            config = LoadConfig(api);
        }

        api.World.Config.SetBool("VVMoreRecipesFeatureEnabled", config.MoreRecipesFeature);
        api.World.Config.SetBool("VVTradersSellVariantsEnabled", config.TradersSellVariants);

        api.World.Config.SetBool("VVArmorStandEnabled", config.ArmorStand);
        api.World.Config.SetBool("VVBedEnabled", config.Bed);
        api.World.Config.SetBool("VVCageEnabled", config.Cage);
        api.World.Config.SetBool("VVChairEnabled", config.Chair);
        api.World.Config.SetBool("VVDecoQuernEnabled", config.DecorativeQuern);
        api.World.Config.SetBool("VVDisplayCaseEnabled", config.DisplayCase);
        api.World.Config.SetBool("VVForgeEnabled", config.Forge);
        api.World.Config.SetBool("VVHenboxEnabled", config.Henbox);
        api.World.Config.SetBool("VVLadderEnabled", config.Ladder);
        api.World.Config.SetBool("VVMoldrackEnabled", config.Moldrack);
        api.World.Config.SetBool("VVOmokTabletopEnabled", config.OmokTabletop);
        api.World.Config.SetBool("VVPalisadeEnabled", config.Palisade);
        api.World.Config.SetBool("VVShelfEnabled", config.Shelf);
        api.World.Config.SetBool("VVSignEnabled", config.Sign);
        api.World.Config.SetBool("VVSignpostEnabled", config.Signpost);
        api.World.Config.SetBool("VVSodRoofingEnabled", config.SodRoofing);
        api.World.Config.SetBool("VVSupportChainEnabled", config.SupportChain);
        api.World.Config.SetBool("VVTableEnabled", config.Table);
        api.World.Config.SetBool("VVToolrackEnabled", config.Toolrack);
        api.World.Config.SetBool("VVTrapdoorEnabled", config.Trapdoor);
        api.World.Config.SetBool("VVTroughLargeEnabled", config.TroughLarge);
        api.World.Config.SetBool("VVTroughSmallEnabled", config.TroughSmall);
        api.World.Config.SetBool("VVWagonWheelsEnabled", config.WagonWheels);
        api.World.Config.SetBool("VVWoodBucketEnabled", config.WoodBucket);
        api.World.Config.SetBool("VVWoodenPanEnabled", config.WoodenPan);
        api.World.Config.SetBool("VVWoodenPathEnabled", config.WoodenPath);
        api.World.Config.SetBool("VVWoodenRailsEnabled", config.WoodenRails);

        return config;
    }

    private static Config LoadConfig(ICoreAPI api)
    {
        return api.LoadModConfig<Config>(jsonConfig);
    }

    private static void GenerateConfig(ICoreAPI api)
    {
        api.StoreModConfig(new Config(), jsonConfig);
    }

    private static void GenerateConfig(ICoreAPI api, Config previousConfig)
    {
        api.StoreModConfig(new Config(previousConfig), jsonConfig);
    }
}