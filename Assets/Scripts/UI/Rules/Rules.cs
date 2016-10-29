public class Rules : BindScreen
{
    public Bind<int> Page { get; private set; }
    public Bind<string> TextBack { get; private set; }
    public Bind<string> RulesText { get; private set; }

    public Rules() : base("Rules/Rules")
    {
        Page.Value = 1;
        TextBack.Value = "В меню";
        RulesText.Value = "Правила";
    }

    [BindCommand]
    private void Back()
    {
        Destroy();
    }

    [BindCommand]
    private void NextPage()
    {
        Page.Value = 2;
    }

    [BindCommand]
    private void PrevPage()
    {
        Page.Value = 1;
    }
}
