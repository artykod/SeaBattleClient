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

    public Layout() : base("Layout")
    {
        Core.Instance.Match.OnMatchReceived += OnMatchReceived;
        Core.Instance.Match.ResetField();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Core.Instance.Match.OnMatchReceived -= OnMatchReceived;
    }

    private void OnMatchReceived(Data.Match match)
    {
        Ship1.Count.Value = match.My.FreeShips.Count1;
        Ship2.Count.Value = match.My.FreeShips.Count2;
        Ship3.Count.Value = match.My.FreeShips.Count3;
        Ship4.Count.Value = match.My.FreeShips.Count4;
    }

    [BindCommand]
    private void ResetLayout()
    {
        Core.Instance.Match.ResetField();
    }

    [BindCommand]
    private void AutoLayout()
    {
        Core.Instance.Match.AutolayoutField();
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