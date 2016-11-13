public abstract class MatchDialogBase : DialogBase
{
    private bool _retryClicked;

    public MatchDialogBase(string prefab) : base(prefab)
    {
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (_retryClicked) new Lobby();
    }

    [BindCommand]
    protected void Retry()
    {
        _retryClicked = true;
        Exit();
    }
}
