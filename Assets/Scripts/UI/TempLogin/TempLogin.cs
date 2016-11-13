public class TempLogin : EmptyScreenWithBackground
{
    public Bind<string> UserName;
    public Bind<string> UserPassword;

    public TempLogin() : base("TempLogin/TempLogin")
    {
        ApplyUser1();
    }

    [BindCommand]
    private void Login()
    {
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
