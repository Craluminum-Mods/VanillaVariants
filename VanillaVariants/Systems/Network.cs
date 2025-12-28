using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.Server;
using VanillaVariants.Configuration;

namespace VanillaVariants;

public class Network : ModSystem
{
    public override void Start(ICoreAPI api)
    {
        api.Network
            .RegisterChannel("vanvar")
            .RegisterMessageType(typeof(Config));
    }

    #region Client
    IClientNetworkChannel clientChannel;
    ICoreClientAPI clientApi;

    public override void StartClientSide(ICoreClientAPI api)
    {
        clientApi = api;
        clientChannel = api.Network
            .GetChannel("vanvar")
            .SetMessageHandler<Config>(OnServerResponse);
    }

    private void OnServerResponse(Config configFromServer)
    {
        if (configFromServer == null) return;

        // ignore packets in SinglePlayer
        if (clientApi.IsSinglePlayer) return;

        Core.Config = configFromServer;
        Configuration.ModConfig.WriteConfig(clientApi, configFromServer);
    }
    #endregion

    #region Server
    IServerNetworkChannel serverChannel;
    ICoreServerAPI serverApi;

    public override void StartServerSide(ICoreServerAPI api)
    {
        serverApi = api;
        serverChannel = api.Network
            .GetChannel("vanvar")
            .SetMessageHandler<Config>(OnClientRequest);
    }

    private void OnClientRequest(IPlayer fromPlayer, Config configFromClient)
    {
        if (!fromPlayer.HasPrivilege(Privilege.controlserver)) return;
        if (configFromClient == null) return;

        // ignore packets in SinglePlayer
        if ((serverApi.World as ServerMain).Clients.Any(p => p.Value.IsSinglePlayerClient)) return;

        Configuration.ModConfig.WriteConfig(serverApi, configFromClient);
        Core.Config = configFromClient;

        IServerPlayer[] allPlayers = [.. serverApi.World.AllOnlinePlayers
            .Select(x => x as IServerPlayer)
            .Where(p => p.ConnectionState == EnumClientState.Connected 
            && p.PlayerUID != fromPlayer.PlayerUID
            )];

        if (allPlayers != null && allPlayers.Length > 0)
        {
            serverChannel.SendPacket(configFromClient, allPlayers);
        }
    }
    #endregion
}
