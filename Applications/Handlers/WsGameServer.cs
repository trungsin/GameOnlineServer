using System.Net;
using System.Net.Sockets;
using GameDatabase.Mongodb.Handlers;
using GameDatabase.Mongodb.Interfaces;
using GameOnlineServer.Applications.Interfaces;
using GameOnlineServer.GameModels;
using GameOnlineServer.Logging;
using GameOnlineServer.Room.Interfaces;
using NetCoreServer;

namespace GameOnlineServer.Applications.Handlers;

public class WsGameServer : WsServer, IWsGameServer
{
    private int _port;
    public readonly IPlayerManager PlayerManager;
    public readonly IGameLogger _logger;
    
    private string _ipaddress;
    private readonly MongoDb _mongoDb;
    public readonly IRoomManager RoomManager;
    public WsGameServer(IPAddress address, int port, IPlayerManager playerManager, IGameLogger logger, MongoDb mongoDB, IRoomManager roomManager) : base(address, port)
    {
        _port = port;
        _ipaddress = address.ToString();
        PlayerManager = playerManager;
        _logger = logger;
        _mongoDb = mongoDB;
        RoomManager = roomManager;

    }
    protected override TcpSession CreateSession()
    {
        //todo handle new session
        _logger.Info("New session connected");
        var player = new Player(this,_mongoDb.GetDatabase());
        PlayerManager.AddPlayer(player);
        return player;
    }
    protected override void OnDisconnected(TcpSession session)
    {
        _logger.Info("Session disconnected");
        var player = PlayerManager.FindPlayer(session.Id.ToString());
        if(player != null){
            PlayerManager.RemovePlayer(player);
        }
        base.OnDisconnected(session);
    }
    public void RestartServer()
    {
        //todo logic before start server
        if(this.Restart()){
            _logger.Print($"Server Ws restarted at {_ipaddress+":"+_port}");
            return;
        }
    }
    public void SendAll(string mess){
        this.MulticastText(mess);
    }
    public void StartServer()
    {
        //todo logic before start server
        if(this.Start()){
            _logger.Print($"Server Ws started at {_ipaddress+":"+_port}");
            return;
        }
        
    }
    protected override void OnError(SocketError error)
    {
        _logger.Error($"Server Ws error");
        base.OnError(error);
    }
    public void StopServer()
    {
        //todo logic before stop server
        this.Stop();
        _logger.Print("Server Ws stopped");

    }
}
