using UnityEngine;
using TwitchChatConnect.Client;
using TwitchChatConnect.Data;

public class TwitchChatEventManager : MonoBehaviour 
{
    [SerializeField] private GlobalVariables _globalVariables;
    public void InitializeChatConnection()
    {
        TwitchChatClient.instance.Init(
            onSuccess: () => 
            {
                _globalVariables.isConnected = true;
                TwitchChatClient.instance.onChatMessageReceived 
                    += OnChatMessageReceived;
                TwitchChatClient.instance.onChatCommandReceived
                    += OnChatCommandReceived;
            },
            onError: errorMessage => 
            {
                Debug.LogError
                    (message: $"Error iniciando: {errorMessage}");
            }
        );
    }
    private void OnChatMessageReceived(TwitchChatMessage chatMessage)
    {
        Console.ChatLog(chatMessage: chatMessage);
        AddUser(user: chatMessage.User);
    }
    private void OnChatCommandReceived(TwitchChatCommand chatCommand)
    {
        Console.CommandLog(chatCommand: chatCommand);
        AddUser(user: chatCommand.User);
    }
    private void AddUser(TwitchUser user)
    {
        if(!CheckUserAdded(user: user))
        {
            _globalVariables.chatUsers.Add(item: user);
            _globalVariables.gameUsers.Add(new GameUser(user: user));
        }
    }
    private bool CheckUserAdded(TwitchUser user)
    {
        foreach(TwitchUser addedUser in _globalVariables.chatUsers)
        {
            if(addedUser.DisplayName==user.DisplayName) return true;
        }
        return false;
    }
}