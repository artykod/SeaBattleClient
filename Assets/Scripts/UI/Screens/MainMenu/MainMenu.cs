using UnityEngine;

public class MainMenu : BindingScreen
{
    private string _menuTitle;
    public string MenuTitle
    {
        get
        {
            return _menuTitle;
        }
        set
        {
            Set(ref _menuTitle, value, () => MenuTitle);
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
        MenuTitle = "Sea battle game title";

        var items = new string[] 
        {
            "New game",
            "Settings",
            "About",
            "Exit"
        };

        for (int i = 0; i < items.Length; i++)
        {
            AddChild(new MainMenuItem(items[i]));
        }
    }
}