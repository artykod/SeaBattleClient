using UnityEngine;
using System.Collections.Generic;

public class GameContent : BindModel
{
    #region My field bindings
    public PlayerContext MyInfo;
    public FieldShipsAliveStateContext MyShipsInfo;
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
    #endregion

    #region Opponent field bindings
    public PlayerContext OpponentInfo;
    public FieldShipsAliveStateContext OpponentShipsInfo;
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

    public Bind<bool> IsMyTurn;
    public Bind<bool> IsOpponentTurn;
    public Bind<int> TurnNumber;
    public Bind<string> ChatNewMessage;
    public Bind<bool> IsChatSendEnabled;

    private Data.FieldCells _opponentField;
    private GameFieldInput _fieldInput;
    private FieldCellsContent _myFieldCells;
    private FieldCellsContent _opponentFieldCells;

    private Transform _root;
    protected override Transform Content { get { return !_root ? transform : _root; } }

    public GameContent() : base("UI/Game/GameContent")
    {
        _fieldInput = Instance.GetComponentInChildren<GameFieldInput>();
        _fieldInput.OnCellClick += OnOpponentFieldClick;

        _myFieldCells = new FieldCellsContent();
        _root = transform.FindChild("MyField/CellsRoot");
        AddLast(_myFieldCells);
        _root = transform.FindChild("OpponentField/CellsRoot");
        _opponentFieldCells = new FieldCellsContent();
        AddLast(_opponentFieldCells);
        _root = null;
        
        IsChatSendEnabled.Value = true;
        ChatNewMessage.Value = string.Empty;

        TurnNumber.Value = 1;
    }

    private void OnOpponentFieldClick(int x, int y)
    {
        if (IsMyTurn.Value && _opponentField != null && _opponentField[x][y] == 0)
            Core.Instance.Match.Shoot(x, y);
        else
            _fieldInput.WrongMove();
    }

    private void FillAliveShips(FieldShipsAliveStateContext ships, Data.FieldState fieldState)
    {
        ships.Ship1_1.Value = fieldState.AliveShips.Count1 > 0 ? 0 : 1;
        ships.Ship1_2.Value = fieldState.AliveShips.Count1 > 1 ? 0 : 1;
        ships.Ship1_3.Value = fieldState.AliveShips.Count1 > 2 ? 0 : 1;
        ships.Ship1_4.Value = fieldState.AliveShips.Count1 > 3 ? 0 : 1;
        ships.Ship2_1.Value = fieldState.AliveShips.Count2 > 0 ? 0 : 1;
        ships.Ship2_2.Value = fieldState.AliveShips.Count2 > 1 ? 0 : 1;
        ships.Ship2_3.Value = fieldState.AliveShips.Count2 > 2 ? 0 : 1;
        ships.Ship3_1.Value = fieldState.AliveShips.Count3 > 0 ? 0 : 1;
        ships.Ship3_2.Value = fieldState.AliveShips.Count3 > 1 ? 0 : 1;
        ships.Ship4_1.Value = fieldState.AliveShips.Count4 > 0 ? 0 : 1;
    }

    public void UpdateData(Data.Match match)
    {
        if (match.My != null)
        {
            var _ships_1 = new List<ShipModel>();
            var _ships_2 = new List<ShipModel>();
            var _ships_3 = new List<ShipModel>();
            var _ships_4 = new List<ShipModel>();

            ShipModel.FillAllShips(true, match.My.Field, _ships_1, _ships_2, _ships_3, _ships_4);
            FillAliveShips(MyShipsInfo, match.My);
            _myFieldCells.UpdateData(match.My.Field);
            IsMyTurn.Value = match.My.Status == 3;

            if (match.My.User != null)
            {
                MyInfo.Name.Value = match.My.User.Nick;
                Core.Instance.LoadUserAvatar(match.My.User.Id, MyInfo.Avatar);
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
        }
        else
        {
            IsMyTurn.Value = false;
        }

        if (match.Opponent != null)
        {
            var _ships_1 = new List<ShipModel>();
            var _ships_2 = new List<ShipModel>();
            var _ships_3 = new List<ShipModel>();
            var _ships_4 = new List<ShipModel>();

            ShipModel.FillAllShips(false, match.Opponent.Field, _ships_1, _ships_2, _ships_3, _ships_4);
            FillAliveShips(OpponentShipsInfo, match.Opponent);
            _opponentFieldCells.UpdateData(match.Opponent.Field);
            IsOpponentTurn.Value = match.Opponent.Status == 3;

            if (match.Opponent.User != null)
            {
                OpponentInfo.Name.Value = match.Opponent.User.Nick;
                Core.Instance.LoadUserAvatar(match.Opponent.User.Id, OpponentInfo.Avatar);
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

            _opponentField = match.Opponent.Field;
        }
        else
        {
            IsOpponentTurn.Value = false;
            _opponentField = null;
        }

        TurnNumber.Value = match.TurnNumber;
    }

    [BindCommand]
    private void Back()
    {
        Root.Destroy();
    }

    [BindCommand]
    private void ChatSend()
    {
        Core.Instance.Match.SendChatMessage(ChatNewMessage.Value);
        ChatNewMessage.Value = string.Empty;
    }
}
