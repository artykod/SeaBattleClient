public class MenuContent : BindModel
{
    private Data.Character _character;

    public SocialContext Social { get; private set; }
    public PlayerContext Player { get; private set; }

    public Bind<bool> IsSoundEnabled { get; private set; }
    public Bind<bool> IsTempLoginEnabled { get; private set; }

    public MenuContent() : base("UI/Menu/MenuContent")
    {
        AddFirst(new BackgroundArt());

        UpdateData();

        IsSoundEnabled.OnValueChanged += (val) => SoundController.IsSoundEnabled = val.Value;
        IsTempLoginEnabled.Value = Networking.Connection.TempLoginEnabled;
    }

    public void UpdateData()
    {
        _character = Core.Instance.Character;

        Player.Name.Value = _character.Nick;
        Player.Gold.Value = _character.Gold;
        Player.Silver.Value = _character.Silver;
        Core.Instance.LoadUserAvatar(_character.Id, Player.Avatar);
    }

    [BindCommand]
    private void Play()
    {
        new Lobby();
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

    [BindCommand]
    private void Logout()
    {
        new YesNoDialog("Logout?")
            .AddButton("Yes", () =>
            {
                Networking.Connection.Instance.Logout();
                UnityEngine.SceneManagement.SceneManager.LoadScene("TempLogin", UnityEngine.SceneManagement.LoadSceneMode.Single);
            })
            .AddButton("No", null);
    }
}
