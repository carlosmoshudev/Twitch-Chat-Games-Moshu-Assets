using TwitchChatConnect.Config;
using TwitchChatConnect.Data;
using UnityEngine;
using System.Collections.Generic;
namespace TwitchMiniGames.GameControllers
{
    [System.Serializable]
    public class GlobalVariables : MonoBehaviour
    {
        [Header(header: "Twitch Connect")]
        [SerializeField] private string _channel;
        [SerializeField] private string _bot;
        [SerializeField] private string _oAuth;
        [SerializeField] private TwitchConnectData _defaultBotDataSO;
        [SerializeField] private TwitchConnectData _generatedBotDataSO;
        [SerializeField] private bool _isConnected = false;
        public GameSelected gameSelected = GameSelected.None;
        [Header(header: "Twitch")]
        public List<TwitchUser> chatUsers = new List<TwitchUser>();
        public List<GameUser> gameUsers = new List<GameUser>();
        [SerializeField] private int _totalChatUsers;

        public string channel { get => _channel; set => _channel = value; }
        public string bot { get => _bot; set => _bot = value; }
        public string oAuth { get => _oAuth; set => _oAuth = value; }
        public TwitchConnectData botDataSO { get => _defaultBotDataSO; }
        public TwitchConnectData generatedBotDataSO { get => _generatedBotDataSO; set => _generatedBotDataSO = value; }
        public bool isConnected { get => _isConnected; set => _isConnected = value; }
        public int totalChatUsers { get => chatUsers.Count; }
    }
    public enum GameSelected
    {
        None,
        Ahorcado,
        Stop,
        Trivia,
        Board,
        Aventura,
        Poll,
        Prediction,
        Laberinto
    }
}