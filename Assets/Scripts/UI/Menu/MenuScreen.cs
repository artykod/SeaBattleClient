using UnityEngine;

public class MenuScreen : BindScreen
{
    private PlayerData _player;

    public SocialContext Social { get; private set; }
    public PlayerContext Player { get; private set; }

    public Bind<string> TextPlay { get; private set; }
    public Bind<string> TextPrivateOffice { get; private set; }
    public Bind<string> TextSettings { get; private set; }
    public Bind<string> TextRules { get; private set; }
    public Bind<bool> IsSoundEnabled { get; private set; }

    public MenuScreen(PlayerData player) : base("Menu/Menu")
    {
        _player = player;

        Player.Avatar.Value = Resources.Load<Texture2D>(_player.Avatar);
        Player.Name.Value = _player.Name;
        Player.Gold.Value = _player.Gold;
        Player.Silver.Value = _player.Silver;

        TextPlay.Value = "Играть";
        TextPrivateOffice.Value = "Личный кабинет";
        TextSettings.Value = "Настройки";
        TextRules.Value = "Правила";

        IsSoundEnabled.Value = true;
    }

    [BindCommand]
    private void Play()
    {

    }

    [BindCommand]
    private void PrivateOffice()
    {
        new PrivateOfficeScreen(_player);
    }

    [BindCommand]
    private void Settings()
    {
        new SettingsScreen();
    }

    [BindCommand]
    private void Rules()
    {
        new RulesScreen();
    }
}
