using System;
using TwitchChatConnect.Client;

[Serializable]
public static class TwitchChatSend 
{
	public static void SendChat(string message)
    {
        TwitchChatClient.instance.SendChatMessage(message: message);
    }
}
