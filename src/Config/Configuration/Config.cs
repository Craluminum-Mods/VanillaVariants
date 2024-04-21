namespace VanillaVariants.Configuration;

public class Config
{
    public bool ExperimentalOverlayTest { get; set; } = true;

    public bool ResolveHenboxImposter { get; set; } = true;
    public bool ResolveMechanicalBlockIssues { get; set; } = true;
    public bool ResolveQuernAndAxleRelationship { get; set; } = true;

    public bool CraftableCage { get; set; }
    public bool CraftableWagonWheels { get; set; }
    public bool CraftableWoodenRails { get; set; }

    public bool ArmorStand { get; set; } = true;
    public bool Bed { get; set; } = true;
    public bool Cage { get; set; } = true;
    public bool Chair { get; set; } = true;
    public bool CrudeDoor { get; set; } = true;
    public bool DecorativeQuern { get; set; } = true;
    public bool DisplayCase { get; set; } = true;
    public bool Firewood { get; set; } = true;
    public bool Forge { get; set; } = true;
    public bool Henbox { get; set; } = true;
    public bool Ladder { get; set; } = true;
    public bool MechanicalAngledGears { get; set; } = true;
    public bool MechanicalAxle { get; set; } = true;
    public bool MechanicalBrake { get; set; } = true;
    public bool MechanicalClutch { get; set; } = true;
    public bool MechanicalHelveHammerBase { get; set; } = true;
    public bool MechanicalHelveHammerItem { get; set; } = true;
    public bool MechanicalToggle { get; set; } = true;
    public bool MechanicalTransmission { get; set; } = true;
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

    public Config() { }

    public Config(Config previousConfig)
    {
        ExperimentalOverlayTest = previousConfig.ExperimentalOverlayTest;

        ResolveHenboxImposter = previousConfig.ResolveHenboxImposter;
        ResolveMechanicalBlockIssues = previousConfig.ResolveMechanicalBlockIssues;
        ResolveQuernAndAxleRelationship = previousConfig.ResolveQuernAndAxleRelationship;

        CraftableCage = previousConfig.CraftableCage;
        CraftableWagonWheels = previousConfig.CraftableWagonWheels;
        CraftableWoodenRails = previousConfig.CraftableWoodenRails;

        ArmorStand = previousConfig.ArmorStand;
        Bed = previousConfig.Bed;
        Cage = previousConfig.Cage;
        Chair = previousConfig.Chair;
        CrudeDoor = previousConfig.CrudeDoor;
        DecorativeQuern = previousConfig.DecorativeQuern;
        DisplayCase = previousConfig.DisplayCase;
        Firewood = previousConfig.Firewood;
        Forge = previousConfig.Forge;
        Henbox = previousConfig.Henbox;
        Ladder = previousConfig.Ladder;
        MechanicalAngledGears = previousConfig.MechanicalAngledGears;
        MechanicalAxle = previousConfig.MechanicalAxle;
        MechanicalBrake = previousConfig.MechanicalBrake;
        MechanicalClutch = previousConfig.MechanicalClutch;
        MechanicalHelveHammerBase = previousConfig.MechanicalHelveHammerBase;
        MechanicalHelveHammerItem = previousConfig.MechanicalHelveHammerItem;
        MechanicalToggle = previousConfig.MechanicalToggle;
        MechanicalTransmission = previousConfig.MechanicalTransmission;
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