using System.Collections.Generic;
using UnityEngine;
using TwitchChatConnect.Data;
using TMPro;

[DisallowMultipleComponent]
public class StopGM : MonoBehaviour 
{
    [Header(header:"Prefabs")]
    [SerializeField] private GameObject _userPrefab;
    [SerializeField] private GameObject _elementTextPrefab;
    [SerializeField] private GameObject _elementAnswerPrefab;

    [Header(header:"Parents")]
    [SerializeField] private GameObject _usersParent;
    [SerializeField] private GameObject _elementsParent;
    [SerializeField] private GameObject _elementsAnswerParent;

    [Header(header:"Twitch Connect")]
    [SerializeField] private TwitchChatEventManager _twitchChatEventManager;

    [Header(header:"Canvas References")]
    [SerializeField] private TextMeshProUGUI _slectedLetterCanvas;
    [SerializeField] private TextMeshProUGUI _userNameCanvas;
    [SerializeField] private GameObject _answerCanvasPanel;
    [SerializeField] private TextMeshProUGUI _playersCounterCanvas;

    private bool _gameRunning = false;
    private List<TwitchUser> _players = new List<TwitchUser>();
    private Dictionary<string,ToldElements> _respuestas = new Dictionary<string,ToldElements>();
    private TwitchUser _userToken;
    private int _currentIndex = 0;
    private int _turn = 0;
    private const int MAX_PLAYERS = 15;
    private const int ANSWERS = 11;
    private const int MIN_PLAYERS = 1;

    [Header(header:"Debug")]
    public List<string> players = new List<string>();
    public string userToken { get => _userToken.Username; }

    public void SetupStop()
    {
        _twitchChatEventManager.SendChat("¡Se ha iniciado el juego Basta! -> usa !jugar para participar.");
        _players.Clear();
        players.Clear();
        _respuestas.Clear();
        SetupElements();
        SetupAnswerElements();
    }
	public void JoinUser(TwitchUser user)
    {
        if (_players.Contains(user)) return;
        _twitchChatEventManager.SendChat($"[Basta] {user.DisplayName} se ha unido.");
        AddUserOnLists(user);
        SetInterfaceForNewUser(user);
    }
    private void SetInterfaceForNewUser(TwitchUser user)
    {
        GameObject userGO = Instantiate(_userPrefab, _usersParent.transform);
        userGO.GetComponent<TextMeshProUGUI>().text = user.DisplayName;
        _playersCounterCanvas.text = $"Jugadores ({_players.Count}/15)";
    }
    private void AddUserOnLists(TwitchUser user)
    {
        _players.Add(user);
        players.Add(user.Username);
        _respuestas.Add(user.Username, new ToldElements());
    }

    public void StartGame()
    {
        if(_players.Count < MIN_PLAYERS) return;
        _twitchChatEventManager.SendChat($"[Basta] Comienza el juego. {_players.Count} jugadores.");
        _twitchChatEventManager.SendChat($"[Basta] {_players[_turn].DisplayName} es el primero. Elije una letra con !letra <letra>, o usa !random para elegir una letra aleatoria.");
        _userToken = _players[_turn];
        _gameRunning = true;    
    }
    public void AskLetter(char letter, TwitchUser user) 
    {
        if(user.DisplayName!=_userToken.DisplayName || !_gameRunning) return;
        _twitchChatEventManager.SendChat(
                $"[Basta] {_players[_turn].DisplayName} ha elegido la letra {letter}.");
        _slectedLetterCanvas.text = letter.ToString().ToUpper();
    }
    public void AskRandomLetter(TwitchUser user)
    {
        if(user.DisplayName!=_userToken.DisplayName || !_gameRunning) return;
        char letter = (char)UnityEngine.Random.Range(65, 91);
        _twitchChatEventManager.SendChat(
            $"[Basta] {_players[_turn].DisplayName} ha elegido la letra aleatoria {letter}.");
        _slectedLetterCanvas.text = letter.ToString().ToUpper();
    }
    public void SetWord(string element, string word, TwitchUser user)
    {
        if(!_gameRunning || !_players.Contains(user)) return;
        switch(element)
        {
            case "nombre":
                _respuestas[user.Username].nombre = word;
                break;
            case "apellido":
                _respuestas[user.Username].apellido = word;
                break;
            case "ciudad":
                _respuestas[user.Username].ciudad = word;
                break;
            case "pais":
                _respuestas[user.Username].pais = word;
                break;
            case "animal":
                _respuestas[user.Username].animal = word;
                break;
            case "comida":
                _respuestas[user.Username].comida = word;
                break;
            case "color":
                _respuestas[user.Username].color = word;
                break;
            case "libro":
                _respuestas[user.Username].libro = word;
                break;
            case "pelicula":
                _respuestas[user.Username].pelicula = word;
                break;
            case "serie":
                _respuestas[user.Username].serie = word;
                break;
            case "objeto":
                _respuestas[user.Username].objeto = word;
                break;
            default:
                break;
        }
    }

