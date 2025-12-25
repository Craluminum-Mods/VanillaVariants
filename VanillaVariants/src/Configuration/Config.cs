using ProtoBuf;
using System.Collections.Generic;
using Vintagestory.API.Util;

namespace VanillaVariants.Configuration;

[ProtoContract]
public class Config
{
    [ProtoMember(1)]
    public bool ResolveChestNames { get; set; } = true;
    
    [ProtoMember(2)]
    public bool ResolveMechanicalBlockIssues { get; set; } = true;
    
    [ProtoMember(3)]
    public bool ResolveQuernAndAxleRelationship { get; set; } = true;

    [ProtoMember(4)]
    public bool CraftableCage { get; set; }

    [ProtoMember(5)]
    public bool CraftableWoodenRails { get; set; }

    [ProtoMember(6)]
    public bool ArchimedesScrew { get; set; } = true;
    
    [ProtoMember(7)]
    public bool ArmorStand { get; set; } = true;
    
    [ProtoMember(8)]
    public bool Barrel { get; set; } = true;
    
    [ProtoMember(9)]
    public bool Bed { get; set; } = true;
    
    [ProtoMember(10)]
    public bool Cage { get; set; } = true;
    
    [ProtoMember(11)]
    public bool Chair { get; set; } = true;
    
    [ProtoMember(12)]
    public bool Chest { get; set; } = true;
    
    [ProtoMember(13)]
    public bool Chute { get; set; } = true;
    
    [ProtoMember(14)]
    public bool ChuteSectionItem { get; set; } = true;
    
    [ProtoMember(15)]
    public bool CrudeDoor { get; set; } = true;
    
    [ProtoMember(16)]
    public bool DisplayCase { get; set; } = true;
    
    [ProtoMember(17)]
    public bool Forge { get; set; } = true;
    
    [ProtoMember(18)]
    public bool FruitPress { get; set; } = true;
    
    [ProtoMember(19)]
    public bool Henbox { get; set; } = true;
    
    [ProtoMember(20)]
    public bool Hopper { get; set; } = true;
    
    [ProtoMember(21)]
    public bool Ladder { get; set; } = true;
    
    [ProtoMember(22)]
    public bool MechanicalAngledGears { get; set; } = true;
    
    [ProtoMember(23)]
    public bool MechanicalAxle { get; set; } = true;
    
    [ProtoMember(24)]
    public bool MechanicalBrake { get; set; } = true;
    
    [ProtoMember(25)]
    public bool MechanicalClutch { get; set; } = true;
    
    [ProtoMember(26)]
    public bool MechanicalHelveHammerBase { get; set; } = true;
    
    [ProtoMember(27)]
    public bool MechanicalHelveHammerItem { get; set; } = true;
    
    [ProtoMember(28)]
    public bool MechanicalLargeGear { get; set; } = true;
    
    [ProtoMember(29)]
    public bool MechanicalLargeGearSectionItem { get; set; } = true;
    
    [ProtoMember(30)]
    public bool MechanicalPulverizer { get; set; } = true;
    
    [ProtoMember(31)]
    public bool MechanicalToggle { get; set; } = true;
    
    [ProtoMember(32)]
    public bool MechanicalTransmission { get; set; } = true;
    
    [ProtoMember(33)]
    public bool MechanicalWindmillRotor { get; set; } = true;
    
    [ProtoMember(34)]
    public bool MetalDoor { get; set; } = true;
    
    [ProtoMember(35)]
    public bool Moldrack { get; set; } = true;
    
    [ProtoMember(36)]
    public bool OmokTabletop { get; set; } = true;
    
    [ProtoMember(37)]
    public bool Palisade { get; set; } = true;
    
    [ProtoMember(38)]
    public bool Quern { get; set; } = true;
    
    [ProtoMember(39)]
    public bool Shelf { get; set; } = true;
    
    [ProtoMember(40)]
    public bool Sieve { get; set; } = true;
    
    [ProtoMember(41)]
    public bool Sign { get; set; } = true;
    
    [ProtoMember(42)]
    public bool Signpost { get; set; } = true;
    
    [ProtoMember(43)]
    public bool SodRoofing { get; set; } = true;
    
    [ProtoMember(44)]
    public bool StonePath { get; set; } = true;
    
    [ProtoMember(45)]
    public bool SupportBeamMetal { get; set; } = true;
    
    [ProtoMember(46)]
    public bool SupportChain { get; set; } = true;
    
    [ProtoMember(47)]
    public bool Table { get; set; } = true;
    
    [ProtoMember(48)]
    public bool Toolrack { get; set; } = true;
    
    [ProtoMember(49)]
    public bool TroughLarge { get; set; } = true;
    
    [ProtoMember(50)]
    public bool TroughSmall { get; set; } = true;
    
    [ProtoMember(51)]
    public bool WoodBucket { get; set; } = true;
    
    [ProtoMember(52)]
    public bool WoodenPan { get; set; } = true;
    
    [ProtoMember(53)]
    public bool WoodenRails { get; set; } = true;

    [ProtoMember(54)]
    public List<string> ChuteCraftable { get; set; } = 
    [
        "tinbronze", "bismuthbronze", "blackbronze", "iron", "meteoriciron", "steel"
    ];

    public Config() { }

    public Config(Config previousConfig)
    {
        ChuteCraftable = previousConfig.ChuteCraftable;

        ResolveChestNames = previousConfig.ResolveChestNames;
        ResolveMechanicalBlockIssues = previousConfig.ResolveMechanicalBlockIssues;
        ResolveQuernAndAxleRelationship = previousConfig.ResolveQuernAndAxleRelationship;
        CraftableCage = previousConfig.CraftableCage;
        CraftableWoodenRails = previousConfig.CraftableWoodenRails;

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
        MetalDoor = previousConfig.MetalDoor;
        Moldrack = previousConfig.Moldrack;
        OmokTabletop = previousConfig.OmokTabletop;
        Palisade = previousConfig.Palisade;
        Quern = previousConfig.Quern;
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
        WoodBucket = previousConfig.WoodBucket;
        WoodenPan = previousConfig.WoodenPan;
        WoodenRails = previousConfig.WoodenRails;
    }
}