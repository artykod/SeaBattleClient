using UnityEngine;

public abstract class LobbyCreateBattleItem : BindModel
{
    public Bind<string> Title { get; private set; }
    public Bind<int> BetCurrency { get; private set; }
    public Bind<int> BetValue { get; private set; }
    public Bind<bool> CanClickCreateButton { get; private set; }

    protected abstract bool IsMatchWithBot { get; }

    public LobbyCreateBattleItem() : base("UI/Lobby/LobbyCreateBattleItem")
    {
        CanClickCreateButton.Value = true;
        BetCurrency.Value = (int)Data.CurrencyType.Silver;
        BetValue.Value = 100;
        RefreshState();
    }

    [BindCommand]
    protected void SwitchBetCurrencyType()
    {
        switch ((Data.CurrencyType)BetCurrency.Value)
        {
            case Data.CurrencyType.Silver:
                BetCurrency.Value = (int)Data.CurrencyType.Gold;
                break;
            case Data.CurrencyType.Gold:
                BetCurrency.Value = (int)Data.CurrencyType.Silver;
                break;
            default:
                throw new System.Exception("Unknown currency: " + BetCurrency.Value);
        }
    }

    [BindCommand]
    protected void IncreaseBetValue()
    {
        BetValue.Value = Mathf.Min(10000, BetValue.Value + 10);
        RefreshState();
    }

    [BindCommand]
    protected void DecreaseBetValue()
    {
        BetValue.Value = Mathf.Max(0, BetValue.Value - 10);
        RefreshState();
    }

    [BindCommand]
    protected void CreateMatch()
    {
        Core.Instance.Lobby.CreateMatch((Data.CurrencyType)BetCurrency.Value, BetValue.Value, IsMatchWithBot);
        Core.Instance.Lobby.OnMatchReceived += OnNewMatchReceived;
    }

    private void OnNewMatchReceived(string token, Data.Match match)
    {
        Core.Instance.Lobby.OnMatchReceived -= OnNewMatchReceived;
        Core.Instance.MakeApiForMatch(token);

        Root.Destroy();
        new Layout();
    }

    private void RefreshState()
    {
        CanClickCreateButton.Value = BetValue.Value > 0;
    }
}
