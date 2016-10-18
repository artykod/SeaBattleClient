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

    public MainMenuItem(string itemName) : base("Screens/MainMenu/Item")
    {
        Name = itemName;
    }

    public void Click()
    {
        Core.Log("Click on item named '{0}'", Name);
    }
}