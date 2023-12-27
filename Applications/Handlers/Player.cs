using System.Text;
using GameDatabase.Mongodb.Handlers;
using GameOnlineServer.Applications.Interfaces;
using GameOnlineServer.Applications.Messaging;
using GameOnlineServer.GameModels;
using GameOnlineServer.Logging;
using MongoDB.Driver;
using NetCoreServer;
using GameDatabase.Mongodb.Interfaces;
using GameOnlineServer.GameModels.Handlers;
using GameOnlineServer.Messaging.Constants;

namespace GameOnlineServer.Applications.Handlers;

public class Player : WsSession, IPlayer
{
    public string SessionId { get ; set; }
    public string Name { get; set ; }
    private bool IsDisconnected { get; set; }
    private readonly IGameLogger _logger;
    private UserHandler UsersDb { get; set; }
    private User UserInfo { get; set; }
   public Player(WsServer server, IMongoDatabase database) : base(server)
    {
        SessionId = this.Id.ToString();
        IsDisconnected = false;
        _logger = new GameLogger();
        UsersDb = new UserHandler(database);
    }
    public override void OnWsDisconnected()
    {
        OnDisconnect();
        base.OnWsDisconnected();
    }
    public override void OnWsConnected(HttpRequest request)
    {
        //todo logic an  player connected
        _logger.Info("Player connected");
        IsDisconnected = false;
    }
    public void OnDisconnect()
    {
        //todo logic handle player disconnect
        var lobby = ((WsGameServer) Server).RoomManager.Lobby;
        lobby.ExitRoom(this);
        _logger.Warning("Player disconnected", null);
    }
    public override void OnWsReceived(byte[] buffer, long offset, long size)
    {
        string mess = Encoding.UTF8.GetString(buffer, index:(int) offset, count:(int)size);
        try{
            var wsMessage = GameHelper.ParseStruct<WsMessage<object>>(mess);
            switch (wsMessage.Tags)
            {
                case WsTags.Invalid:
                    break;
                case WsTags.Login:
                    var loginData = GameHelper.ParseStruct<LoginData>(wsMessage.Data.ToString());
                    UserInfo  = UsersDb.FindByUserName(loginData.Username);
                    if(UserInfo != null){
                        var hashPass = GameHelper.HashPassword(loginData.Password);
                        if(hashPass == UserInfo.Password){
                            var messInfo =new WsMessage<UserInfo>(WsTags.UserInfo,this.GetUserInfo());
                            this.SendMessage(messInfo);
                            this.PlayerJoinLobby();
                            return;
                        }
                    }
                    var invalidMess =  new WsMessage<string>(WsTags.Invalid,data:"UserName or Password is Invalid");
                    this.SendMessage(mes:GameHelper.ParseString(invalidMess));
                    break;
                case WsTags.Registry:
                    var regData = GameHelper.ParseStruct<RegisterData>(wsMessage.Data.ToString());
                    if (UserInfo != null)
                    {
                        invalidMess = new WsMessage<string>(WsTags.Invalid, "You are Loginned");
                        this.SendMessage(GameHelper.ParseString(invalidMess));
                        return;
                    }
                    var check = UsersDb.FindByUserName(regData.Username);
                    if (check != null)
                    {
                        invalidMess = new WsMessage<string>(WsTags.Invalid, "Username exits");
                        this.SendMessage(GameHelper.ParseString(invalidMess));
                        return;
                    }

                    var newUser = new User(regData.Username, regData.Password, regData.DisplayName);
                    UserInfo = UsersDb.Create(newUser);
                    if (UserInfo != null)
                    {
                        //todo move user to lobby
                        this.PlayerJoinLobby();
                    }
                    break;
                case  WsTags.UserInfo:
                     var lineData = GameHelper.ParseStruct<LoginData>(wsMessage.Data.ToString());
                    break;
                case WsTags.RoomInfo:
                    break;
                default:
                    break;
                    //throw new ArgumentOutOfRangeException();
            }
        } catch (Exception e){
            _logger.Error("OnWsReceived error", e);
        }
    }
    private void PlayerJoinLobby(){
        //todo logic Lobby
        var lobby = ((WsGameServer) Server).RoomManager.Lobby;
        lobby.JoinRoom(this);
    }
    public void SetDisconnect(bool value)
    {
        this.IsDisconnected = value;
    }

    public bool SendMessage(string mes)
    {
        return this.SendTextAsync(mes);
    }

    public UserInfo GetUserInfo()
    {
        if (UserInfo != null)
        {
            return new UserInfo
            {
                DisplayName = UserInfo.DisplayName,
                Amount = UserInfo.Amount,
                Avatar = UserInfo.Avatar,
                Level = UserInfo.Level,
            };
        }
        return new UserInfo();
    }

    public bool SendMessage<T>(WsMessage<T> message)
    {
        var mes = GameHelper.ParseString(message);
        return this.SendMessage(mes);
    }
}
