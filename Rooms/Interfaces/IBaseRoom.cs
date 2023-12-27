using System.Collections.Concurrent;
using System.Net;
using GameOnlineServer.Applications.Interfaces;
using GameOnlineServer.Applications.Messaging;

namespace GameOnlineServer.Room.Interfaces;

public interface IBaseRoom
{
    public string Id{get;set;}
    public ConcurrentDictionary<string, IPlayer> Players {get;set;}
    bool JoinRoom(IPlayer player);
    bool ExitRoom(IPlayer player);
    bool ExitRoom(String id);
    IPlayer FindPlayer(string id);
    void SendMessage(string mes);
    void SendMessage<T>(WsMessage<T> message);
    void SendMessage<T>(WsMessage<T> message, string idIgnore);
}
