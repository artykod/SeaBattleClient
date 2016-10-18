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
            var item = AddChild(new MainMenuItem(items[i]));
            item.Instance.transform.localPosition = new Vector3(0f, (items.Length - i) * 100f - 300f);
        }
    }
}