    public void EndRun()
    {
        _twitchChatEventManager.SendChat($"[Basta] El juego ha terminado.");
        _gameRunning = false;
        ShowUser(user: _players[_currentIndex]);
    }
    public void NextRound()
    {
        if (_turn < _players.Count - 1) 
        {
            _turn++;
            _currentIndex = _turn;
            _twitchChatEventManager.SendChat($"[Basta] {_players[_turn].DisplayName} es el siguiente.");
            _twitchChatEventManager.SendChat($"[Basta] {_players[_turn].DisplayName} elige una letra con !letra <letra>, o usa !random para elegir una letra aleatoria.");
            _userToken = _players[_turn];
            var emptyUser = new TwitchUser("Usuario");
            _respuestas.Add(emptyUser.Username,new ToldElements());
            ShowUser(user: emptyUser);
            _respuestas.Clear();
        }
        else
        {
            _twitchChatEventManager.SendChat($"[Basta] El juego ha terminado.");
            _gameRunning = false;
        }
    }
    private void ShowUser(TwitchUser user)
    {
        _userNameCanvas.text = user.DisplayName;
        _answerCanvasPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _respuestas[user.Username].nombre;
        _answerCanvasPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _respuestas[user.Username].apellido;
        _answerCanvasPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = _respuestas[user.Username].ciudad;
        _answerCanvasPanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = _respuestas[user.Username].pais;
        _answerCanvasPanel.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = _respuestas[user.Username].animal;
        _answerCanvasPanel.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = _respuestas[user.Username].comida;
        _answerCanvasPanel.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = _respuestas[user.Username].color;
        _answerCanvasPanel.transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = _respuestas[user.Username].libro;
        _answerCanvasPanel.transform.GetChild(8).GetComponent<TextMeshProUGUI>().text = _respuestas[user.Username].pelicula;
        _answerCanvasPanel.transform.GetChild(9).GetComponent<TextMeshProUGUI>().text = _respuestas[user.Username].serie;
        _answerCanvasPanel.transform.GetChild(10).GetComponent<TextMeshProUGUI>().text = _respuestas[user.Username].objeto;
    }
    public void NextUser()
    {   
        if(_currentIndex < _players.Count-1)
        {
            _currentIndex++;
            ShowUser(user: _players[_currentIndex]);
        }
        else
        {
            _twitchChatEventManager.SendChat($"[Basta] El juego ha terminado.");
            _gameRunning = false;
        }
    }
    private void SetupElements()
    {
        string[] elements = new string[] 
        {
            "Nombre: ",
            "Apellido: ",
            "Ciudad: ",
            "Pais: ",
            "Animal: ",
            "Comida: ",
            "Color: ",
            "Libro: ",
            "Pelicula: ",
            "Serie: ",
            "Objeto: "
        };
        foreach(string element in elements)
        {
            GameObject elementGO = Instantiate(_elementTextPrefab, _elementsParent.transform);
            elementGO.GetComponent<TextMeshProUGUI>().text = element;
        }
    }
    private void SetupAnswerElements()
    {
        for (var i = 0; i < ANSWERS; i++)
        {
            GameObject elementGO = Instantiate(_elementAnswerPrefab, _elementsAnswerParent.transform);
            elementGO.GetComponent<TextMeshProUGUI>().text = "";
        }
    }
}
