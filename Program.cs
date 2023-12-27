// See https://aka.ms/new-console-template for more information
using System;
using System.Net;
using GameOnlineServer.Applications.Handlers;
using GameOnlineServer.Applications.Interfaces;
using GameOnlineServer;
using GameOnlineServer.Logging;
using GameDatabase.Mongodb.Handlers;
using GameDatabase;
using GameOnlineServer.GameModels;
using GameOnlineServer.Room.Interfaces;
using GameOnlineServer.Room.Handlers;

// TCP server port
int port = 1111;
if (args.Length > 0)
    port = int.Parse(args[0]);

Console.WriteLine($"TCP server port: {port}");

Console.WriteLine();
IGameLogger logger =new GameLogger();
var mongodb = new MongoDb();
var mongoHandler = new MongoHandler<User>(mongodb.GetDatabase());
IPlayerManager playerManager = new PlayerManger(logger);
IRoomManager roomManager = new RoomManager();
// Create a new TCP chat server
var server = new WsGameServer(IPAddress.Any, port, playerManager,logger,mongodb,roomManager);
logger.Print(msg:"Game Server started");
// Start the server
Console.Write("Server starting...");
server.StartServer();
Console.WriteLine("Done!");

Console.WriteLine("Press Enter to stop the server or '!' to restart the server...");

// Perform text input
for (; ; )
{
    string line = Console.ReadLine();
    if (line == "shutdown")
    {
        logger.Print(msg:"Game Server stopping ...");
        server.StopServer();
    }
    if (line == "restart")
    {
        logger.Print(msg:"Game Server restarting ...");
        server.RestartServer();

    }
}
