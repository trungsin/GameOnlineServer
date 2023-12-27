using GameOnlineServer.Room.Handlers;
using MongoDB.Driver;

namespace GameOnlineServer.Room.Interfaces;

public interface IRoomManager
{
    BaseRoom Lobby {get;set;}
    BaseRoom FindRoom(string id);
    bool RemoveRoom(string id);
    BaseRoom CreateRoom();
}
