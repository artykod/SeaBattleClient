using UnityEngine;

public class Settings : BindingScreen
{
    public Bind<string> Title;
    public Bind<GameObject> Root;

    protected override Transform Content
    {
        get
        {
            return Root.Value.transform;
        }
    }

    public Settings() : base("Settings/Container")
    {
        Title.Value = "Settings";
    }

    [BindCommand]
    public void Close()
    {
        Destroy();
    }
}
