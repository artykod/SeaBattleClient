public class LobbyCreateBattleWithPlayerItem : LobbyCreateBattleItem
{
    public LobbyCreateBattleWithPlayerItem() : base()
    {
        Title.Value = LanguageController.Localize("lobby.play_with_bot");
    }

    protected override void MatchCreateClicked()
    {
    }
}
