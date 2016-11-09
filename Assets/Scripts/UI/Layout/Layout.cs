using System.Collections.Generic;

public class Layout : EmptyScreenWithBackground
{
    public ShipCountViewContext Ship1;
    public ShipCountViewContext Ship2;
    public ShipCountViewContext Ship3;
    public ShipCountViewContext Ship4;

    public ShipViewContext FieldShip1_1;
    public ShipViewContext FieldShip1_2;
    public ShipViewContext FieldShip1_3;
    public ShipViewContext FieldShip1_4;
    public ShipViewContext FieldShip2_1;
    public ShipViewContext FieldShip2_2;
    public ShipViewContext FieldShip2_3;
    public ShipViewContext FieldShip3_1;
    public ShipViewContext FieldShip3_2;
    public ShipViewContext FieldShip4_1;

    public Layout() : base("Layout")
    {
        Core.Instance.Match.OnMatchReceived += OnMatchReceived;
        Core.Instance.Match.ResetField();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Core.Instance.Match.OnMatchReceived -= OnMatchReceived;
    }

    private void OnMatchReceived(Data.Match match)
    {
        var _ships_1 = new List<ShipModel>();
        var _ships_2 = new List<ShipModel>();
        var _ships_3 = new List<ShipModel>();
        var _ships_4 = new List<ShipModel>();

        ShipModel.FillAllShips(match.My.Field, _ships_1, _ships_2, _ships_3, _ships_4);

        FieldShip1_1.FetchFromModel(_ships_1, 0);
        FieldShip1_2.FetchFromModel(_ships_1, 1);
        FieldShip1_3.FetchFromModel(_ships_1, 2);
        FieldShip1_4.FetchFromModel(_ships_1, 3);
        FieldShip2_1.FetchFromModel(_ships_2, 0);
        FieldShip2_2.FetchFromModel(_ships_2, 1);
        FieldShip2_3.FetchFromModel(_ships_2, 2);
        FieldShip3_1.FetchFromModel(_ships_3, 0);
        FieldShip3_2.FetchFromModel(_ships_3, 1);
        FieldShip4_1.FetchFromModel(_ships_4, 0);

        Ship1.Count.Value = match.My.FreeShips.Count1;
        Ship2.Count.Value = match.My.FreeShips.Count2;
        Ship3.Count.Value = match.My.FreeShips.Count3;
        Ship4.Count.Value = match.My.FreeShips.Count4;
    }

    [BindCommand]
    private void ResetLayout()
    {
        Core.Instance.Match.ResetField();
    }

    [BindCommand]
    private void AutoLayout()
    {
        Core.Instance.Match.AutolayoutField();
    }

    [BindCommand]
    private void Battle()
    {
        new Game();
    }

    [BindCommand]
    private void Back()
    {
        Destroy();
    }

    [BindCommand]
    private void Help()
    {
        new Rules();
    }
}