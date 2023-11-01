using Vintagestory.API.Common;

namespace VanillaVariants;

public class VariantTypeWildcard
{
    /// <summary>
    /// e.g. oak
    /// </summary>
    public string DefaultVariant { get; }
    /// <summary>
    /// e.g. wood
    /// </summary>
    public string Type { get; }
    /// <summary>
    /// e.g. new AssetLocation("plank-*")
    /// </summary>
    public AssetLocation Wildcard { get; }

    public VariantTypeWildcard(string defaultVariant, string type, AssetLocation wildcard)
    {
        DefaultVariant = defaultVariant;
        Type = type;
        Wildcard = wildcard;
    }
}
