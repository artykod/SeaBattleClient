using UnityEngine;
using System.Collections.Generic;

public class OpponentField : EmptyScreenWithBackground
{
    #region Last opponent field bindings
    public ShipViewContext MyFieldShip1_1;
    public ShipViewContext MyFieldShip1_2;
    public ShipViewContext MyFieldShip1_3;
    public ShipViewContext MyFieldShip1_4;
    public ShipViewContext MyFieldShip2_1;
    public ShipViewContext MyFieldShip2_2;
    public ShipViewContext MyFieldShip2_3;
    public ShipViewContext MyFieldShip3_1;
    public ShipViewContext MyFieldShip3_2;
    public ShipViewContext MyFieldShip4_1;
    public Bind<GameObject> MyCellsRoot;
    #endregion

    #region Opponent field bindings
    public ShipViewContext OpponentFieldShip1_1;
    public ShipViewContext OpponentFieldShip1_2;
    public ShipViewContext OpponentFieldShip1_3;
    public ShipViewContext OpponentFieldShip1_4;
    public ShipViewContext OpponentFieldShip2_1;
    public ShipViewContext OpponentFieldShip2_2;
    public ShipViewContext OpponentFieldShip2_3;
    public ShipViewContext OpponentFieldShip3_1;
    public ShipViewContext OpponentFieldShip3_2;
    public ShipViewContext OpponentFieldShip4_1;
    #endregion

    private Data.FieldCellsData _lastOpponentField;
    private FieldCellsContent _myFieldCells;
    private Transform _root;
    protected override Transform Content { get { return !_root ? transform : _root; } }

    public OpponentField(Data.FieldCellsData lastOpponentField) : base("OpponentField")
    {
        _lastOpponentField = lastOpponentField;

        _myFieldCells = new FieldCellsContent();
        _root = MyCellsRoot.Value.transform;
        AddLast(_myFieldCells);
        _root = null;

        IsLoading = true;
        Core.Instance.Match.OnOpponentFieldReceived += OnOpponentFieldReceived;
        Core.Instance.Match.GetOpponentFieldAfterBattle();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Core.Instance.Match.OnOpponentFieldReceived -= OnOpponentFieldReceived;
    }

    private void OnOpponentFieldReceived(Data.FieldCellsData field)
    {
        IsLoading = false;

        // last opponent field
        {
            var _ships_1 = new List<ShipModel>();
            var _ships_2 = new List<ShipModel>();
            var _ships_3 = new List<ShipModel>();
            var _ships_4 = new List<ShipModel>();

            ShipModel.FillAllShips(true, _lastOpponentField, _ships_1, _ships_2, _ships_3, _ships_4);

            MyFieldShip1_1.FetchFromModel(_ships_1, 0);
            MyFieldShip1_2.FetchFromModel(_ships_1, 1);
            MyFieldShip1_3.FetchFromModel(_ships_1, 2);
            MyFieldShip1_4.FetchFromModel(_ships_1, 3);
            MyFieldShip2_1.FetchFromModel(_ships_2, 0);
            MyFieldShip2_2.FetchFromModel(_ships_2, 1);
            MyFieldShip2_3.FetchFromModel(_ships_2, 2);
            MyFieldShip3_1.FetchFromModel(_ships_3, 0);
            MyFieldShip3_2.FetchFromModel(_ships_3, 1);
            MyFieldShip4_1.FetchFromModel(_ships_4, 0);

            _myFieldCells.UpdateData(_lastOpponentField);
        }

        // win opponent field from server
        {
            var _ships_1 = new List<ShipModel>();
            var _ships_2 = new List<ShipModel>();
            var _ships_3 = new List<ShipModel>();
            var _ships_4 = new List<ShipModel>();

            ShipModel.FillAllShips(true, field, _ships_1, _ships_2, _ships_3, _ships_4);

            OpponentFieldShip1_1.FetchFromModel(_ships_1, 0);
            OpponentFieldShip1_2.FetchFromModel(_ships_1, 1);
            OpponentFieldShip1_3.FetchFromModel(_ships_1, 2);
            OpponentFieldShip1_4.FetchFromModel(_ships_1, 3);
            OpponentFieldShip2_1.FetchFromModel(_ships_2, 0);
            OpponentFieldShip2_2.FetchFromModel(_ships_2, 1);
            OpponentFieldShip2_3.FetchFromModel(_ships_2, 2);
            OpponentFieldShip3_1.FetchFromModel(_ships_3, 0);
            OpponentFieldShip3_2.FetchFromModel(_ships_3, 1);
            OpponentFieldShip4_1.FetchFromModel(_ships_4, 0);
        }
    }

    [BindCommand]
    private void Retry()
    {
        Root.Destroy();
        new Lobby();
    }

    [BindCommand]
    private void Exit()
    {
        Root.Destroy();
    }

    /*private void TestField()
    {
        var json = "[[0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,1,0],[0,0,1,0,0,1,0,0,1,0],[0,0,1,0,0,0,0,0,1,0],[0,0,0,0,0,0,0,0,0,0],[0,0,0,0,1,0,0,1,1,1],[1,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0],[1,0,1,1,1,1,0,1,0,1],[0,0,0,0,0,0,0,1,0,1]]";
        var dataJson = System.Activator.CreateInstance<Data.OpponentFieldData>();
        dataJson.FromJson(SimpleJSON.JSON.Parse(json));
        OnOpponentFieldReceived(dataJson);
    }*/
}
