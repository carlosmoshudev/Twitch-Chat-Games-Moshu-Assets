using UnityEngine;
using TwitchChatConnect.Client;
using TwitchChatConnect.Data;
using TwitchMiniGames.GameControllers;
using TwitchMiniGames.Games;
[DisallowMultipleComponent]
public class TwitchChatEventManager : MonoBehaviour 
{
    [Header(header:"Referencias Minijuegos")]
    [SerializeField] private AhorcadoGM refAhorcado;
    [Space(10)]
    [SerializeField] private StopGM refStop;
    [Space(10)]
    [SerializeField] private LaberinthGM refLaberinto;
    [Header(header:"Global")]
    [SerializeField] private GlobalVariables _globalVariables;

    public void InitializeChatConnection() {
        TwitchChatClient.instance.Init(
            onSuccess: () => 
            {
                _globalVariables.isConnected = true;
                TwitchChatClient.instance.onChatMessageReceived += OnChatMessage;
                TwitchChatClient.instance.onChatCommandReceived += OnChatCommand;
            },
            onError: errorMessage => { Debug.LogError($"Error iniciando: {errorMessage}"); }
        );
    }
    public void SendChat(string message) => TwitchChatClient.instance.SendChatMessage(message);
    private void OnChatMessage(TwitchChatMessage chatMessage) => Console.ChatLog(chatMessage);

    private void OnChatCommand(TwitchChatCommand chatCommand)
    {
        Console.CommandLog(chatCommand);

        var cmd             = chatCommand.Command.ToLower();
        var args            = chatCommand.Message.Split(' ');
        var streamer        = this.streamer(chatCommand.User);

        bool isAhorcado     = (_globalVariables.gameSelected == GameSelected.Ahorcado);
        bool isStop         = (_globalVariables.gameSelected == GameSelected.Stop);
        bool isLaberinto    = (_globalVariables.gameSelected == GameSelected.Laberinto);

        if (isAhorcado)     AhorcadoCommands    (chatCommand, cmd, args, streamer);
        if (isStop)         BastaCommands       (chatCommand, cmd, args, streamer); 
        if (isLaberinto)    LaberintoCommands   (chatCommand, cmd, args, streamer);

    }
    private void LaberintoCommands(TwitchChatCommand chatCommand, string cmd, string[] args, bool streamer)
    {
        var user = chatCommand.User;

        if (cmd == "!gen" && streamer) refLaberinto.Generate();
        if (cmd == "!start" && streamer) refLaberinto.StartGame();

        if (cmd == "!a") refLaberinto.Move(new Vector2Int(x: -10, y: 0));
        if (cmd == "!s") refLaberinto.Move(new Vector2Int(x: 0, y: -10));
        if (cmd == "!d") refLaberinto.Move(new Vector2Int(x: 10, y: 0));
        if (cmd == "!w") refLaberinto.Move(new Vector2Int(x: 0, y: 10));

    }
    
    private void BastaCommands(TwitchChatCommand chatCommand, string cmd, string[] args, bool streamer)
    {
        var user    = chatCommand.User;
        
        if (cmd == "!jugar") refStop.JoinUser(user: user);

        if (cmd == "!iniciar" && streamer) refStop.StartGame();
        if (cmd == "!basta") refStop.EndRun();

        if (cmd == "!aleatorio")   refStop.AskRandomLetter(user: user);
        if (cmd == "!letra")    
        {
            var letter  = (args[1])?.ToCharArray()[0] ?? '-';
            refStop.AskLetter(letter: letter , user: user);
        };

        if (cmd == "!nombre")   refStop.SetWord(element: "nombre",      word: args[1], user: user);
        if (cmd == "!apellido") refStop.SetWord(element: "apellido",    word: args[1], user: user);
        if (cmd == "!ciudad")   refStop.SetWord(element: "ciudad",      word: args[1], user: user);
        if (cmd == "!pais")     refStop.SetWord(element: "pais",        word: args[1], user: user);
        if (cmd == "!animal")   refStop.SetWord(element: "animal",      word: args[1], user: user);
        if (cmd == "!comida")   refStop.SetWord(element: "comida",      word: args[1], user: user);
        if (cmd == "!color")    refStop.SetWord(element: "color",       word: args[1], user: user);
        if (cmd == "!libro")    refStop.SetWord(element: "libro",       word: args[1], user: user);
        if (cmd == "!pelicula") refStop.SetWord(element: "pelicula",    word: args[1], user: user);
        if (cmd == "!serie")    refStop.SetWord(element: "serie",       word: args[1], user: user);
        if (cmd == "!objeto")   refStop.SetWord(element: "objeto",      word: args[1], user: user);
        return;
    }
    private void AhorcadoCommands(TwitchChatCommand chatCommand, string cmd, string[] args, bool streamer)
    {
        var user    = chatCommand.User;

        cmd = cmd.ToLower();
        
        if (cmd == "!jugar")     refAhorcado.JoinUser(user: user);
        if (cmd == "!letra")    
        {
            var letter  = (args[1])?.ToLower().ToCharArray()[0] ?? ' ';
            refAhorcado.TryLetter(letter: letter, user: user);
        };
        if (cmd == "!palabra")  refAhorcado.TryPalabra(triedWord: args[1], user: user);
        return;
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
    private bool streamer(TwitchUser user)
    {
        foreach(TwitchUserBadge badge in user.Badges)
        {
            if(badge.Name=="broadcaster") return true;
        }
        return false;
    }
}