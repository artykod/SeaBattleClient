public class LobbyBattleList : BindModel
{
    public class SortedLobbyMatch
    {
        public string Token { get; private set; }
        public Data.LobbyMatch Match { get; private set; }

        public SortedLobbyMatch(string token, Data.LobbyMatch match)
        {
            Token = token;
            Match = match;
        }
    }

    public LobbyBattleList(SortedLobbyMatch[] lobby) : base("UI/Lobby/LobbyBattleList")
    {
        UpdateData(lobby);
    }

    public void UpdateData(SortedLobbyMatch[] lobby)
    {
        Clear();

        foreach (var i in lobby)
        {
            AddLast(new LobbyBattleListItem(i.Token, i.Match));
        }
    }
}
