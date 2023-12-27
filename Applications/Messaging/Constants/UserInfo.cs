﻿namespace GameOnlineServer.Messaging.Constants;

public struct UserInfo
{
    public string DisplayName { get; set; }
    public string Avatar { get; set; }
    public int Level { get; set; }
    public long Amount { get; set; }
}
