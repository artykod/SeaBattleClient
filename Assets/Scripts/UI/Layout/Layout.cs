public class Layout : EmptyScreenWithBackground
{
    public class ShipContext : NeastedBindContext
    {
        public Bind<int> Count { get; set; }
        public Bind<int> Mode { get; set; }

        public void Init(int count, int mode)
        {
            Count.Value = count;
            Mode.Value = mode;
        }
    }

    public ShipContext Ship1 { get; private set; }
    public ShipContext Ship2 { get; private set; }
    public ShipContext Ship3 { get; private set; }
    public ShipContext Ship4 { get; private set; }

    public Layout(string matchToken, Data.LobbyMatch lobbyMatch) : base("Layout")
    {
        Core.Instance.StartMatch(matchToken);
        Core.Instance.Match.JoinToMatch((Data.CurrencyType)lobbyMatch.Bet.Type, lobbyMatch.Bet.Value);

        Ship1.Init(4, 0);
        Ship2.Init(3, 0);
        Ship3.Init(2, 0);
        Ship4.Init(1, 0);
    }

    [BindCommand]
    private void ResetLayout()
    {

    }

    [BindCommand]
    private void AutoLayout()
    {

    }

    [BindCommand]
    private void Battle()
    {
        new Game();
    }

    [BindCommand]
    private void Back()
    {
        Destroy();
    }

    [BindCommand]
    private void Help()
    {
        new Rules();
    }
}