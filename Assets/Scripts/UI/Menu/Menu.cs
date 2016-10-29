using UnityEngine;

public class Menu : BindScreen
{
    private PlayerData _player;

    public SocialContext Social { get; private set; }
    public PlayerContext Player { get; private set; }
    
    public Bind<bool> IsSoundEnabled { get; private set; }

    public Menu(PlayerData player) : base("Menu/Menu")
    {
        _player = player;

        Player.Avatar.Value = Resources.Load<Texture2D>(_player.Avatar);
        Player.Name.Value = _player.Name;
        Player.Gold.Value = _player.Gold;
        Player.Silver.Value = _player.Silver;

        IsSoundEnabled.Value = SoundController.Instance.IsSoundEnabled;
        IsSoundEnabled.OnValueChanged += OnSoundEnabledChanged;
    }

    protected override void OnShowScreen()
    {
        IsSoundEnabled.Value = SoundController.Instance.IsSoundEnabled;
    }

    private void OnSoundEnabledChanged(Bind<bool> bind)
    {
        SoundController.Instance.IsSoundEnabled = bind.Value;
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
