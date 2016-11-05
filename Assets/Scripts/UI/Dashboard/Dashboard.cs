using UnityEngine;

public class Dashboard : EmptyScreenWithBackground
{
    private Data.Character _character;
    public PlayerContext Player;

    public Bind<string> Total;
    public Bind<string> Win;
    public Bind<string> Lose;
    public Bind<string> Draw;

    public Dashboard() : base("Dashboard")
    {
        _character = Core.Instance.Character;

        Player.Avatar.Value = Resources.Load<Texture2D>(_character.Avatar);
        Player.Name.Value = _character.Nick;
        Player.Gold.Value = _character.Gold;
        Player.Silver.Value = _character.Silver;

        Core.Instance.Lobby.OnBattleStatisticsReceived += OnStatisticsReceived;
        Core.Instance.Lobby.GetStatistics();
    }

    protected override void OnDestroy()
    {
        Core.Instance.Lobby.OnBattleStatisticsReceived -= OnStatisticsReceived;

        base.OnDestroy();
    }

    private string PercentValue(int count, int total)
    {
        return (int)(((float)count / total) * 100f) + "%";
    }

    private void OnStatisticsReceived(Data.BattleStatistics stats)
    {
        var fmt = "{0} <color=#00188f>({1})</color>";

        Total.Value = stats.TotalBattles.ToString();
        Win.Value   = string.Format(fmt, stats.WinCount,  PercentValue(stats.WinCount,  stats.TotalBattles));
        Lose.Value  = string.Format(fmt, stats.LoseCount, PercentValue(stats.LoseCount, stats.TotalBattles));
        Draw.Value  = string.Format(fmt, stats.DrawCount, PercentValue(stats.DrawCount, stats.TotalBattles));
    }

    [BindCommand]
    private void Back()
    {
        Destroy();
    }

    [BindCommand]
    private void AddGold()
    {

    }

    [BindCommand]
    private void AddSilver()
    {

    }
}
