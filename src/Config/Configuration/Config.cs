namespace VanillaVariants.Configuration;

public class Config
{
    public bool ExperimentalOverlayTest { get; set; } = true;
    public bool MoreRecipesFeature { get; set; }

    public bool ArmorStand { get; set; } = true;
    public bool Bed { get; set; } = true;
    public bool Cage { get; set; } = true;
    public bool Chair { get; set; } = true;
    public bool DecorativeQuern { get; set; } = true;
    public bool DisplayCase { get; set; } = true;
    public bool Forge { get; set; } = true;
    public bool Henbox { get; set; } = true;
    public bool Ladder { get; set; } = true;
    public bool Moldrack { get; set; } = true;
    public bool OmokTabletop { get; set; } = true;
    public bool Palisade { get; set; } = true;
    public bool Shelf { get; set; } = true;
    public bool Sign { get; set; } = true;
    public bool Signpost { get; set; } = true;
    public bool SodRoofing { get; set; } = true;
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
        MoreRecipesFeature = previousConfig.MoreRecipesFeature;

        ArmorStand = previousConfig.ArmorStand;
        Bed = previousConfig.Bed;
        Cage = previousConfig.Cage;
        Chair = previousConfig.Chair;
        DecorativeQuern = previousConfig.DecorativeQuern;
        DisplayCase = previousConfig.DisplayCase;
        Forge = previousConfig.Forge;
        Henbox = previousConfig.Henbox;
        Ladder = previousConfig.Ladder;
        Moldrack = previousConfig.Moldrack;
        OmokTabletop = previousConfig.OmokTabletop;
        Palisade = previousConfig.DecorativeQuern;
        Shelf = previousConfig.Shelf;
        Sign = previousConfig.Sign;
        Signpost = previousConfig.Signpost;
        SodRoofing = previousConfig.SodRoofing;
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