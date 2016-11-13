public class TempLogin : EmptyScreenWithBackground
{
    public Bind<string> UserName;
    public Bind<string> UserPassword;

    public TempLogin() : base("TempLogin/TempLogin")
    {
        LanguageController.Instance.Initialize();
        ApplyUser1();
    }

    private bool Check()
    {
        if (string.IsNullOrEmpty(UserName.Value)) return false;
        if (string.IsNullOrEmpty(UserPassword.Value)) return false;
        return true;
    }

    [BindCommand]
    private void Login()
    {
        if (!Check())
        {
            new ErrorDialog("Wrong email or password");
            return;
        }

        Networking.Connection.TempAuthUserName = UserName.Value;
        Networking.Connection.TempAuthUserPassword = UserPassword.Value;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Preloader", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    [BindCommand]
    private void ApplyUser1()
    {
        UserName.Value = "u1@r.ru";
        UserPassword.Value = "111";
    }

    [BindCommand]
    private void ApplyUser2()
    {
        UserName.Value = "u2@r.ru";
        UserPassword.Value = "222";
    }
}
