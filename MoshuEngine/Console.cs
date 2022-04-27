using System;
using UnityEngine;

[Serializable]
public static class Console 
{
	public static void Log(MonoBehaviour sender, string message) 
    {
        var obj = sender.gameObject.name;
        var componentArr = sender.ToString().Split('.');
        var component = componentArr[componentArr.Length-1];
        component = component.Replace(oldChar:')',newChar:' ').Trim();
        Debug.Log(
            message:
            $"<color=magenta>{obj}</color>" +
            $"<color=blue> {component} </color>" +
            $"<color=cyan> {message} </color>"
        );
    }
    public static void ChatLog(TwitchChatConnect.Data.TwitchChatMessage chatMessage) 
    {
        var user = chatMessage.User.DisplayName;
        var message = chatMessage.Message;
        Debug.Log(
            message:
            $"<color=magenta>{user}:</color>" +
            $"<color=cyan> {message}</color>"
        );
    }
    public static void CommandLog(TwitchChatConnect.Data.TwitchChatCommand chatCommand)
    {
        var user = chatCommand.User.DisplayName;
        var message = chatCommand.Message;
        var roles = "";

        foreach (TwitchChatConnect.Data.TwitchUserBadge role in chatCommand.User.Badges)
        {
            roles += $"[{role.Name}] ";
        }

        Debug.LogWarning(
            message:
            $"<color=magenta>{roles}{user}:</color>" +
            $"<color=cyan> {message}</color>"
        );
    }
}
