using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Data
{
    public enum CurrencyType
    {
        Silver = 0,
        Gold = 1,
    }

    public enum ShipDirection
    {
        Vertical = 1,
        Horizontal = 2,
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

    public class Empty
    {
    }

    public class Character
    {
        [JsonProperty("id")]
        public int Id { get; private set; }
        [JsonProperty("nick")]
        public string Nick { get; private set; }
        [JsonProperty("gameId")]
        public int GameId { get; private set; }
        [JsonProperty("silver")]
        public int Silver { get; private set; }
        [JsonProperty("gold")]
        public int Gold { get; private set; }

        [JsonIgnore]
        public string Avatar { get { return "Textures/avatar"; } }
    }

    public class MatchBet
    {
        [JsonProperty("type")]
        public int Type { get; private set; }
        [JsonProperty("val")]
        public int Value { get; private set; }

        public MatchBet(CurrencyType currency, int value)
        {
            Type = (int)currency;
            Value = value;
        }
    }

    public class CreateMatchRequest
    {
        [JsonProperty("bet")]
        public MatchBet Bet { get; private set; }
        [JsonProperty("withBot")]
        public bool WithBot { get; private set; }

        public CreateMatchRequest(MatchBet bet, bool withBot)
        {
            Bet = bet;
            WithBot = withBot;
        }
    }

    public class CreateMatchResponse
    {
        [JsonProperty("key")]
        public string Key { get; private set; }
        [JsonProperty("match")]
        public Match Match { get; private set; }
    }

    public class LobbyMatchPlayer
    {
        [JsonProperty("id")]
        public int Id { get; private set; }
        [JsonProperty("nick")]
        public string Nick { get; private set; }

        public LobbyMatchPlayer(int id, string nick)
        {
            Id = id;
            Nick = nick;
        }
    }

    public class LobbyMatch
    {
        [JsonProperty("sides")]
        public LobbyMatchPlayer[] Sides { get; private set; }
        [JsonProperty("bet")]
        public MatchBet Bet { get; private set; }

        public LobbyMatch(MatchBet bet, LobbyMatchPlayer[] sides)
        {
            Sides = sides;
            Bet = bet;
        }
    }

    public class Lobby : Dictionary<string, LobbyMatch>
    {
        public static Lobby GenerateStub(int count = 10)
        {
            var lobby = new Lobby();
            for (int i = 0; i < count; i++)
                lobby.Add(System.Guid.NewGuid().ToString(), new LobbyMatch(
                    new MatchBet(Random.value > 0.5f ? CurrencyType.Gold : CurrencyType.Silver, Random.Range(10, 1000)),
                    new LobbyMatchPlayer[] {
                        new LobbyMatchPlayer(Random.Range(0, 9999999), System.Guid.NewGuid().ToString()),
                        Random.value > 0.5f ? null : new LobbyMatchPlayer(Random.Range(0, 9999999), System.Guid.NewGuid().ToString()),
                    }));
            return lobby;
        }
    }

    public class Match
    {
        [JsonProperty("my")]
        public FieldState My { get; private set; }
        [JsonProperty("opponent")]
        public FieldState Opponent { get; private set; }
        [JsonProperty("bet")]
        public MatchBet Bet { get; private set; }
    }

    public class FieldState
    {
        [JsonProperty("status")]
        public int Status { get; private set; }
        [JsonProperty("field")]
        public FieldCells Field { get; private set; }
        [JsonProperty("freeShips")]
        public FieldShipsCount FreeShips { get; private set; }
        [JsonProperty("aliveShips")]
        public FieldShipsCount AliveShips { get; private set; }
    }

    public class OpponentField
    {
        [JsonProperty("field")]
        public FieldCells Field { get; private set; }
    }

    public class FieldCells : List<List<int>>
    {
    }

    public class FieldShipsCount
    {
        [JsonProperty("1")]
        public int Count1 { get; private set; }
        [JsonProperty("2")]
        public int Count2 { get; private set; }
        [JsonProperty("3")]
        public int Count3 { get; private set; }
        [JsonProperty("4")]
        public int Count4 { get; private set; }
    }

    public class FieldShip
    {
        [JsonProperty("direction")]
        public int DirectionRaw { get; private set; }
        [JsonProperty("type")]
        public int TypeRaw { get; private set; }
        [JsonProperty("colIdx")]
        public int X { get; private set; }
        [JsonProperty("rowIdx")]
        public int Y { get; private set; }

        [JsonIgnore]
        public ShipDirection Direction { get { return (ShipDirection)DirectionRaw; } }
        [JsonIgnore]
        public ShipType Type { get { return (ShipType)TypeRaw; } }

        public FieldShip(ShipDirection direction, ShipType shipType, int x, int y)
        {
            DirectionRaw = (int)direction;
            TypeRaw = (int)shipType;
            X = x;
            Y = y;
        }
    }

    public class SetShipPositionRequest
    {
        [JsonProperty("ship")]
        public FieldShip Ship { get; private set; }

        public SetShipPositionRequest(FieldShip ship)
        {
            Ship = ship;
        }
    }

    public class ShootResult
    {
        [JsonProperty("result")]
        public int ResultRaw { get; private set; }
        [JsonProperty("match")]
        public Match Match { get; private set; }

        [JsonIgnore]
        public ShootResultType Result { get { return (ShootResultType)ResultRaw; } }
    }

    public class BattleStatistics
    {
        [JsonProperty("cnt")]
        public int TotalBattles { get; private set; }
        [JsonProperty("wins")]
        public int WinCount { get; private set; }
        [JsonProperty("losses")]
        public int LoseCount { get; private set; }
        [JsonIgnore]
        public int DrawCount { get { return TotalBattles - WinCount - LoseCount; } }
    }
}
