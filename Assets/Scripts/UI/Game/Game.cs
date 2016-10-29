public class Game : EmptyScreenWithBackground
{
    public Game() : base("Game")
    {
    }

    [BindCommand]
    private void Back()
    {
        Destroy();
    }
}
