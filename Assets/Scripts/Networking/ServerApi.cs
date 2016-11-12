using System;

namespace Networking
{
    public static class ServerApi
    {
        public class Auth
        {
            public event Action<Data.Character> OnLogin = delegate { };

            public bool IsLoggedIn { get; private set; }

            public void Login()
            {
                IsLoggedIn = false;

                Connection.Instance.Login((chr) =>
                {
                    IsLoggedIn = true;
                    OnLogin(chr);
                });
            }
        }

        public class Lobby
        {
            public event Action<Data.Lobby> OnLobbyReceived = delegate { };
            public event Action<string, Data.Match> OnMatchReceived = delegate { };
            public event Action<Data.BattleStatistics> OnBattleStatisticsReceived = delegate { };

            public void GetLobby()
            {
                Connection.Get<Data.Lobby>("/m/get", resp => OnLobbyReceived(resp));
            }

            public void CreateMatch(Data.CurrencyType currency, int value, bool withBot)
            {
                Connection.Post<Data.CreateMatchRequest, Data.CreateMatchResponse>("/m/new", new Data.CreateMatchRequest(new Data.MatchBet(currency, value), withBot), resp => OnMatchReceived(resp.Key, resp.Match));
            }

            public void GetStatistics()
            {
                Connection.Get<Data.BattleStatistics>("/m/stats", resp => OnBattleStatisticsReceived(resp));
            }
        }

        public class Match
        {
            public event Action<Data.Match> OnMatchReceived = delegate { };
            public event Action<Data.ShootResultType> OnShootResult = delegate { };
            public event Action<Data.OpponentField> OnOpponentFieldReceived = delegate { };
            public event Action OnMatchNotFound = delegate { };

            private string _matchToken;

            public Match(string matchToken)
            {
                _matchToken = matchToken;
            }

            private string MatchRequest(string method, params object[] args)
            {
                return string.Format("/m/k/{0}/{1}", _matchToken, string.Format(method, args));
            }

            private void MatchReceived(Data.Match match)
            {
                OnMatchReceived(match);
            }

            private void LayoutRequestErrorHandler(int errorCode)
            {
                MatchRequestErrorHandler(errorCode);
            }

            private void MatchRequestErrorHandler(int errorCode)
            {
                switch (errorCode)
                {
                    case 404:
                        OnMatchNotFound();
                        break;
                }
            }

            public void JoinToMatch(Data.CurrencyType currency, int value)
            {
                Connection.Post<Data.MatchBet, Data.Match>(MatchRequest("new"), new Data.MatchBet(currency, value), resp => MatchReceived(resp), MatchRequestErrorHandler);
            }

            public void AutolayoutField()
            {
                Connection.Post<Data.Empty, Data.Match>(MatchRequest("autoSetUp"), new Data.Empty(), resp => MatchReceived(resp), LayoutRequestErrorHandler);
            }

            public void ResetField()
            {
                Connection.Post<Data.Empty, Data.Match>(MatchRequest("reset"), new Data.Empty(), resp => MatchReceived(resp), LayoutRequestErrorHandler);
            }

            public void PlaceShip(Data.FieldShip ship)
            {
                Connection.Post<Data.SetShipPositionRequest, Data.Match>(MatchRequest("placeShip"), new Data.SetShipPositionRequest(ship), resp => MatchReceived(resp), LayoutRequestErrorHandler);
            }

            public void ChangeShip(int x, int y, Data.FieldShip ship)
            {
                Connection.Post<Data.SetShipPositionRequest, Data.Match>(MatchRequest("changeShip/{0}/{1}", x, y), new Data.SetShipPositionRequest(ship), resp => MatchReceived(resp), LayoutRequestErrorHandler);
            }

            public void SendReady()
            {
                Connection.Post<Data.Empty, Data.Match>(MatchRequest("setReady"), new Data.Empty(), resp => MatchReceived(resp), MatchRequestErrorHandler);
            }

            public void SendNotReady()
            {
                Connection.Post<Data.Empty, Data.Match>(MatchRequest("setNotReady"), new Data.Empty(), resp => MatchReceived(resp), MatchRequestErrorHandler);
            }

            public void GetCurrentState()
            {
                Connection.Get<Data.Match>(MatchRequest(""), resp => MatchReceived(resp), MatchRequestErrorHandler);
            }

            public void Shoot(int x, int y)
            {
                Connection.Post<Data.Empty, Data.ShootResult>(MatchRequest("shoot/{0}/{1}", x, y), new Data.Empty(), resp =>
                {
                    OnMatchReceived(resp.Match);
                    OnShootResult(resp.Result);
                }, MatchRequestErrorHandler);
            }

            public void GetOpponentFieldAfterBattle()
            {
                Connection.Post<Data.Empty, Data.OpponentField>(MatchRequest("oppField"), new Data.Empty(), resp => OnOpponentFieldReceived(resp), MatchRequestErrorHandler);
            }
        }
    }
}