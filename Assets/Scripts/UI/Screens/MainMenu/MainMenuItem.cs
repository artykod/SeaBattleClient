public class MainMenuItem : BindingScreen
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

    public MainMenuItem(string itemName) : base("MainMenu/Item")
    {
        Name = itemName;
    }
}