public class LobbyCreateBattleWithPlayerItem : LobbyCreateBattleItem
{
    public LobbyCreateBattleWithPlayerItem() : base()
    {
        Title.Value = LanguageController.Localize("lobby.create");
    }

    protected override bool IsMatchWithBot { get { return false; } }
}
