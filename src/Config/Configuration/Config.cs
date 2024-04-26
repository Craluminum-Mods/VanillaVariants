using System.Collections.Generic;
using Vintagestory.API.Util;

namespace VanillaVariants.Configuration;

public class Config
{
    public bool ExperimentalOverlayTest { get; set; } = true;

    public bool ResolveBarrelSounds { get; set; } = true;
    public bool ResolveChestNames { get; set; } = true;
    public bool ResolveHenboxImposter { get; set; } = true;
    public bool ResolveMechanicalBlockIssues { get; set; } = true;
    public bool ResolveQuernAndAxleRelationship { get; set; } = true;

    public bool CraftableCage { get; set; }
    public bool CraftableWagonWheels { get; set; }
    public bool CraftableWoodenRails { get; set; }

    public bool DecorativeQuern { get; set; } = true;
    public bool MakeDecorativeQuernFunctional { get; set; } = false;

    public bool ArchimedesScrew { get; set; } = true;
    public bool ArmorStand { get; set; } = true;
    public bool Barrel { get; set; } = true;
    public bool Bed { get; set; } = true;
    public bool Cage { get; set; } = true;
    public bool Chair { get; set; } = true;
    public bool Chest { get; set; } = true;
    public bool Chute { get; set; } = true;
    public bool ChuteSectionItem { get; set; } = true;
    public bool CrudeDoor { get; set; } = true;
    public bool DisplayCase { get; set; } = true;
    public bool Firewood { get; set; } = true;
    public bool Forge { get; set; } = true;
    public bool FruitPress { get; set; } = true;
    public bool Henbox { get; set; } = true;
    public bool Hopper { get; set; } = true;
    public bool Ladder { get; set; } = true;
    public bool MechanicalAngledGears { get; set; } = true;
    public bool MechanicalAxle { get; set; } = true;
    public bool MechanicalBrake { get; set; } = true;
    public bool MechanicalClutch { get; set; } = true;
    public bool MechanicalHelveHammerBase { get; set; } = true;
    public bool MechanicalHelveHammerItem { get; set; } = true;
    public bool MechanicalLargeGear { get; set; } = true;
    public bool MechanicalLargeGearSectionItem { get; set; } = true;
    public bool MechanicalPulverizer { get; set; } = true;
    public bool MechanicalToggle { get; set; } = true;
    public bool MechanicalTransmission { get; set; } = true;
    public bool MechanicalWindmillRotor { get; set; } = true;
    public bool Moldrack { get; set; } = true;
    public bool OmokTabletop { get; set; } = true;
    public bool Palisade { get; set; } = true;
    public bool Shelf { get; set; } = true;
    public bool Sieve { get; set; } = true;
    public bool Sign { get; set; } = true;
    public bool Signpost { get; set; } = true;
    public bool SodRoofing { get; set; } = true;
    public bool StonePath { get; set; } = true;
    public bool SupportChain { get; set; } = true;
    public bool Table { get; set; } = true;
    public bool Toolrack { get; set; } = true;
    public bool Trapdoor { get; set; } = true;
    public bool TroughLarge { get; set; } = true;
    public bool TroughSmall { get; set; } = true;
    public bool WagonWheels { get; set; } = true;
    public bool WoodBucket { get; set; } = true;
    public bool WoodenPan { get; set; } = true;
    public bool WoodenPath { get; set; } = true;
    public bool WoodenRails { get; set; } = true;

    public Dictionary<string, Dictionary<string, float>> FlowRates { get; set; } = new()
    {
        ["archimedesscrew"] = new() { ["default"] = 1, },
        ["chute"] = new() { ["default"] = 1, },
        ["hopper"] = new() { ["default"] = 1, },
    };

    public Dictionary<string, Dictionary<string, int>> QuantitySlots { get; set; } = new()
    {
        ["archimedesscrew"] = new() { ["default"] = 1, },
        ["chute"] = new() { ["default"] = 1, },
        ["hopper"] = new() { ["default"] = 4, },
    };

    public Dictionary<string, Dictionary<string, int>> CheckRateMs { get; set; } = new()
    {
        ["archimedesscrew"] = new() { ["default"] = 500, },
    };

