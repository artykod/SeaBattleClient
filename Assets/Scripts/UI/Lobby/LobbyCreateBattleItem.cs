using UnityEngine;

public abstract class LobbyCreateBattleItem : BindModel
{
    public Bind<string> Title { get; private set; }
    public Bind<int> BetCurrency { get; private set; }
    public Bind<int> BetValue { get; private set; }
    public Bind<bool> CanClickCreateButton { get; private set; }

    protected abstract bool IsMatchWithBot { get; }

    private bool IsLoading
    {
        get
        {
            var root = Root as EmptyScreenWithBackground;
            if (root != null) return root.IsLoading;
            return false;
        }
        set
        {
            var root = Root as EmptyScreenWithBackground;
            if (root != null) root.IsLoading = value;
        }
    }

    public LobbyCreateBattleItem() : base("UI/Lobby/LobbyCreateBattleItem")
    {
        CanClickCreateButton.Value = true;
        BetCurrency.Value = (int)Data.CurrencyType.Silver;
        BetValue.Value = 100;
        RefreshState();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Unsubscribe();
    }

    private void Subscribe()
    {
        Core.Instance.Lobby.OnFailMatchCreate += OnFailCreateMatch;
        Core.Instance.Lobby.OnMatchReceived += OnNewMatchReceived;
    }

    private void Unsubscribe()
    {
        Core.Instance.Lobby.OnFailMatchCreate -= OnFailCreateMatch;
        Core.Instance.Lobby.OnMatchReceived -= OnNewMatchReceived;
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
        Unsubscribe();
        Subscribe();

        Core.Instance.Lobby.CreateMatch((Data.CurrencyType)BetCurrency.Value, BetValue.Value, IsMatchWithBot);
        IsLoading = true;
    }

    private void OnFailCreateMatch()
    {
        Unsubscribe();
        IsLoading = false;
        new ErrorDialog("error.cant_create_match");
    }

    private void OnNewMatchReceived(string token, Data.Match match)
    {
        Unsubscribe();
        IsLoading = false;
        Core.Instance.MakeApiForMatch(token);

        Root.Destroy();
        new Layout();
    }

    private void RefreshState()
    {
        CanClickCreateButton.Value = BetValue.Value > 0;
    }
}
