using UnityEngine;
using System.Collections.Generic;

public class GameContent : BindModel
{
    #region My field bindings
    public ShipCountViewContext MyShip1;
    public ShipCountViewContext MyShip2;
    public ShipCountViewContext MyShip3;
    public ShipCountViewContext MyShip4;

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

    public PlayerContext MyInfo;
    #endregion

    #region Opponent field bindings
    public ShipCountViewContext OpponentShip1;
    public ShipCountViewContext OpponentShip2;
    public ShipCountViewContext OpponentShip3;
    public ShipCountViewContext OpponentShip4;

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

    public PlayerContext OpponentInfo;
    #endregion

    public Bind<bool> IsMyTurn;
    public Bind<bool> IsOpponentTurn;
    public Bind<int> TurnNumber;
    public Bind<string> ChatNewMessage;
    public Bind<bool> IsChatSendEnabled;

    private GameFieldInput _fieldInput;
    private FieldCellsContent _myFieldCells;
    private FieldCellsContent _opponentFieldCells;

    private Transform _root;
    protected override Transform Content { get { return !_root ? transform : _root; } }

    public GameContent() : base("UI/Game/GameContent")
    {
        _fieldInput = Instance.GetComponentInChildren<GameFieldInput>();
        _fieldInput.OnCellClick += OnFieldCellClick;

        _myFieldCells = new FieldCellsContent();
        _root = transform.FindChild("CellsRootMy");
        AddLast(_myFieldCells);
        _root = transform.FindChild("CellsRootOpponent");
        _opponentFieldCells = new FieldCellsContent();
        AddLast(_opponentFieldCells);
        _root = null;

        ChatNewMessage.OnValueChanged += (val) => IsChatSendEnabled.Value = !string.IsNullOrEmpty(ChatNewMessage.Value);
        ChatNewMessage.Value = string.Empty;

        TurnNumber.Value = 1;
    }

    private void OnFieldCellClick(int x, int y)
    {
        Core.Instance.Match.Shoot(x, y);
    }

    public void UpdateData(Data.Match match)
    {
        var _ships_1 = new List<ShipModel>();
        var _ships_2 = new List<ShipModel>();
        var _ships_3 = new List<ShipModel>();
        var _ships_4 = new List<ShipModel>();

        // my ships
        if (match.My != null)
        {
            ShipModel.FillAllShips(true, match.My.Field, _ships_1, _ships_2, _ships_3, _ships_4);
            MyShip1.Count.Value = match.My.AliveShips.Count1;
            MyShip2.Count.Value = match.My.AliveShips.Count2;
            MyShip3.Count.Value = match.My.AliveShips.Count3;
            MyShip4.Count.Value = match.My.AliveShips.Count4;
        }

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

        _myFieldCells.UpdateData(match.My.Field);

        _ships_1.Clear();
        _ships_2.Clear();
        _ships_3.Clear();
        _ships_4.Clear();

        // opponent ships
        if (match.Opponent != null)
        {
            ShipModel.FillAllShips(false, match.Opponent.Field, _ships_1, _ships_2, _ships_3, _ships_4);
            OpponentShip1.Count.Value = match.Opponent.AliveShips.Count1;
            OpponentShip2.Count.Value = match.Opponent.AliveShips.Count2;
            OpponentShip3.Count.Value = match.Opponent.AliveShips.Count3;
            OpponentShip4.Count.Value = match.Opponent.AliveShips.Count4;

            _opponentFieldCells.UpdateData(match.Opponent.Field);
        }

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

        // turn setup
        IsMyTurn.Value = match.My.Status == 3;
        IsOpponentTurn.Value = match.Opponent != null ? match.Opponent.Status == 3 : false;

        // my info setup
        if (match.My != null && match.My.User != null)
        {
            MyInfo.Name.Value = match.My.User.Nick;
            Core.Instance.LoadUserAvatar(match.My.User.Id, MyInfo.Avatar);
        }

        // opponent info setup
        if (match.Opponent != null && match.Opponent.User != null)
        {
            OpponentInfo.Name.Value = match.Opponent.User.Nick;
            Core.Instance.LoadUserAvatar(match.Opponent.User.Id, OpponentInfo.Avatar);
        }
    }

    [BindCommand]
    private void Back()
    {
        Root.Destroy();
    }

    [BindCommand]
    private void ChatSend()
    {
        if (!string.IsNullOrEmpty(ChatNewMessage.Value))
        {
            Debug.Log("TODO: send to chat " + ChatNewMessage.Value);
        }
    }
}
