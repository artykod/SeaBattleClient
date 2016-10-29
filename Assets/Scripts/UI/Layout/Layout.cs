public class Layout : BindScreen
{
    public Layout() : base("Layout/Layout")
    {
    }

    [BindCommand]
    private void ResetLayout()
    {

    }

    [BindCommand]
    private void AutoLayout()
    {

    }

    [BindCommand]
    private void Battle()
    {
        new Game();
    }

    [BindCommand]
    private void Back()
    {
        Destroy();
    }

    [BindCommand]
    private void Help()
    {
        new Rules();
    }
}