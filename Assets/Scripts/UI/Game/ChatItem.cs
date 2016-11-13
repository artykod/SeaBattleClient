using System;
using UnityEngine;

public class ChatItem : BindModel
{
    public Bind<string> Name;
    public Bind<string> Time;
    public Bind<string> Message;
    public Bind<Color> Color;

    private DateTime _time;

    public ChatItem(string playerName, int unixTime, string message, bool isMy) : base("UI/Game/ChatItem")
    {
        _time = new DateTime(1970, 1, 1).AddMilliseconds(unixTime);
        Name.Value = playerName;
        Time.Value = string.Format("{0:T}", _time);
        Message.Value = message;

        Color c;
        ColorUtility.TryParseHtmlString(isMy ? "#0064E6FF" : "#00188FFF", out c);
        Color.Value = c;
    }
}
