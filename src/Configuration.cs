using System.Collections.Generic;
using Vintagestory.API.Common;
using VanillaVariants.Constructor;
using VanillaVariants.List;

[assembly: ModInfo("VanillaVariants",
  Description = "Adds piles for some items and blocks",
  Website = "https://github.com/Craluminum2413/VanillaVariants",
  Authors = new[] { "Craluminum2413" })]

namespace VanillaVariants
{
  public class Core : ModSystem
	{
    public override void StartPre(ICoreAPI api)
		{
			base.StartPre(api);

			VanillaVariantsConfig settingsFromDisk;

			try
			{
				settingsFromDisk = api.LoadModConfig<VanillaVariantsConfig>("VanillaVariantsConfig.json");
			}
			catch
			{
				settingsFromDisk = null;
			}

			if (settingsFromDisk is null)
			{
				settingsFromDisk = VanillaVariantsConfig.Loaded;
				api.StoreModConfig<VanillaVariantsConfig>(VanillaVariantsConfig.Loaded, "VanillaVariantsConfig.json");
			}

			foreach (KeyValuePair<string, Part> p in settingsFromDisk)
			{
				api.World.Config.SetBool($"VV{p.Key}Enabled", p.Value.Enabled);
			}
		}
	}
}