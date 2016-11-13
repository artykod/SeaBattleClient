public class Rules : EmptyScreenWithBackground
{
    public Bind<int> Page { get; private set; }

    public Rules() : base("Rules")
    {
        Page.Value = 1;
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
