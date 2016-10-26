public class MainMenuItem : BindModel
{
    public Bind<string> Name;

    private System.Action _onClick;

    public MainMenuItem(string itemName, System.Action onClick = null) : base("Screens/MainMenu/Item")
    {
        Name.Value = itemName;
        _onClick = onClick;
    }

    [BindCommand]
    public void Click()
    {
        Core.Log("Click on item named '{0}'", Name.Value);

        if (_onClick != null)
        {
            _onClick();
        }
    }
}