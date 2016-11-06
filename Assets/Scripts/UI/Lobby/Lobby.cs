using UnityEngine;

public class Lobby : EmptyScreenWithBackground
{
    private LobbyContent _content;

    public Lobby() : base("Lobby/Lobby")
    {
        IsLoading = true;

        Core.Instance.Lobby.OnLobbyReceived += OnLobbyReceived;
        Core.Instance.Lobby.GetLobby();
    }

    protected override void OnDestroy()
    {
        Core.Instance.Lobby.OnLobbyReceived -= OnLobbyReceived;

        base.OnDestroy();
    }

    private void OnLobbyReceived(Data.Lobby lobby)
    {
        IsLoading = false;

        lobby = new Data.Lobby();
        for (int i = 0; i < 10; i++)
        {
            lobby.Add("match_token_" + i, new Data.LobbyMatch
            {
                Bet = new Data.MatchBet(Random.value > 0.5f ? Data.CurrencyType.Gold : Data.CurrencyType.Silver, Random.Range(10, 1000)),
                Sides = new Data.LobbyMatchPlayer[]
                {
                    new Data.LobbyMatchPlayer()
                    {
                        Id = 0,
                        Nick = "Nick " + i,
                    },
                    Random.value > 0.5f ? null : new Data.LobbyMatchPlayer()
                    {
                        Id = 1,
                        Nick = "Enemy " + i,
                    },
                },
            });
        }

        if (_content == null)
            AddLast(_content = new LobbyContent(lobby));
        else
            _content.UpdateData(lobby);
    }
}
