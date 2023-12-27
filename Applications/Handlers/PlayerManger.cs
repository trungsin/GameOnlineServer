using System.Collections.Concurrent;
using GameOnlineServer.Applications.Handlers;
using GameOnlineServer.Applications.Interfaces;
using GameOnlineServer.Logging;

namespace GameOnlineServer;

public class PlayerManger : IPlayerManager
{
   
    public ConcurrentDictionary<string, IPlayer> Players {get;set;}
    private readonly IGameLogger _logger;

    public PlayerManger(IGameLogger logger){
        Players = new ConcurrentDictionary<string, IPlayer>();
        _logger = logger;
    }
    public void AddPlayer(IPlayer player)
    {
        if(FindPlayer(player) == null){
            Players.TryAdd(player.SessionId, player);
            _logger.Info($"List Players {Players.Count}");
        }
    }

    public IPlayer FindPlayer(string id)
    {
        return Players.FirstOrDefault(p => p.Key.Equals(id)).Value;
    }

    public IPlayer FindPlayer(IPlayer player)
    {
        return Players.FirstOrDefault(p => p.Key.Equals(player)).Value;
    }

    public List<IPlayer> GetPlayers()  => Players.Values.ToList();

    public void RemovePlayer(string id)
    {
        if(FindPlayer(id) != null){
            Players.TryRemove(id, out var player);
            if(player != null){
                //to do logic record a log for remove player
                _logger.Info($"Remove {id} success");
                _logger.Info($"List Players {Players.Count}");
            }
        }
    }
        

    public void RemovePlayer(IPlayer player)
    {
        this.RemovePlayer(player.SessionId);
    }
}
