using UnityEngine;
using System.Collections.Generic;
using TwitchChatConnect.Data;
[System.Serializable]
public class GameUser 
{
    [SerializeField] private string _userName;
    [SerializeField] private string _badge;
    [SerializeField] private List<string> _bagdes;
    public GameUser(TwitchChatConnect.Data.TwitchUser user)
    {
        this._userName = user.DisplayName;
        this._badge = user.Badges.Count>0?user.Badges[0].Name:"None";
        this._bagdes = new List<string>();
        foreach(TwitchUserBadge badge in user.Badges) { this._bagdes.Add(badge.Name); }
    }
}
