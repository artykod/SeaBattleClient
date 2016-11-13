﻿public class DashboardContent : BindModel
{
    private Data.Character _character;

    public PlayerContext Player;

    public Bind<string> Total;
    public Bind<string> Win;
    public Bind<string> Lose;
    public Bind<string> Draw;

    public DashboardContent(Data.Character character, Data.BattleStatistics stats) : base("UI/Dashboard/DashboardContent")
    {
        _character = Core.Instance.Character;
        
        Player.Name.Value = _character.Nick;
        Player.Gold.Value = _character.Gold;
        Player.Silver.Value = _character.Silver;
        Core.Instance.LoadUserAvatar(_character.Id, Player.Avatar);

        var fmt = "{0} <color=#00188f>({1})</color>";

        Total.Value = stats.TotalBattles.ToString();
        Win.Value = string.Format(fmt, stats.WinCount, PercentValue(stats.WinCount, stats.TotalBattles));
        Lose.Value = string.Format(fmt, stats.LoseCount, PercentValue(stats.LoseCount, stats.TotalBattles));
        Draw.Value = string.Format(fmt, stats.DrawCount, PercentValue(stats.DrawCount, stats.TotalBattles));
    }

    private string PercentValue(int count, int total)
    {
        return (int)(((float)count / total) * 100f) + "%";
    }

    [BindCommand]
    private void Back()
    {
        Root.Destroy();
    }

    [BindCommand]
    private void AddGold()
    {
        new ErrorDialog("TODO buy gold");
    }

    [BindCommand]
    private void AddSilver()
    {
        new ErrorDialog("TODO buy silver");
    }
}
