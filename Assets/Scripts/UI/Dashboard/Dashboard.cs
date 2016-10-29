using UnityEngine;

public class Dashboard : BindScreen
{
    private PlayerData _player;

    public PlayerContext Player;

    public Bind<string> TextBack;
    public Bind<string> AddGoldText;
    public Bind<string> AddSilverText;

    public Dashboard(PlayerData player) : base("Dashboard/Dashboard")
    {
        _player = player;

        TextBack.Value = "В меню";
        AddGoldText.Value = AddSilverText.Value = "Пополнить";

        Player.Avatar.Value = Resources.Load<Texture2D>(_player.Avatar);
        Player.Name.Value = _player.Name;
        Player.Gold.Value = _player.Gold;
        Player.Silver.Value = _player.Silver;
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
