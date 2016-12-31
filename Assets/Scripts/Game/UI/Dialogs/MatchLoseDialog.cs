public class MatchLoseDialog : MatchDialogBase
{
    public MatchLoseDialog() : base("MatchLoseDialog")
    {
        SoundController.Sound(SoundController.SOUND_LOSE);
    }

    [BindCommand]
    private void ShowOpponentField()
    {
        Exit();
        new OpponentField();
    }
}
