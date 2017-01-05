public class MatchLoseDialog : MatchDialogBase
{
    private Data.FieldCellsData _lastOpponentField;

    public MatchLoseDialog(Data.FieldCellsData lastOpponentField) : base("MatchLoseDialog")
    {
        _lastOpponentField = lastOpponentField;
        SoundController.Sound(SoundController.SOUND_LOSE);
    }

    [BindCommand]
    private void ShowOpponentField()
    {
        Exit();
        new OpponentField(_lastOpponentField);
    }
}
