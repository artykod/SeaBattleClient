using UnityEngine;

public class Menu : EmptyScreenWithBackground
{
    private Data.Character _character;

    public SocialContext Social { get; private set; }
    public PlayerContext Player { get; private set; }
    
    public Bind<bool> IsSoundEnabled { get; private set; }

    public Menu() : base("Menu")
    {
        AddFirst(new BackgroundArt());

        UpdateData();

        IsSoundEnabled.OnValueChanged += (val) => SoundController.IsSoundEnabled = val.Value;
    }

    public void UpdateData()
    {
        _character = Core.Instance.Character;

        Player.Avatar.Value = Resources.Load<Texture2D>(_character.Avatar);
        Player.Name.Value = _character.Nick;
        Player.Gold.Value = _character.Gold;
        Player.Silver.Value = _character.Silver;
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
        new Dashboard();
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
