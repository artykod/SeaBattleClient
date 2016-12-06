using System;
using System.Collections.Generic;
using SimpleJSON;
using Random = UnityEngine.Random;

namespace Data
{
    public enum CurrencyType
    {
        Silver = 0,
        Gold = 1,
    }

    public enum ShipDirection
    {
        Horizontal = 1,
        Vertical = 2,
    }

    public enum ShipType
    {
        Ship1 = 1,
        Ship2 = 2,
        Ship3 = 3,
        Ship4 = 4,
    }

    public enum ShootResultType
    {
        Miss = 2,
        Hit = 3,
        Kill = 4,
    }

    public abstract class BaseData
    {
        public abstract void FromJson(JSONNode node);
        protected abstract void FillJson(JSONNode node);
        public JSONNode ToJson()
        {
            var node = new JSONClass();
            FillJson(node);
            return node;
        }
    }

    public class EmptyData : BaseData
    {
        public override void FromJson(JSONNode node) { }
        protected override void FillJson(JSONNode node) { }
    }

    public class CharacterData : BaseData
    {
        public int Id { get; private set; }
        public string Nick { get; private set; }
        public int GameId { get; private set; }
        public int Silver { get; private set; }
        public int Gold { get; private set; }
        public string Token { get; private set; }

        public override void FromJson(JSONNode node)
        {
            Id = node["id"].AsInt;
            Nick = node["nick"];
            GameId = node["gameId"].AsInt;
            Silver = node["silver"].AsInt;
            Gold = node["gold"].AsInt;
            Token = node["token"];
        }

        protected override void FillJson(JSONNode node)
        {
            node["id"].AsInt = Id;
            node["nick"] = Nick;
            node["gameId"].AsInt = GameId;
            node["silver"].AsInt = Silver;
            node["gold"].AsInt = Gold;
        }
    }

    public class MatchBetData : BaseData
    {
        public int Type { get; private set; }
        public int Value { get; private set; }

        public MatchBetData() { }
        public MatchBetData(CurrencyType currency, int value)
        {
            Type = (int)currency;
            Value = value;
        }

        public override void FromJson(JSONNode node)
        {
            Type = node["type"].AsInt;
            Value = node["val"].AsInt;
        }

        protected override void FillJson(JSONNode node)
        {
            node["type"].AsInt = Type;
            node["val"].AsInt = Value;
        }
    }

    public class CreateMatchRequestData : BaseData
    {
        public MatchBetData Bet { get; private set; }
        public bool WithBot { get; private set; }

        public CreateMatchRequestData(MatchBetData bet, bool withBot)
        {
            Bet = bet;
            WithBot = withBot;
        }

        public override void FromJson(JSONNode node)
        {
            Bet = new MatchBetData();
            Bet.FromJson(node["bet"]);
            WithBot = node["withBot"].AsBool;
        }

        protected override void FillJson(JSONNode node)
        {
            node["bet"] = Bet.ToJson();
            node["withBot"].AsBool = WithBot;
        }
    }

    public class CreateMatchResponseData : BaseData
    {
        public string Key { get; private set; }
        public MatchData Match { get; private set; }

        public override void FromJson(JSONNode node)
        {
            Key = node["key"];
            Match = new MatchData();
            Match.FromJson(node["match"]);
        }

        protected override void FillJson(JSONNode node)
        {
            node["key"] = Key;
            node["match"] = Match.ToJson();
        }
    }

    public class LobbyMatchPlayerData : BaseData
    {
        public int Id { get; private set; }
        public string Nick { get; private set; }

        public LobbyMatchPlayerData() { }
        public LobbyMatchPlayerData(int id, string nick)
        {
            Id = id;
            Nick = nick;
        }

        public override void FromJson(JSONNode node)
        {
            Id = node["id"].AsInt;
            Nick = node["nick"];
        }

        protected override void FillJson(JSONNode node)
        {
            node["id"].AsInt = Id;
            node["nick"] = Nick;
        }
    }

    public class LobbyMatchData : BaseData
    {
        public LobbyMatchPlayerData[] Sides { get; private set; }
        public MatchBetData Bet { get; private set; }

        public LobbyMatchData() { }
        public LobbyMatchData(MatchBetData bet, LobbyMatchPlayerData[] sides)
        {
            Sides = sides;
            Bet = bet;
        }

        public override void FromJson(JSONNode node)
        {
            var sides = node["sides"].AsArray;
            Sides = new LobbyMatchPlayerData[sides.Count];
            for (int i = 0; i < sides.Count; i++)
            {
                if (sides[i].IsNull())
                {
                    Sides[i] = null;
                }
                else
                {
                    Sides[i] = new LobbyMatchPlayerData();
                    Sides[i].FromJson(sides[i]);
                }
            }
            Bet = new MatchBetData();
            Bet.FromJson(node["bet"]);
        }

