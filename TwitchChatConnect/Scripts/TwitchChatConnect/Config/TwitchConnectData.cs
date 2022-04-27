using UnityEngine;

namespace TwitchChatConnect.Config
{
    [CreateAssetMenu(fileName = "TwitchConnectData", menuName = "TwitchChatConnect/TwitchConnectData", order = 1)]
    public class TwitchConnectData : ScriptableObject
    {
        public TwitchConnectConfig TwitchConnectConfig;
    }
}