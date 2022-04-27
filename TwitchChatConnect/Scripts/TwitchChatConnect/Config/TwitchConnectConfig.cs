using System;
using UnityEngine;

namespace TwitchChatConnect.Config
{
    [Serializable]
    public class TwitchConnectConfig
    {
        [SerializeField] private string username;
        [SerializeField] private string userToken;
        [SerializeField] private string channelName;

        public string Username {
            get { return username.ToLower(); } set { username=value; } }
        public string UserToken {
            get { return userToken; } set { userToken=value; } }
        public string ChannelName {
            get { return channelName; } set { channelName=value; } }

        public TwitchConnectConfig(string username, string userToken, string channelName)
        {
            this.username = username;
            this.userToken = userToken;
            this.channelName = channelName;
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(UserToken) && !string.IsNullOrEmpty(ChannelName);
        }
    }
}