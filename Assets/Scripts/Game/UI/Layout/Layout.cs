using System.Collections.Generic;
using UnityEngine;

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
    
    private FieldCellsContent _fieldCells;

    private Transform _root;
    protected override Transform Content { get { return !_root ? transform : _root; } }

    public Layout() : base("Layout")
    {
        Core.Instance.Match.OnMatchReceived += OnMatchReceived;
        Core.Instance.Match.OnMatchNotFound += OnMatchNotFound;

        _fieldCells = new FieldCellsContent();
        _root = transform.FindChild("CellsRoot");
        AddLast(_fieldCells);
        _root = null;

        Ship1.Count.Value = 4;
        Ship2.Count.Value = 3;
        Ship3.Count.Value = 2;
        Ship4.Count.Value = 1;

        AutoLayout();
    }

    private void OnMatchNotFound()
    {
        IsLoading = false;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Core.Instance.Match.OnMatchReceived -= OnMatchReceived;
        Core.Instance.Match.OnMatchNotFound -= OnMatchNotFound;
    }

    private void OnMatchReceived(Data.Match match)
    {
        IsLoading = false;

        var _ships_1 = new List<ShipModel>();
        var _ships_2 = new List<ShipModel>();
        var _ships_3 = new List<ShipModel>();
        var _ships_4 = new List<ShipModel>();

        ShipModel.FillAllShips(true, match.My.Field, _ships_1, _ships_2, _ships_3, _ships_4);

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

        _fieldCells.UpdateData(match.My.Field);
    }

    [BindCommand]
    private void ResetLayout()
    {
        IsLoading = true;
        Core.Instance.Match.ResetField();
    }

    [BindCommand]
    private void AutoLayout()
    {
        IsLoading = true;
        Core.Instance.Match.AutolayoutField();
    }

    [BindCommand]
    private void Battle()
    {
        if (Ship1.Count.Value > 0 || Ship2.Count.Value > 0 || Ship3.Count.Value > 0 || Ship4.Count.Value > 0)
        {
            new ErrorDialog("error.layout_not_done");
            return;
        }

        Root.Destroy();
        Core.Instance.Match.SendReady();
        new Game();
    }

    [BindCommand]
    private void Back()
    {
        new YesNoDialog("layout.sure_back")
            .AddButton("common.yes", () => Root.Destroy())
            .AddButton("common.no", null);
    }

    [BindCommand]
    private void Help()
    {
        new Rules();
    }
}