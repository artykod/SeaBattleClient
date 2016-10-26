using UnityEngine;

public class MainMenu : BindingScreen
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

    public MainMenu() : base("MainMenu/Container")
    {
        Title.Value = "Sea battle game title";

        AddLast(new MainMenuItem("Play"));
        AddLast(new MainMenuItem("Records"));
        AddLast(new MainMenuItem("Settings", () => new Settings()));
        AddLast(new MainMenuItem("Exit", () => Destroy()));
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}