    public Dictionary<string, Dictionary<string, bool>> CraftableChutes { get; set; } = new()
    {
        ["archimedesscrew"] = new()
        {
            ["tinbronze"] = true,
            ["bismuthbronze"] = true,
            ["blackbronze"] = true,
            ["iron"] = true,
            ["meteoriciron"] = true,
            ["steel"] = true
        },
        ["chute"] = new()
        {
            ["tinbronze"] = true,
            ["bismuthbronze"] = true,
            ["blackbronze"] = true,
            ["iron"] = true,
            ["meteoriciron"] = true,
            ["steel"] = true
        },
        ["chutesection"] = new()
        {
            ["tinbronze"] = true,
            ["bismuthbronze"] = true,
            ["blackbronze"] = true,
            ["iron"] = true,
            ["meteoriciron"] = true,
            ["steel"] = true
        },
        ["hopper"] = new()
        {
            ["tinbronze"] = true,
            ["bismuthbronze"] = true,
            ["blackbronze"] = true,
            ["iron"] = true,
            ["meteoriciron"] = true,
            ["steel"] = true
        },
    };

    public Config() { }

    public Config(Config previousConfig)
    {
        FlowRates.AddRange(previousConfig.FlowRates);
        QuantitySlots.AddRange(previousConfig.QuantitySlots);
        CheckRateMs.AddRange(previousConfig.CheckRateMs);
        CraftableChutes.AddRange(previousConfig.CraftableChutes);

        ExperimentalOverlayTest = previousConfig.ExperimentalOverlayTest;

        ResolveBarrelSounds = previousConfig.ResolveBarrelSounds;
        ResolveChestNames = previousConfig.ResolveChestNames;
        ResolveHenboxImposter = previousConfig.ResolveHenboxImposter;
        ResolveMechanicalBlockIssues = previousConfig.ResolveMechanicalBlockIssues;
        ResolveQuernAndAxleRelationship = previousConfig.ResolveQuernAndAxleRelationship;

        CraftableCage = previousConfig.CraftableCage;
        CraftableWagonWheels = previousConfig.CraftableWagonWheels;
        CraftableWoodenRails = previousConfig.CraftableWoodenRails;

        DecorativeQuern = previousConfig.DecorativeQuern;
        MakeDecorativeQuernFunctional = previousConfig.MakeDecorativeQuernFunctional;

        ArchimedesScrew = previousConfig.ArchimedesScrew;
        ArmorStand = previousConfig.ArmorStand;
        Barrel = previousConfig.Barrel;
        Bed = previousConfig.Bed;
        Cage = previousConfig.Cage;
        Chair = previousConfig.Chair;
        Chest = previousConfig.Chest;
        Chute = previousConfig.Chute;
        ChuteSectionItem = previousConfig.ChuteSectionItem;
        CrudeDoor = previousConfig.CrudeDoor;
        DisplayCase = previousConfig.DisplayCase;
        Firewood = previousConfig.Firewood;
        Forge = previousConfig.Forge;
        FruitPress = previousConfig.FruitPress;
        Henbox = previousConfig.Henbox;
        Hopper = previousConfig.Hopper;
        Ladder = previousConfig.Ladder;
        MechanicalAngledGears = previousConfig.MechanicalAngledGears;
        MechanicalAxle = previousConfig.MechanicalAxle;
        MechanicalBrake = previousConfig.MechanicalBrake;
        MechanicalClutch = previousConfig.MechanicalClutch;
        MechanicalHelveHammerBase = previousConfig.MechanicalHelveHammerBase;
        MechanicalHelveHammerItem = previousConfig.MechanicalHelveHammerItem;
        MechanicalLargeGear = previousConfig.MechanicalLargeGear;
        MechanicalLargeGearSectionItem = previousConfig.MechanicalLargeGearSectionItem;
        MechanicalPulverizer = previousConfig.MechanicalPulverizer;
        MechanicalToggle = previousConfig.MechanicalToggle;
        MechanicalTransmission = previousConfig.MechanicalTransmission;
        MechanicalWindmillRotor = previousConfig.MechanicalWindmillRotor;
        Moldrack = previousConfig.Moldrack;
        OmokTabletop = previousConfig.OmokTabletop;
        Palisade = previousConfig.Palisade;
        Shelf = previousConfig.Shelf;
        Sieve = previousConfig.Sieve;
        Sign = previousConfig.Sign;
        Signpost = previousConfig.Signpost;
        SodRoofing = previousConfig.SodRoofing;
        StonePath = previousConfig.StonePath;
        SupportChain = previousConfig.SupportChain;
        Table = previousConfig.Table;
        Toolrack = previousConfig.Toolrack;
        Trapdoor = previousConfig.Trapdoor;
        TroughLarge = previousConfig.TroughLarge;
        TroughSmall = previousConfig.TroughSmall;
        WagonWheels = previousConfig.WagonWheels;
        WoodBucket = previousConfig.WoodBucket;
        WoodenPan = previousConfig.WoodenPan;
        WoodenPath = previousConfig.WoodenPath;
        WoodenRails = previousConfig.WoodenRails;
    }
}