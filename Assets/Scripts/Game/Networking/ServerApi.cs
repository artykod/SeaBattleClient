using System;

namespace Networking
{
    public static class ServerApi
    {
        public class Auth
        {
            public event Action<Data.CharacterData> OnLogin = delegate { };

            public bool IsLoggedIn { get; private set; }

            public void Login()
            {
                IsLoggedIn = false;

                if (GameConfig.Instance.Config.TempLogin)
                    Connection.Instance.TempLogin(LoginHandler);
                else
                    Connection.Instance.Login(LoginHandler);
            }

            private void LoginHandler(Data.CharacterData chr)
            {
                IsLoggedIn = true;
                OnLogin(chr);
            }
        }

        public class Lobby
        {
            public event Action<Data.LobbyData> OnLobbyReceived = delegate { };
            public event Action<string, Data.MatchData> OnMatchReceived = delegate { };
            public event Action<Data.BattleStatisticsData> OnBattleStatisticsReceived = delegate { };
            public event Action OnFailMatchCreate = delegate { };

            private void CreateMatchErrorHandler(int errorCode)
            {
                OnFailMatchCreate();
            }

            public void GetLobby()
            {
                Connection.Get<Data.LobbyData>("/m/get", resp => OnLobbyReceived(resp));
            }

            public void CreateMatch(Data.CurrencyType currency, int value, bool withBot)
            {
                Connection.Post<Data.CreateMatchRequestData, Data.CreateMatchResponseData>("/m/new", new Data.CreateMatchRequestData(new Data.MatchBetData(currency, value), withBot), resp => OnMatchReceived(resp.Key, resp.Match), CreateMatchErrorHandler);
            }

            public void GetStatistics()
            {
                Connection.Get<Data.BattleStatisticsData>("/m/stats", resp => OnBattleStatisticsReceived(resp));
            }
        }

        public class Match
        {
            public event Action<Data.MatchData> OnMatchReceived = delegate { };
            public event Action<Data.ShootResultType> OnShootResult = delegate { };
            public event Action<Data.OpponentFieldData> OnOpponentFieldReceived = delegate { };
            public event Action OnMatchNotFound = delegate { };
            public event Action OnFailSendChat = delegate { };
            public event Action<Data.ChatData> OnChatReceived = delegate { };
            public event Action OnLayoutError = delegate { };

            private string _matchToken;

            public Match(string matchToken)
            {
                _matchToken = matchToken;
            }

            private string MatchRequest(string method, params object[] args)
            {
                return string.Format("/m/k/{0}/{1}", _matchToken, string.Format(method, args));
            }

            private void MatchReceived(Data.MatchData match)
            {
                OnMatchReceived(match);
            }

            private void LayoutRequestErrorHandler(int errorCode)
            {
                MatchRequestErrorHandler(errorCode);
                OnLayoutError();
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

            private void SendChatErrorHandler(int errorCode)
            {
                OnFailSendChat();
            }

            public void JoinToMatch(Data.CurrencyType currency, int value)
            {
                Connection.Post<Data.MatchBetData, Data.MatchData>(MatchRequest("new"), new Data.MatchBetData(currency, value), resp => MatchReceived(resp), MatchRequestErrorHandler);
            }

            public void AutolayoutField()
            {
                Connection.Post<Data.EmptyData, Data.MatchData>(MatchRequest("autoSetUp"), new Data.EmptyData(), resp => MatchReceived(resp), LayoutRequestErrorHandler);
            }

            public void ResetField()
            {
                Connection.Post<Data.EmptyData, Data.MatchData>(MatchRequest("reset"), new Data.EmptyData(), resp => MatchReceived(resp), LayoutRequestErrorHandler);
            }

            public void PlaceShip(Data.FieldShipData ship)
            {
                Connection.Post<Data.FieldShipData, Data.MatchData>(MatchRequest("placeShip"), ship, resp => MatchReceived(resp), LayoutRequestErrorHandler);
            }

            public void ChangeShip(int x, int y, Data.FieldShipData ship)
            {
                Connection.Post<Data.FieldShipData, Data.EmptyData>(MatchRequest("changeShip/{0}/{1}", y, x), ship, resp => GetCurrentState(), LayoutRequestErrorHandler);
            }

            public void SendReady()
            {
                Connection.Post<Data.EmptyData, Data.MatchData>(MatchRequest("setReady"), new Data.EmptyData(), resp => MatchReceived(resp), MatchRequestErrorHandler);
            }

            public void SendNotReady()
            {
                Connection.Post<Data.EmptyData, Data.MatchData>(MatchRequest("setNotReady"), new Data.EmptyData(), resp => MatchReceived(resp), MatchRequestErrorHandler);
            }

            public void GetCurrentState()
            {
                Connection.Get<Data.MatchData>(MatchRequest(""), resp => MatchReceived(resp), MatchRequestErrorHandler);
            }

            public void Shoot(int x, int y)
            {
                Connection.Post<Data.EmptyData, Data.ShootResultData>(MatchRequest("shoot/{0}/{1}", x, y), new Data.EmptyData(), resp =>
                {
                    OnMatchReceived(resp.Match);
                    OnShootResult(resp.Result);
                }, MatchRequestErrorHandler);
            }

            public void GetOpponentFieldAfterBattle()
            {
                Connection.Post<Data.EmptyData, Data.OpponentFieldData>(MatchRequest("oppField"), new Data.EmptyData(), resp => OnOpponentFieldReceived(resp), MatchRequestErrorHandler);
            }

            public void SendChatMessage(string message)
            {
                if (string.IsNullOrEmpty(message)) return;
                Connection.Post<Data.SendChatMessageData, Data.EmptyData>(MatchRequest("chat"), new Data.SendChatMessageData(message), resp => { }, SendChatErrorHandler);
            }

            public void RequestChat()
            {
                var method = string.Format("chat/{0}", 0);
                Connection.Get<Data.ChatData>(MatchRequest(method), resp => OnChatReceived(resp));
            }
        }
    }
}