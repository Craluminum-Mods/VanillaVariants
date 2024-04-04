using Vintagestory.API.Common;

namespace VanillaVariants;

public class RecipePatch
{
    public bool NotReplaceDefault;

    public string OutputCode = null;
    public string IngredientCode = null;

    public string OldCode = null;
    public string[] OldAllowedVariants = null;

    public string NewOutputCode = null;
    public string NewCode = null;
    public string NewName = null;
    public string[] NewAllowedVariants = null;

    public AssetLocation GetIngredientCode() => new AssetLocation(IngredientCode);

    public AssetLocation GetOldCode() => string.IsNullOrEmpty(OldCode) ? GetIngredientCode() : new AssetLocation(OldCode);
    public AssetLocation GetOutputCode() => new AssetLocation(OutputCode);

    public AssetLocation GetNewCode() => string.IsNullOrEmpty(NewCode) ? GetIngredientCode() : new AssetLocation(NewCode);
    public AssetLocation GetNewOutputCode() => new AssetLocation(NewOutputCode);
}