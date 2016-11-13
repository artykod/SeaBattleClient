public class ErrorDialog : DialogBase
{
    public Bind<string> Title;

    public ErrorDialog(string titleLocKey) : base("ErrorDialog")
    {
        Title.Value = LanguageController.Localize(titleLocKey);
    }

    [BindCommand]
    private void Close()
    {
        Exit();
    }
}