        protected override void FillJson(JSONNode node)
        {
            var sides = new JSONNode();
            for (int i = 0; i < Sides.Length; i++)
            {
                sides[i] = Sides[i] != null ? Sides[i].ToJson() : null;
            }
            node["sides"] = sides;
            node["bet"] = Bet.ToJson();
        }
    }

    public class LobbyData : BaseData
    {
        public Dictionary<string, LobbyMatchData> Lobby { get; private set; }

        public static LobbyData GenerateStub(int count = 10)
        {
            var lobby = new LobbyData();
            for (int i = 0; i < count; i++)
            {
                lobby.Lobby.Add(Guid.NewGuid().ToString(), new LobbyMatchData(
                    new MatchBetData(Random.value > 0.5f ? CurrencyType.Gold : CurrencyType.Silver, Random.Range(10, 1000)),
                    new LobbyMatchPlayerData[] {
                        new LobbyMatchPlayerData(Random.Range(0, 9999999), Guid.NewGuid().ToString()),
                        Random.value > 0.5f ? null : new LobbyMatchPlayerData(Random.Range(0, 9999999), Guid.NewGuid().ToString()),
                    }));
            }
            return lobby;
        }

        public override void FromJson(JSONNode node)
        {
            Lobby = new Dictionary<string, LobbyMatchData>();
            foreach (var i in node.AsObject.GetAllKeys())
            {
                Lobby[i] = new LobbyMatchData();
                Lobby[i].FromJson(node[i]);
            }
        }

        protected override void FillJson(JSONNode node)
        {
            foreach (var i in Lobby) node[i.Key] = i.Value.ToJson();
        }
    }

    public class MatchData : BaseData
    {
        public FieldStateData My { get; private set; }
        public FieldStateData Opponent { get; private set; }
        public MatchBetData Bet { get; private set; }
        public int TurnNumber { get; private set; }

        public override void FromJson(JSONNode node)
        {
            My = new FieldStateData();
            My.FromJson(node["my"]);
            if (node["opponent"].IsNull())
            {
                Opponent = null;
            }
            else
            {
                Opponent = new FieldStateData();
                Opponent.FromJson(node["opponent"]);
            }
            Bet = new MatchBetData();
            Bet.FromJson(node["bet"]);
            TurnNumber = node["turnCnt"].AsInt;
        }

        protected override void FillJson(JSONNode node)
        {
            node["my"] = My.ToJson();
            node["opponent"] = Opponent.ToJson();
            node["bet"] = Bet.ToJson();
            node["turnCnt"].AsInt = TurnNumber;
        }
    }

    public class MatchUserData : BaseData
    {
        public int Id { get; private set; }
        public string Nick { get; private set; }

        public override void FromJson(JSONNode node)
        {
            Id = node["id"].AsInt;
            Nick = node["nick"];
        }

        protected override void FillJson(JSONNode node)
        {
            node["id"].AsInt = Id;
            node["nick"] = Nick;
        }
    }

    public class FieldStateData : BaseData
    {
        public MatchUserData User { get; private set; }
        public int Status { get; private set; }
        public FieldCellsData Field { get; private set; }
        public FieldShipsCountData FreeShips { get; private set; }
        public FieldShipsCountData AliveShips { get; private set; }

        public override void FromJson(JSONNode node)
        {
            User = new MatchUserData();
            User.FromJson(node["user"]);
            Status = node["status"].AsInt;
            Field = new FieldCellsData();
            Field.FromJson(node["field"]);
            if (node["freeShips"].IsNull())
            {
                FreeShips = null;
            }
            else
            {
                FreeShips = new FieldShipsCountData();
                FreeShips.FromJson(node["freeShips"]);
            }
            AliveShips = new FieldShipsCountData();
            AliveShips.FromJson(node["aliveShips"]);
        }

        protected override void FillJson(JSONNode node)
        {
            node["user"] = User.ToJson();
            node["status"].AsInt = Status;
            node["field"] = Field.ToJson();
            node["freeShips"] = FreeShips.ToJson();
            node["aliveShips"] = AliveShips.ToJson();
        }
    }

    public class OpponentFieldData : BaseData
    {
        public FieldCellsData Field { get; private set; }

        public override void FromJson(JSONNode node)
        {
            Field = new FieldCellsData();
            Field.FromJson(node["field"]);
        }

        protected override void FillJson(JSONNode node)
        {
            node["field"] = Field.ToJson();
        }
    }

    public class FieldCellsData : BaseData
    {
        public List<List<int>> Cells { get; private set; }

        public override void FromJson(JSONNode node)
        {
            var rows = node.AsArray;
            Cells = new List<List<int>>(rows.Count);
            for (int i = 0; i < rows.Count; i++)
            {
                var cols = rows[i].AsArray;
                Cells.Add(new List<int>(cols.Count));
                for (int j = 0; j < cols.Count; j++)
                {
                    Cells[i].Add(cols[j].AsInt);
                }
            }
        }

        protected override void FillJson(JSONNode node)
        {
            for (int i = 0; i < Cells.Count; i++)
            {
                node[i] = new JSONNode();
                for (int j = 0; j < Cells[i].Count; j++)
                {
                    node[i][j].AsInt = Cells[i][j];
                }
            }
        }
    }

