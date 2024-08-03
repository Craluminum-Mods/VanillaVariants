using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.GameContent;

namespace VanillaVariants;

public static class BlockEntityBarrel_toggleInventoryDialogClient_Patch
{
    public static bool Prefix(BlockEntityBarrel __instance, IPlayer byPlayer)
    {
        GuiDialogBarrel invDialog = __instance.GetField<GuiDialogBarrel>("invDialog");

        if (invDialog == null)
        {
            ICoreClientAPI capi = __instance.Api as ICoreClientAPI;
            invDialog = new GuiDialogBarrel(Lang.Get("Barrel"), __instance.Inventory, __instance.Pos, __instance.Api as ICoreClientAPI);
            invDialog.OnClosed += delegate
            {
                __instance.SetField("invDialog", null);
                capi.Network.SendBlockEntityPacket(__instance.Pos.X, __instance.Pos.Y, __instance.Pos.Z, 1001);
                capi.Network.SendPacketClient(__instance.Inventory.Close(byPlayer));
            };

            invDialog.OpenSound = __instance.Block.Attributes?["openSound"]?.AsString() != null
                ? AssetLocation.Create(__instance.Block.Attributes?["openSound"].AsString())
                : AssetLocation.Create("sounds/block/barrelopen", __instance.Block.Code.Domain);

            invDialog.CloseSound = __instance.Block.Attributes?["closeSound"]?.AsString() != null
                ? AssetLocation.Create(__instance.Block.Attributes?["closeSound"].AsString())
                : AssetLocation.Create("sounds/block/barrelclose", __instance.Block.Code.Domain);

            __instance.SetField("invDialog", invDialog);
            invDialog.TryOpen();
            capi.Network.SendPacketClient(__instance.Inventory.Open(byPlayer));
            capi.Network.SendBlockEntityPacket(__instance.Pos.X, __instance.Pos.Y, __instance.Pos.Z, 1000);
        }
        else
        {
            invDialog.TryClose();
        }

        return false;
    }
}