public class LobbyCreateBattleWithCPUItem : LobbyCreateBattleItem
{
    public LobbyCreateBattleWithCPUItem() : base()
    {
        Title.Value = LanguageController.Localize("lobby.play_with_bot");
    }

    protected override bool IsMatchWithBot { get { return true; } }
}
