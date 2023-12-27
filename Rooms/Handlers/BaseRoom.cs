using System.Collections.Concurrent;
using GameOnlineServer.Applications.Handlers;
using GameOnlineServer.Applications.Interfaces;
using GameOnlineServer.Applications.Messaging;
using GameOnlineServer.Messaging.Constants;
using GameOnlineServer.Room.Interfaces;

namespace GameOnlineServer.Room.Handlers;

public class BaseRoom : IBaseRoom
{
    public string Id { get; set; }
    public ConcurrentDictionary<string, IPlayer> Players { get; set; }
    public BaseRoom()
    {
        Id = GameHelper.RandomString(len: 10);
        Players = new ConcurrentDictionary<string, IPlayer>();
    }
    public bool ExitRoom(IPlayer player)
    {
        return this.ExitRoom(player.SessionId);
    }

    public bool ExitRoom(string id)
    {
        var player = FindPlayer(id);
        if(player != null){
            Players.TryRemove(player.SessionId,out player);
            return true;
        }
        return false;
    }

    public IPlayer FindPlayer(string id)
    {
        return Players.FirstOrDefault(p => p.Key.Equals(id)).Value;
    }

    public bool JoinRoom(IPlayer player)
    {
        if ((FindPlayer(player.SessionId) == null) && Players.TryAdd(player.SessionId, player))
        {
           this.RoomInfo();
           return true;
        }
        return false;
    }
private void RoomInfo(){
    var lobby = new RoomInfo()
    {
        Players = Players.Values.Select(p => p.GetUserInfo()).ToList()
    };
    var mes = new WsMessage<RoomInfo>(WsTags.RoomInfo, lobby);
    this.SendMessage(mes);
}
    public void SendMessage(string mes)
    {
        lock(Players){
            foreach(var player in Players.Values){
                player.SendMessage(mes);
            }
        }
    }

    public void SendMessage<T>(WsMessage<T> message)
    {
        lock(Players)
        {
            foreach(var player in Players.Values)
            {
                player.SendMessage(message);
            }
        }
    }

    public void SendMessage<T>(WsMessage<T> message, string idIgnore)
    {
        lock(Players)
        {
            foreach(var player in Players.Values.Where(p=>p.SessionId != idIgnore))
            {
                player.SendMessage(message);
            }
        }

    }
}
