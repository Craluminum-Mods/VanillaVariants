using Vintagestory.API.Common;

namespace VanillaVariants;

public class IngredientPatch
{
    public string IngredientCode = null;
    public string Code = null;
    public string Name = null;
    public string[] AllowedVariants = null;
    public string[] SkipVariants = null;

    public AssetLocation GetIngredientCode() => new AssetLocation(IngredientCode);
    public AssetLocation GetCode() => string.IsNullOrEmpty(Code) ? GetIngredientCode() : new AssetLocation(Code);
}