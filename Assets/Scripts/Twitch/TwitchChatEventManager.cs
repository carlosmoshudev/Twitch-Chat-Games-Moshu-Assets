using UnityEngine;
using TwitchChatConnect.Client;
using TwitchChatConnect.Data;

public class TwitchChatEventManager : MonoBehaviour 
{
    public AhorcadoGM refAhorcado;
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
        //Console.ChatLog(chatMessage: chatMessage);
        //AddUser(user: chatMessage.User);
    }
    private void OnChatCommandReceived(TwitchChatCommand chatCommand)
    {
        var cmd = chatCommand.Command;
        var args = chatCommand.Message.Split(' ');
        Console.CommandLog(chatCommand: chatCommand);
        if (cmd == "!play" && _globalVariables.gameSelected == GameSelected.Ahorcado) 
        {
            AddUser(user: chatCommand.User);
        }
        if (cmd == "!letra" && _globalVariables.gameSelected == GameSelected.Ahorcado)
        {
            char letter = (args[1]).ToCharArray()[0];
            refAhorcado.TryLetter(letter: letter);
        }
        if (cmd == "!palabra" && _globalVariables.gameSelected == GameSelected.Ahorcado) 
        {
            refAhorcado.TryPalabra(triedWord: args[1]);
        }
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
    public void SendChat(string message)
    {
        TwitchChatClient.instance.SendChatMessage(message: message);
    }
}