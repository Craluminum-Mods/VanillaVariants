using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;

namespace VanillaVariants;

public class CollectibleBehaviorName : CollectibleBehavior
{
    private string translationName;
    private string translationPart;
    private string newName;

    public CollectibleBehaviorName(CollectibleObject collObj) : base(collObj) { }

    public override void Initialize(JsonObject properties)
    {
        base.Initialize(properties);

        translationName = properties["translationName"].AsString();
        translationPart = properties["translationPart"].AsString();

        newName = Lang.GetMatchingIfExists(translationName) + " (" + Lang.Get(translationPart) + ")";
    }

    public override void GetHeldItemName(StringBuilder sb, ItemStack itemStack)
    {
        sb.Clear();
        sb.Append(newName);
    }
}