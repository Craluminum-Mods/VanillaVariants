using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using VanillaVariants.Constructor;
using Vintagestory.API.Common;

namespace VanillaVariants.List
{
  [JsonObject]
	public class VanillaVariantsConfig : ModSystem, IEnumerable<KeyValuePair<string, Part>>
	{
		public static VanillaVariantsConfig Loaded { get; set; } = new VanillaVariantsConfig();

		public Part MoreRecipes_feature { get; set; } = new Part(false, "Turn on/off recipes like in More Recipes mod");
		public Part SlidingDoor_compatibility { get; set; } = new Part(false, "Turn on/off compatibility with Sliding Doors if it is loaded");
		public Part TradersSellVariants { get; set; } = new Part(false, "Whether traders sell variants or not");
		public Part Barrel { get; set; } = new Part(true);
		public Part Bed { get; set; } = new Part(true);
		public Part Cage { get; set; } = new Part(true);
		public Part Chair { get; set; } = new Part(true);
		public Part DisplayCase { get; set; } = new Part(true);
		public Part Door { get; set; } = new Part(true);
		public Part Forge { get; set; } = new Part(true);
		public Part Henbox { get; set; } = new Part(true);
		public Part Ladder { get; set; } = new Part(true);
		public Part Moldrack { get; set; } = new Part(true);
		public Part Shelf { get; set; } = new Part(true);
		public Part Sign { get; set; } = new Part(true);
		public Part Signpost { get; set; } = new Part(true);
		public Part Table { get; set; } = new Part(true);
		public Part Toolrack { get; set; } = new Part(true);
		public Part Trapdoor { get; set; } = new Part(true);
		public Part TroughLarge { get; set; } = new Part(true);
		public Part TroughSmall { get; set; } = new Part(true);
		public Part WagonWheels { get; set; } = new Part(true);
		public Part WoodBucket { get; set; } = new Part(true);
		public Part WoodenClub { get; set; } = new Part(true);
		public Part WoodenPan { get; set; } = new Part(true);
		public Part WoodenRails { get; set; } = new Part(true);

		private static readonly PropertyInfo[] propertyInfos = typeof(VanillaVariantsConfig).GetProperties()
			.Where(propertyInfo => propertyInfo.PropertyType == typeof(Part)).ToArray();

		public IEnumerator<KeyValuePair<string, Part>> GetEnumerator()
		{
			return propertyInfos.Select(propertyInfo => new KeyValuePair<string, Part>(propertyInfo.Name, (Part)propertyInfo.GetValue(this))).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}