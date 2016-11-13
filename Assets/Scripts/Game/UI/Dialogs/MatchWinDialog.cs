public class MatchWinDialog : MatchDialogBase
{
    public Bind<int> CurrencyIcon;
    public Bind<int> Value;

    public MatchWinDialog(Data.CurrencyType currency, int bet) : base("MatchWinDialog")
    {
        CurrencyIcon.Value = (int)currency;
        Value.Value = bet;
        SoundController.Sound(SoundController.SOUND_WIN);
    }
}