    public class FieldShipsCountData : BaseData
    {
        public int Count1 { get; private set; }
        public int Count2 { get; private set; }
        public int Count3 { get; private set; }
        public int Count4 { get; private set; }

        public override void FromJson(JSONNode node)
        {
            Count1 = node["1"].AsInt;
            Count2 = node["2"].AsInt;
            Count3 = node["3"].AsInt;
            Count4 = node["4"].AsInt;
        }

        protected override void FillJson(JSONNode node)
        {
            node["1"].AsInt = Count1;
            node["2"].AsInt = Count2;
            node["3"].AsInt = Count3;
            node["4"].AsInt = Count4;
        }
    }

    public class FieldShipData : BaseData
    {
        public int DirectionRaw { get; private set; }
        public int TypeRaw { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public ShipDirection Direction { get { return (ShipDirection)DirectionRaw; } }
        public ShipType Type { get { return (ShipType)TypeRaw; } }

        public FieldShipData() { }
        public FieldShipData(ShipDirection direction, ShipType shipType, int x, int y)
        {
            DirectionRaw = (int)direction;
            TypeRaw = (int)shipType;
            X = x;
            Y = y;
        }

        public override void FromJson(JSONNode node)
        {
            DirectionRaw = node["direction"].AsInt;
            TypeRaw = node["type"].AsInt;
            X = node["rowIdx"].AsInt;
            Y = node["colIdx"].AsInt;
        }

        protected override void FillJson(JSONNode node)
        {
            node["direction"].AsInt = DirectionRaw;
            node["type"].AsInt = TypeRaw;
            node["rowIdx"].AsInt = X;
            node["colIdx"].AsInt = Y;
        }
    }

    public class SetShipPositionRequestData : BaseData
    {
        public FieldShipData Ship { get; private set; }

        public SetShipPositionRequestData() { }
        public SetShipPositionRequestData(FieldShipData ship)
        {
            Ship = ship;
        }

        public override void FromJson(JSONNode node)
        {
            Ship = new FieldShipData();
            Ship.FromJson(node["ship"]);
        }

        protected override void FillJson(JSONNode node)
        {
            node["ship"] = Ship.ToJson();
        }
    }

    public class ShootResultData : BaseData
    {
        public int ResultRaw { get; private set; }
        public MatchData Match { get; private set; }
        public ShootResultType Result { get { return (ShootResultType)ResultRaw; } }

        public override void FromJson(JSONNode node)
        {
            ResultRaw = node["result"].AsInt;
            Match = new MatchData();
            Match.FromJson(node["match"]);
        }

        protected override void FillJson(JSONNode node)
        {
            node["result"].AsInt = ResultRaw;
            node["match"] = Match.ToJson();
        }
    }

    public class BattleStatisticsData : BaseData
    {
        public int TotalBattles { get; private set; }
        public int WinCount { get; private set; }
        public int LoseCount { get; private set; }
        public int DrawCount { get { return TotalBattles - WinCount - LoseCount; } }

        public override void FromJson(JSONNode node)
        {
            TotalBattles = node["cnt"].AsInt;
            WinCount = node["wins"].AsInt;
            LoseCount = node["losses"].AsInt;
        }

        protected override void FillJson(JSONNode node)
        {
            node["cnt"].AsInt = TotalBattles;
            node["wins"].AsInt = WinCount;
            node["losses"].AsInt = LoseCount;
        }
    }

    public class SendChatMessageData : BaseData
    {
        public string Message { get; private set; }

        public SendChatMessageData() { }
        public SendChatMessageData(string message)
        {
            Message = message;
        }

        public override void FromJson(JSONNode node)
        {
            Message = node["msg"];
        }

        protected override void FillJson(JSONNode node)
        {
            node["msg"] = Message;
        }
    }

    public class ChatMessageData : BaseData
    {
        public int UserId { get; private set; }
        public int Timestamp { get; private set; }
        public string Message { get; private set; }

        public override void FromJson(JSONNode node)
        {
            UserId = node["usrId"].AsInt;
            Timestamp = node["ts"].AsInt;
            Message = node["msg"];
        }

        protected override void FillJson(JSONNode node)
        {
            node["usrId"].AsInt = UserId;
            node["ts"].AsInt = Timestamp;
            node["msg"] = Message;
        }
    }

    public class ChatData : BaseData
    {
        public List<ChatMessageData> Chat { get; private set; }

        public override void FromJson(JSONNode node)
        {
            var chat = node.AsArray;
            Chat = new List<ChatMessageData>(chat.Count);
            for (int i = 0; i < chat.Count; i++)
            {
                Chat.Add(new ChatMessageData());
                Chat[i].FromJson(chat[i]);
            }
        }

        protected override void FillJson(JSONNode node)
        {
            for (int i = 0; i < Chat.Count; i++)
            {
                node[i] = Chat[i].ToJson();
            }
        }
    }
}
