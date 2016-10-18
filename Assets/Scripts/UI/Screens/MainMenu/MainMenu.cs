using UnityEngine;

public class MainMenu : BindingScreen
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

    protected override Transform Content
    {
        get
        {
            return transform.FindChild("ItemsRoot");
        }
    }

    public MainMenu() : base("MainMenu/Container")
    {
        Title = "Sea battle game title";

        AddChild(new MainMenuItem("Play"));
        AddChild(new MainMenuItem("Records"));
        AddChild(new MainMenuItem("Settings", () => new Settings()));
        AddChild(new MainMenuItem("Exit", () => Destroy()));
    }

    public override void Destroy()
    {
        base.Destroy();

        Application.Quit();
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}