using Vintagestory.API.Common;
using VanillaVariants.Configuration;

[assembly: ModInfo("Vanilla Variants",
  Authors = new[] { "Craluminum2413" })]

namespace VanillaVariants
{
  public class Core : ModSystem
	{
    public override void StartPre(ICoreAPI api)
		{
			base.StartPre(api);
			ModConfig.ReadConfig(api);
			api.World.Logger.Event("started 'Vanilla Variants' mod");
		}
	}
}