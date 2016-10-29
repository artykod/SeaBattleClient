public class Game : BindScreen
{
    public Game() : base("Game/Game")
    {
    }

    [BindCommand]
    private void Back()
    {
        Destroy();
    }
}
