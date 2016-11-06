public class LobbyBattleListItem : BindModel
{
    private string _matchToken;
    private Data.LobbyMatch _matchData;

    public Bind<string> Nick;
    public Bind<int> CurrencyType;
    public Bind<int> CurrencyValue;
    public Bind<bool> CanJoin;

    public LobbyBattleListItem(string matchToken, Data.LobbyMatch matchData) : base("UI/Lobby/LobbyBattleListItem")
    {
        _matchToken = matchToken;
        UpdateData(matchData);
    }

    public void UpdateData(Data.LobbyMatch matchData)
    {
        _matchData = matchData;

        Nick.Value = matchData.Sides[0].Nick;
        CurrencyType.Value = matchData.Bet.Type;
        CurrencyValue.Value = matchData.Bet.Value;
        CanJoin.Value = matchData.Sides[1] != null;
    }

    [BindCommand]
    private void Join()
    {
        Root.Destroy();
        new Layout(_matchToken, _matchData);
    }
}
