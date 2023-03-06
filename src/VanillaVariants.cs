using Vintagestory.API.Common;
using VanillaVariants.Configuration;

[assembly: ModInfo("Vanilla Variants",
    Authors = new[] { "Craluminum2413" })]

namespace VanillaVariants
{
    public class Core : ModSystem
    {
        public override void Start(ICoreAPI api)
        {
            base.Start(api);
            ModConfig.ReadConfig(api);
            api.RegisterBlockClass("VV_BlockStoneForge", typeof(BlockStoneForge));
            api.RegisterBlockClass("VV_BlockWoodBed", typeof(BlockWoodBed));
            api.RegisterBlockClass("VV_BlockWoodBucket", typeof(BlockWoodBucket));
            api.RegisterBlockClass("VV_BlockWoodCage", typeof(BlockWoodCage));
            api.RegisterBlockClass("VV_BlockWoodChair", typeof(BlockWoodChair));
            api.RegisterBlockClass("VV_BlockWoodDisplayCase", typeof(BlockWoodDisplayCase));
            api.RegisterBlockClass("VV_BlockWoodHenbox", typeof(BlockWoodHenbox));
            api.RegisterBlockClass("VV_BlockWoodLadder", typeof(BlockWoodLadder));
            api.RegisterBlockClass("VV_BlockWoodMoldRack", typeof(BlockWoodMoldRack));
            api.RegisterBlockClass("VV_BlockWoodPan", typeof(BlockWoodPan));
            api.RegisterBlockClass("VV_BlockWoodRails", typeof(BlockWoodRails));
            api.RegisterBlockClass("VV_BlockWoodShelf", typeof(BlockWoodShelf));
            api.RegisterBlockClass("VV_BlockWoodSign", typeof(BlockWoodSign));
            api.RegisterBlockClass("VV_BlockWoodSignPost", typeof(BlockWoodSignPost));
            api.RegisterBlockClass("VV_BlockWoodTable", typeof(BlockWoodTable));
            api.RegisterBlockClass("VV_BlockWoodToolRack", typeof(BlockWoodToolRack));
            api.RegisterBlockClass("VV_BlockWoodTrapdoor", typeof(BlockWoodTrapdoor));
            api.RegisterBlockClass("VV_BlockWoodTrough", typeof(BlockWoodTrough));
            api.RegisterBlockClass("VV_BlockWoodTroughDoubleBlock", typeof(BlockWoodTroughDoubleBlock));
            api.RegisterBlockClass("VV_BlockWoodWagonWheels", typeof(BlockWoodWagonWheels));
            api.World.Logger.Event("started 'Vanilla Variants' mod");
        }
    }
}