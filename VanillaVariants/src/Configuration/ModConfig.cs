using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;

namespace VanillaVariants.Configuration;

static class ModConfig
{
    private const string jsonConfig = "VanillaVariants.json";

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

        api.World.Config.SetBool("VanillaVariants_CraftableCage_Enabled", config.CraftableCage);
        api.World.Config.SetBool("VanillaVariants_CraftableWagonWheels_Enabled", config.CraftableWagonWheels);
        api.World.Config.SetBool("VanillaVariants_CraftableWoodenRails_Enabled", config.CraftableWoodenRails);

        api.World.Config.SetBool("VanillaVariants_DecorativeQuern_Enabled", config.DecorativeQuern);
        api.World.Config.SetBool("VanillaVariants_FunctionalQuern_Enabled", config.FunctionalQuern);

        api.World.Config.SetBool("VanillaVariants_RidingEquipment_Enabled", config.RidingEquipment);
        api.World.Config.SetBool("VanillaVariants_ArchimedesScrew_Enabled", config.ArchimedesScrew);
        api.World.Config.SetBool("VanillaVariants_ArmorStand_Enabled", config.ArmorStand);
        api.World.Config.SetBool("VanillaVariants_Barrel_Enabled", config.Barrel);
        api.World.Config.SetBool("VanillaVariants_Basket_Enabled", config.Basket);
        api.World.Config.SetBool("VanillaVariants_Bed_Enabled", config.Bed);
        api.World.Config.SetBool("VanillaVariants_Cage_Enabled", config.Cage);
        api.World.Config.SetBool("VanillaVariants_Chair_Enabled", config.Chair);
        api.World.Config.SetBool("VanillaVariants_Chest_Enabled", config.Chest);
        api.World.Config.SetBool("VanillaVariants_Chute_Enabled", config.Chute);
        api.World.Config.SetBool("VanillaVariants_ChuteSectionItem_Enabled", config.ChuteSectionItem);
        api.World.Config.SetBool("VanillaVariants_CrudeDoor_Enabled", config.CrudeDoor);
        api.World.Config.SetBool("VanillaVariants_DisplayCase_Enabled", config.DisplayCase);
        api.World.Config.SetBool("VanillaVariants_Firewood_Enabled", config.Firewood);
        api.World.Config.SetBool("VanillaVariants_Forge_Enabled", config.Forge);
        api.World.Config.SetBool("VanillaVariants_FruitPress_Enabled", config.FruitPress);
        api.World.Config.SetBool("VanillaVariants_HandBasket_Enabled", config.HandBasket);
        api.World.Config.SetBool("VanillaVariants_Henbox_Enabled", config.Henbox);
        api.World.Config.SetBool("VanillaVariants_Hopper_Enabled", config.Hopper);
        api.World.Config.SetBool("VanillaVariants_Ladder_Enabled", config.Ladder);
        api.World.Config.SetBool("VanillaVariants_MechanicalAngledGears_Enabled", config.MechanicalAngledGears);
        api.World.Config.SetBool("VanillaVariants_MechanicalAxle_Enabled", config.MechanicalAxle);
        api.World.Config.SetBool("VanillaVariants_MechanicalBrake_Enabled", config.MechanicalBrake);
        api.World.Config.SetBool("VanillaVariants_MechanicalClutch_Enabled", config.MechanicalClutch);
        api.World.Config.SetBool("VanillaVariants_MechanicalHelveHammerBase_Enabled", config.MechanicalHelveHammerBase);
        api.World.Config.SetBool("VanillaVariants_MechanicalHelveHammerItem_Enabled", config.MechanicalHelveHammerItem);
        api.World.Config.SetBool("VanillaVariants_MechanicalLargeGear_Enabled", config.MechanicalLargeGear);
        api.World.Config.SetBool("VanillaVariants_MechanicalLargeGearSectionItem_Enabled", config.MechanicalLargeGearSectionItem);
        api.World.Config.SetBool("VanillaVariants_MechanicalPulverizer_Enabled", config.MechanicalPulverizer);
        api.World.Config.SetBool("VanillaVariants_MechanicalToggle_Enabled", config.MechanicalToggle);
        api.World.Config.SetBool("VanillaVariants_MechanicalTransmission_Enabled", config.MechanicalTransmission);
        api.World.Config.SetBool("VanillaVariants_MechanicalWindmillRotor_Enabled", config.MechanicalWindmillRotor);
        api.World.Config.SetBool("VanillaVariants_MetalDoor_Enabled", config.MetalDoor);
        api.World.Config.SetBool("VanillaVariants_Moldrack_Enabled", config.Moldrack);
        api.World.Config.SetBool("VanillaVariants_OmokTabletop_Enabled", config.OmokTabletop);
        api.World.Config.SetBool("VanillaVariants_Palisade_Enabled", config.Palisade);
        api.World.Config.SetBool("VanillaVariants_Shelf_Enabled", config.Shelf);
        api.World.Config.SetBool("VanillaVariants_Sieve_Enabled", config.Sieve);
        api.World.Config.SetBool("VanillaVariants_Sign_Enabled", config.Sign);
        api.World.Config.SetBool("VanillaVariants_Signpost_Enabled", config.Signpost);
        api.World.Config.SetBool("VanillaVariants_SodRoofing_Enabled", config.SodRoofing);
        api.World.Config.SetBool("VanillaVariants_StonePath_Enabled", config.StonePath);
        api.World.Config.SetBool("VanillaVariants_SupportBeamMetal_Enabled", config.SupportBeamMetal);
        api.World.Config.SetBool("VanillaVariants_SupportChain_Enabled", config.SupportChain);
        api.World.Config.SetBool("VanillaVariants_Table_Enabled", config.Table);
        api.World.Config.SetBool("VanillaVariants_Toolrack_Enabled", config.Toolrack);
        api.World.Config.SetBool("VanillaVariants_TroughLarge_Enabled", config.TroughLarge);
        api.World.Config.SetBool("VanillaVariants_TroughSmall_Enabled", config.TroughSmall);
        api.World.Config.SetBool("VanillaVariants_WagonWheels_Enabled", config.WagonWheels);
        api.World.Config.SetBool("VanillaVariants_WoodBucket_Enabled", config.WoodBucket);
        api.World.Config.SetBool("VanillaVariants_WoodenPan_Enabled", config.WoodenPan);
        api.World.Config.SetBool("VanillaVariants_WoodenPath_Enabled", config.WoodenPath);
        api.World.Config.SetBool("VanillaVariants_WoodenRails_Enabled", config.WoodenRails);

        foreach ((string name, Dictionary<string, bool> metals) in config.ChuteCraftable)
        {
            api.World.Config.SetBool($"VanillaVariants-anycraftable-{name}", metals.Any(x => x.Value));

            foreach ((string metal, bool craftable) in metals)
            {
                api.World.Config.SetBool($"VanillaVariants-craftable-{name}-{metal}", craftable);
            }
        }

        return config;
    }

    public static void WriteConfig(ICoreAPI api, Config config) => GenerateConfig(api, config);

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