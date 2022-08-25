namespace VanillaVariants.Configuration
{
  class VanillaVariantsConfig
  {
    public bool MoreRecipesFeature;
    public bool SlidingDoorCompatibility;
    public bool TradersSellVariants;
    public bool Barrel = true;
    public bool Bed = true;
    public bool Cage = true;
    public bool Chair = true;
    public bool DisplayCase = true;
    public bool Door = true;
    public bool Forge = true;
    public bool Henbox = true;
    public bool Ladder = true;
    public bool Moldrack = true;
    public bool Shelf = true;
    public bool Sign = true;
    public bool Signpost = true;
    public bool Table = true;
    public bool Toolrack = true;
    public bool Trapdoor = true;
    public bool TroughLarge = true;
    public bool TroughSmall = true;
    public bool WagonWheels = true;
    public bool WoodBucket = true;
    public bool WoodenClub = true;
    public bool WoodenPan = true;
    public bool WoodenRails = true;

    public VanillaVariantsConfig() { }

    public VanillaVariantsConfig(VanillaVariantsConfig previousConfig)
    {
      MoreRecipesFeature = previousConfig.MoreRecipesFeature;
      SlidingDoorCompatibility = previousConfig.SlidingDoorCompatibility;
      TradersSellVariants = previousConfig.TradersSellVariants;
      Barrel = previousConfig.Barrel;
      Bed = previousConfig.Bed;
      Cage = previousConfig.Cage;
      Chair = previousConfig.Chair;
      DisplayCase = previousConfig.DisplayCase;
      Door = previousConfig.Door;
      Forge = previousConfig.Forge;
      Henbox = previousConfig.Henbox;
      Ladder = previousConfig.Ladder;
      Moldrack = previousConfig.Moldrack;
      Shelf = previousConfig.Shelf;
      Sign = previousConfig.Sign;
      Signpost = previousConfig.Signpost;
      Table = previousConfig.Table;
      Toolrack = previousConfig.Toolrack;
      Trapdoor = previousConfig.Trapdoor;
      TroughLarge = previousConfig.TroughLarge;
      TroughSmall = previousConfig.TroughSmall;
      WagonWheels = previousConfig.WagonWheels;
      WoodBucket = previousConfig.WoodBucket;
      WoodenClub = previousConfig.WoodenClub;
      WoodenPan = previousConfig.WoodenPan;
      WoodenRails = previousConfig.WoodenRails;
    }
  }
}