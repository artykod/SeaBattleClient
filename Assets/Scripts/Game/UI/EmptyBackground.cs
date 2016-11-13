public class EmptyBackground : BindModel
{
    public Bind<bool> IsLoading { get; private set; }

    public EmptyBackground() : base("UI/EmptyBackground")
    {
    }
}
