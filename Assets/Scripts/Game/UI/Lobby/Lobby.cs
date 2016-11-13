using UnityEngine;
using System.Collections;

public class Lobby : EmptyScreenWithBackground
{
    private LobbyContent _content;
    private Data.Lobby _lobby;

    public Lobby() : base("Lobby/Lobby")
    {
        IsLoading = true;

        Core.Instance.Lobby.OnLobbyReceived += OnLobbyReceived;
        StartCoroutine(LobbyUpdateTracker());
    }

    protected override void OnDestroy()
    {
        Core.Instance.Lobby.OnLobbyReceived -= OnLobbyReceived;

        base.OnDestroy();
    }

    private void OnLobbyReceived(Data.Lobby lobby)
    {
        IsLoading = false;
        UpdateData(lobby);
    }

    private IEnumerator LobbyUpdateTracker()
    {
        do
        {
            Core.Instance.Lobby.GetLobby();
            yield return new WaitForSecondsRealtime(5f);
        }
        while (true);
    }

    private bool CheckLobbyEquals(Data.Lobby a, Data.Lobby b)
    {
        if (a == null && b != null) return false;
        if (a != null && b == null) return false;

        if (a.Count == b.Count)
        {
            foreach (var i in b) if (!a.ContainsKey(i.Key)) return false;
            return true;
        }

        return false;
    }

    private void UpdateData(Data.Lobby lobby)
    {
        if (CheckLobbyEquals(_lobby, lobby)) return;

        _lobby = lobby;

        if (_content == null)
        {
            AddLast(_content = new LobbyContent(lobby));
        }
        else
        {
            _content.UpdateData(lobby);
        }
    }
}
