using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TwitchChatConnect.Client;

namespace TwitchMiniGames.GameControllers
{
    [System.Serializable]
    public class MainMenuEvents : MonoBehaviour
    {
        [Header(header:"Global")]
        [SerializeField] private GlobalVariables _globalVariables;

        [Header(header:"Twitch Data")]
        [SerializeField] private TwitchChatClient _twitchClient;
        [SerializeField] private TwitchChatEventManager _eventManager;

        [Header(header:"Config Menu")]
        [SerializeField] private TMP_InputField _channelField;
        [SerializeField] private TMP_Dropdown _botOptions;
        [SerializeField] private Button _connectButton;

        [Header(header:"Game Menu")]
        [SerializeField] private Button _playButton;

        [Header(header:"Own Bot Config")]
        [SerializeField] private GameObject _ownBotConfigurationPanel;
        [SerializeField] private TMP_InputField _botUsernameField;
        [SerializeField] private TMP_InputField _botOauthField;

        [Header(header:"Console Menu")]
        [SerializeField] private TextMeshProUGUI _onlineStatusLabel;
        [SerializeField] private TextMeshProUGUI _streamerLabel;
        [SerializeField] private TextMeshProUGUI _botLabel;
        [SerializeField] private TextMeshProUGUI _chatUsersLabel;

        [Header(header:"Game Selector Menu")]
        [SerializeField] private GameObject _gameSelectorPanel;

        private void Start()
        {
            var timer = 2f;
            InvokeRepeating
            (   
                methodName: "UpdateConsole",
                time: 0.1f,
                repeatRate: timer
            );
        }

        private void UpdateConsole()
        {
            var linkStatus = new string[2] { "Online", "Offline" };
            var linkColors = new Color[2] { Color.green, Color.red };
            var unsetValue = "N/A";
            _onlineStatusLabel.text = _globalVariables.isConnected
                ? linkStatus[0] : linkStatus[1];
            _onlineStatusLabel.color = _globalVariables.isConnected
                ? linkColors[0] : linkColors[1];
            _streamerLabel.text = string.IsNullOrEmpty
                (value: _globalVariables.channel)
                ? unsetValue : _globalVariables.channel;
            _chatUsersLabel.text = _globalVariables.chatUsers.Count.ToString();
            _botLabel.text = string.IsNullOrEmpty
                (value: _globalVariables.bot)
                ? unsetValue : _globalVariables.bot;
        }

        public void OnSaveUserClick()
        {
            _globalVariables.channel = _channelField.text;
            TryEnableConnectButton();
        }

        public void OnSaveBotClick()
        {
            var isDefaultBotSelected =_botOptions.options[0].text==_botOptions.captionText.text;
            if(isDefaultBotSelected) 
            {
                var botDefaultConfig = _globalVariables.botDataSO.TwitchConnectConfig;
                _globalVariables.bot = botDefaultConfig.Username;
                _globalVariables.oAuth = botDefaultConfig.UserToken;
                TryEnableConnectButton();
            }
            else
            {
                _ownBotConfigurationPanel.SetActive(value: true);
            }
        }

        public void OnSaveBotConfigClick()
        {
            _globalVariables.bot = _botUsernameField.text;
            _globalVariables.oAuth = _botOauthField.text;
            _ownBotConfigurationPanel.SetActive(value: false);
            TryEnableConnectButton();
        }

        public void OnCancelBotConfigClick()
        {
            _ownBotConfigurationPanel.SetActive(value: false);
        }

        public void TryEnableConnectButton()
        {
            if(_globalVariables.bot!=string.Empty &&
                _globalVariables.channel!=string.Empty &&
                _globalVariables.oAuth!=string.Empty) _connectButton.interactable = true;
        }

        public void OnSaveAndConnectClick()
        {
            _globalVariables.generatedBotDataSO.TwitchConnectConfig.ChannelName 
                = _globalVariables.channel;
            _globalVariables.generatedBotDataSO.TwitchConnectConfig.Username 
                = _globalVariables.bot;
            _globalVariables.generatedBotDataSO.TwitchConnectConfig.UserToken 
                = _globalVariables.oAuth;
            _twitchClient._initTwitchConnectData = _globalVariables.generatedBotDataSO;
            _playButton.interactable = true;
            _eventManager.InitializeChatConnection();
        }

        public void OnGameSelectClick()
        {
            _gameSelectorPanel.SetActive(value: true);
        }
    }
}

