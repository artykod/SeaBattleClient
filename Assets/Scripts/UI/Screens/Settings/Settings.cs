public class Settings : BindingScreen
{
    private string _title;
    public string Title
    {
        get
        {
            return _title;
        }
        set
        {
            Set(ref _title, value, () => Title);
        }
    }

    public Settings() : base("Settings/Container")
    {
        Title = "Settings";
    }

    public void Close()
    {
        Destroy();
    }
}
