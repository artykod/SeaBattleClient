public class MainMenuItem : BindingObject
{
    private string _name;
    public string Name
    {
        get
        {
            return _name;
        }
        set
        {
            Set(ref _name, value, () => Name);
        }
    }

    private System.Action _onClick;

    public MainMenuItem(string itemName, System.Action onClick = null) : base("Screens/MainMenu/Item")
    {
        Name = itemName;
        _onClick = onClick;
    }

    public void Click()
    {
        Core.Log("Click on item named '{0}'", Name);

        if (_onClick != null)
        {
            _onClick();
        }
    }
}