public class EmptyScreen : EmptyScreenWithBackground
{
    public EmptyScreen() : base("EmptyScreen")
    {
        AddLast(new EmptyBackground());
        AddLast(new BackgroundArt());
    }
}