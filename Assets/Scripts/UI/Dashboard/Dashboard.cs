using UnityEngine;

public class Dashboard : EmptyScreenWithBackground
{
    private PlayerData _player;
    public PlayerContext Player;

    public Dashboard(PlayerData player) : base("Dashboard")
    {
        _player = player;

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
