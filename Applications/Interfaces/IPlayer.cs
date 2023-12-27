using GameOnlineServer.Applications.Messaging;
using GameOnlineServer.Messaging.Constants;

namespace GameOnlineServer.Applications.Interfaces;

public interface IPlayer
{
    public string SessionId{get;set;}
    public string Name{get;set;}
    void SetDisconnect(bool value);
    public bool SendMessage(string mes);
    public bool SendMessage<T>(WsMessage<T> message);
    void OnDisconnect();
    UserInfo GetUserInfo();


}
