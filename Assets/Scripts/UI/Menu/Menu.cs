using UnityEngine;

public class Menu : EmptyScreenWithBackground
{
    private PlayerData _player;

    public SocialContext Social { get; private set; }
    public PlayerContext Player { get; private set; }
    
    public Bind<bool> IsSoundEnabled { get; private set; }

    public Menu(PlayerData player) : base("Menu")
    {
        AddFirst(new BackgroundArt());

        _player = player;

        Player.Avatar.Value = Resources.Load<Texture2D>(_player.Avatar);
        Player.Name.Value = _player.Name;
        Player.Gold.Value = _player.Gold;
        Player.Silver.Value = _player.Silver;

        IsSoundEnabled.OnValueChanged += (val) => SoundController.IsSoundEnabled = val.Value;
    }

    protected override void OnShowScreen()
    {
        IsSoundEnabled.Value = SoundController.IsSoundEnabled;
    }

    [BindCommand]
    private void Play()
    {
        new Layout();
    }

    [BindCommand]
    private void Dashboard()
    {
        new Dashboard(_player);
    }

    [BindCommand]
    private void Settings()
    {
        new Settings();
    }

    [BindCommand]
    private void Rules()
    {
        new Rules();
    }
}
