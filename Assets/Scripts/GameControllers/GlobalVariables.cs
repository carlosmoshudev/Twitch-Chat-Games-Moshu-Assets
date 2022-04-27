using TwitchChatConnect.Config;
using TwitchChatConnect.Data;
using UnityEngine;

[System.Serializable]
public class GlobalVariables : MonoBehaviour {
    [Header(header:"Twitch Connect")]
	[SerializeField] private string _channel;
    [SerializeField] private string _bot;
    [SerializeField] private string _oAuth;
    [SerializeField] private TwitchConnectData _defaultBotDataSO;
    [SerializeField] private TwitchConnectData _generatedBotDataSO;
    [SerializeField] private bool _isConnected = false;
    [Header(header:"Twitch")]
    public System.Collections.Generic.List<TwitchUser> chatUsers 
        = new System.Collections.Generic.List<TwitchUser>();
    public System.Collections.Generic.List<GameUser> gameUsers
        = new System.Collections.Generic.List<GameUser>();
    [SerializeField] private int _totalChatUsers;

    public string channel { get => _channel; set => _channel = value; }
    public string bot { get => _bot; set => _bot = value; }
    public string oAuth { get => _oAuth; set => _oAuth = value; }
    public TwitchConnectData botDataSO { get => _defaultBotDataSO; }
    public TwitchConnectData generatedBotDataSO { get => _generatedBotDataSO; set => _generatedBotDataSO = value; }
    public bool isConnected { get => _isConnected; set => _isConnected = value; }
    public int totalChatUsers { get => chatUsers.Count; }
    private void Update() {
        //borrar
        _totalChatUsers=chatUsers.Count;
    }
}
