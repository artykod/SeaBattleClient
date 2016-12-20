public class Menu : EmptyScreenWithBackground
{
    private MenuContent _menuContent;

    public Menu() : base("Menu/Menu")
    {
        AddFirst(new BackgroundArt());
        Core.Instance.Auth.OnLogin += LoginHandler;
        IsLoading = true;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        Core.Instance.Auth.OnLogin -= LoginHandler;
    }

    private void LoginHandler(Data.CharacterData character)
    {
        if (_menuContent == null)
        {
            _menuContent = new MenuContent();
            _menuContent.IsSoundEnabled.Value = SoundController.IsSoundEnabled;
            AddLast(_menuContent);
        }
        IsLoading = false;
    }

    public void UpdateData()
    {
        _menuContent.UpdateData();
    }

    protected override void OnShowScreen()
    {
        if (_menuContent != null) _menuContent.IsSoundEnabled.Value = SoundController.IsSoundEnabled;
    }
}
