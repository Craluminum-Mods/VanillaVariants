using System.Collections.Generic;
using Vintagestory.API.Util;

namespace VanillaVariants.Configuration;

public class Config
{
    public bool ExperimentalOverlayTest { get; set; } = true;

    public bool ResolveBarrelSounds { get; set; } = true;
    public bool ResolveBasketTrapIssues { get; set; } = true;
    public bool ResolveChestNames { get; set; } = true;
    public bool ResolveMechanicalBlockIssues { get; set; } = true;
    public bool ResolveQuernAndAxleRelationship { get; set; } = true;

    public bool CraftableCage { get; set; }
    public bool CraftableWagonWheels { get; set; }
    public bool CraftableWoodenRails { get; set; }

    public bool DecorativeQuern { get; set; } = true;
    public bool FunctionalQuern { get; set; }

    public bool ArchimedesScrew { get; set; } = true;
    public bool ArmorStand { get; set; } = true;
    public bool Barrel { get; set; } = true;
    public bool Basket { get; set; } = true;
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
    public bool HandBasket { get; set; } = true;
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
    public bool MetalDoor { get; set; } = true;
    public bool Moldrack { get; set; } = true;
    public bool OmokTabletop { get; set; } = true;
    public bool Palisade { get; set; } = true;
    public bool Shelf { get; set; } = true;
    public bool Sieve { get; set; } = true;
    public bool Sign { get; set; } = true;
    public bool Signpost { get; set; } = true;
    public bool SodRoofing { get; set; } = true;
    public bool StonePath { get; set; } = true;
    public bool SupportBeamMetal { get; set; } = true;
    public bool SupportChain { get; set; } = true;
    public bool Table { get; set; } = true;
    public bool Toolrack { get; set; } = true;
    public bool TroughLarge { get; set; } = true;
    public bool TroughSmall { get; set; } = true;
    public bool WagonWheels { get; set; } = true;
    public bool WoodBucket { get; set; } = true;
    public bool WoodenPan { get; set; } = true;
    public bool WoodenPath { get; set; } = true;
    public bool WoodenRails { get; set; } = true;

    public Dictionary<string, Dictionary<string, float>> ChuteFlowRates { get; set; } = new()
    {
        ["archimedesscrew"] = new() { ["default"] = 1, },
        ["chute"] = new() { ["default"] = 1, },
        ["hopper"] = new() { ["default"] = 1, },
    };

    public Dictionary<string, Dictionary<string, int>> ChuteQuantitySlots { get; set; } = new()
    {
        ["archimedesscrew"] = new() { ["default"] = 1, },
        ["chute"] = new() { ["default"] = 1, },
        ["hopper"] = new() { ["default"] = 4, },
    };

    public Dictionary<string, Dictionary<string, int>> ChuteCheckRateMs { get; set; } = new()
    {
        ["archimedesscrew"] = new() { ["default"] = 500, },
    };

    public Dictionary<string, Dictionary<string, bool>> ChuteCraftable { get; set; } = new()
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

    public bool OverrideChestQuantitySlots { get; set; }

    public Dictionary<string, int> ChestQuantitySlots { get; set; } = new()
    {
        ["default"] = 16
    };

    public bool OverrideDoubleChestQuantitySlots { get; set; }

    public Dictionary<string, int> DoubleChestQuantitySlots { get; set; } = new()
    {
        ["default"] = 36
    };

    // TODO
    // public bool OverrideMetalDoorsForSteelProduction { get; set; }

    // public Dictionary<string, bool> MetalDoorsForSteelProduction { get; set; } = new()
    // {
    //     ["meteoriciron"] = true,
    //     ["steel"] = true,
    // };

    public Config() { }

    public Config(Config previousConfig)
    {
        ChuteCheckRateMs.AddRange(previousConfig.ChuteCheckRateMs);
        ChuteCraftable.AddRange(previousConfig.ChuteCraftable);
        ChuteFlowRates.AddRange(previousConfig.ChuteFlowRates);
        ChuteQuantitySlots.AddRange(previousConfig.ChuteQuantitySlots);

        OverrideChestQuantitySlots = previousConfig.OverrideChestQuantitySlots;
        ChestQuantitySlots.AddRange(previousConfig.ChestQuantitySlots);

        OverrideDoubleChestQuantitySlots = previousConfig.OverrideDoubleChestQuantitySlots;
        DoubleChestQuantitySlots.AddRange(previousConfig.DoubleChestQuantitySlots);

        // TODO
        // OverrideMetalDoorsForSteelProduction = previousConfig.OverrideMetalDoorsForSteelProduction;
        // MetalDoorsForSteelProduction.AddRange(previousConfig.MetalDoorsForSteelProduction);

        ExperimentalOverlayTest = previousConfig.ExperimentalOverlayTest;
        ResolveBarrelSounds = previousConfig.ResolveBarrelSounds;
        ResolveBasketTrapIssues = previousConfig.ResolveBasketTrapIssues;
        ResolveChestNames = previousConfig.ResolveChestNames;
        ResolveMechanicalBlockIssues = previousConfig.ResolveMechanicalBlockIssues;
        ResolveQuernAndAxleRelationship = previousConfig.ResolveQuernAndAxleRelationship;
        CraftableCage = previousConfig.CraftableCage;
        CraftableWagonWheels = previousConfig.CraftableWagonWheels;
        CraftableWoodenRails = previousConfig.CraftableWoodenRails;
        DecorativeQuern = previousConfig.DecorativeQuern;
        FunctionalQuern = previousConfig.FunctionalQuern;

        ArchimedesScrew = previousConfig.ArchimedesScrew;
        ArmorStand = previousConfig.ArmorStand;
        Barrel = previousConfig.Barrel;
        Basket = previousConfig.Basket;
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
        HandBasket = previousConfig.HandBasket;
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
        MetalDoor = previousConfig.MetalDoor;
        Moldrack = previousConfig.Moldrack;
        OmokTabletop = previousConfig.OmokTabletop;
        Palisade = previousConfig.Palisade;
        Shelf = previousConfig.Shelf;
        Sieve = previousConfig.Sieve;
        Sign = previousConfig.Sign;
        Signpost = previousConfig.Signpost;
        SodRoofing = previousConfig.SodRoofing;
        StonePath = previousConfig.StonePath;
        SupportBeamMetal = previousConfig.SupportBeamMetal;
        SupportChain = previousConfig.SupportChain;
        Table = previousConfig.Table;
        Toolrack = previousConfig.Toolrack;
        TroughLarge = previousConfig.TroughLarge;
        TroughSmall = previousConfig.TroughSmall;
        WagonWheels = previousConfig.WagonWheels;
        WoodBucket = previousConfig.WoodBucket;
        WoodenPan = previousConfig.WoodenPan;
        WoodenPath = previousConfig.WoodenPath;
        WoodenRails = previousConfig.WoodenRails;
    }
}