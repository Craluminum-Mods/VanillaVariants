using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.GameContent.Mechanics;

namespace VanillaVariants;

public static class HelveHammer_TexturePosition_Patch
{
    public static bool Prefix(BEHelveHammer __instance, ref TextureAtlasPosition __result, string textureCode)
    {
        ItemStack hammerStack = __instance.GetField<ItemStack>("hammerStack");
        ICoreClientAPI capi = __instance.GetField<ICoreClientAPI>("capi");
        ITexPositionSource blockTexSource = __instance.GetField<ITexPositionSource>("blockTexSource");

        if ((textureCode == "metal" || textureCode == "oak" || textureCode == "oak1") && hammerStack.Item.Textures.TryGetValue(textureCode, out var ctex))
        {
            AssetLocation texturePath = ctex.Base;
            __result = capi.BlockTextureAtlas[texturePath];
            return false;
        }
        __result = blockTexSource[textureCode];
        return false;
    }
}