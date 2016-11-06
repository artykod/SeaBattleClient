public class LobbyCreateBattleWithCPUItem : LobbyCreateBattleItem
{
    public LobbyCreateBattleWithCPUItem() : base()
    {
        Title.Value = LanguageController.Localize("lobby.create");
    }

    protected override void MatchCreateClicked()
    {
    }
}
