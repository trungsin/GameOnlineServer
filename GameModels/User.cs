using GameOnlineServer.Applications.Handlers;
using GameOnlineServer.GameModels.Base;
namespace GameOnlineServer.GameModels;

public class User(string userName, string password, string displayName) : BaseModel
{
    public string Username { get; set; } = userName;
    public string Password { get; set; } = GameHelper.HashPassword(password);
    public string DisplayName { get; set; } = displayName;
    public string Avatar { get; set; } = "";
    public int Level { get; set; } = 1;
    public long Amount { get; set; } = 0;
   
}
