public class Dashboard : EmptyScreenWithBackground
{
    public Dashboard() : base("Dashboard/Dashboard")
    {
        IsLoading = true;

        Core.Instance.Lobby.OnBattleStatisticsReceived += OnStatisticsReceived;
        Core.Instance.Lobby.GetStatistics();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Core.Instance.Lobby.OnBattleStatisticsReceived -= OnStatisticsReceived;
    }

    private void OnStatisticsReceived(Data.BattleStatistics stats)
    {
        AddLast(new DashboardContent(Core.Instance.Character, stats));

        IsLoading = false;
    }
}
