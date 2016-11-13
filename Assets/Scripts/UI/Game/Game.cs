using UnityEngine;
using System.Collections;

public class Game : EmptyScreenWithBackground
{
    private GameContent _gameContent;
    private bool _lastRequestSuccess;
    private bool _isBattleDone;

    public Game() : base("Game/Game")
    {
        Subscribe();
        StartCoroutine(CheckBattleState());
        IsLoading = true;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Unsubscribe();
    }

    private void Subscribe()
    {
        Core.Instance.Match.OnMatchReceived += OnMatchReceived;
        Core.Instance.Match.OnMatchNotFound += OnMatchLost;
        Core.Instance.Match.OnChatReceived += OnChatReceived;
    }

    private void OnChatReceived(Data.Chat chat)
    {
        if (_gameContent != null) _gameContent.UpdateChat(chat);
    }

    private void Unsubscribe()
    {
        Core.Instance.Match.OnMatchReceived -= OnMatchReceived;
        Core.Instance.Match.OnMatchNotFound -= OnMatchLost;
    }

    private void OnMatchLost()
    {
        Root.Destroy();
        new ErrorDialog("error.match_not_found");
    }

    private void OnMatchReceived(Data.Match match)
    {
        if (_gameContent == null)
        {
            _gameContent = new GameContent();
            AddLast(_gameContent);
        }
        _gameContent.UpdateData(match);
        _lastRequestSuccess = true;

        if (match.My != null && match.Opponent != null && (match.My.Status == 3 || match.Opponent.Status == 3)) IsLoading = false;

        CheckEndMatch(match);
    }

    private void CheckEndMatch(Data.Match match)
    {
        if (match == null || match.My == null) return;

        switch (match.My.Status)
        {
            case 4:
                Unsubscribe();
                new MatchLoseDialog().OnClose += (dialog) => ExitFromBattle();
                _isBattleDone = true;
                break;
            case 5:
                Unsubscribe();
                new MatchWinDialog((Data.CurrencyType)match.Bet.Type, match.Bet.Value).OnClose += (dialog) => ExitFromBattle();
                _isBattleDone = true;
                break;
        }
    }

    private void ExitFromBattle()
    {
        Root.Destroy();
    }

    private IEnumerator CheckBattleState()
    {
        do
        {
            if (_isBattleDone) break;

            if (!_lastRequestSuccess) IsLoading = true;

            _lastRequestSuccess = false;
            Core.Instance.Match.GetCurrentState();
            Core.Instance.Match.RequestChat();

            yield return new WaitForSecondsRealtime(2f);
        }
        while (!_isBattleDone);

        IsLoading = false;
    }
}
