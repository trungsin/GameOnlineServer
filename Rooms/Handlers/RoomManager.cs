using System.Collections.Concurrent;
using GameOnlineServer.Room.Interfaces;

namespace GameOnlineServer.Room.Handlers;

public class RoomManager : IRoomManager
{
    public BaseRoom Lobby { get; set; }
    private ConcurrentDictionary<string,BaseRoom> Rooms{get;set;}
    public RoomManager(){
        Rooms = new ConcurrentDictionary<string, BaseRoom>();
        Lobby = new BaseRoom();
    }
    public BaseRoom FindRoom(string id)
    {
        return Rooms.FirstOrDefault(r => r.Key == id).Value;
    }

    public bool RemoveRoom(string id)
    {
        var oldRoom =FindRoom(id);
        if(oldRoom != null){
            Rooms.TryRemove(id, out var room);
            return room != null;
        }
        return false;
    }

    public BaseRoom CreateRoom()
    {
        var newRoom = new BaseRoom();
        Rooms.TryAdd(newRoom.Id, newRoom);
        return newRoom;
    }
}
