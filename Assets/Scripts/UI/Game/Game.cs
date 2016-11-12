using UnityEngine;
using System.Collections;

public class Game : EmptyScreenWithBackground
{
    private GameContent _gameContent;
    private bool _lastRequestSuccess;

    public Game() : base("Game/Game")
    {
        Core.Instance.Match.OnMatchReceived += OnMatchReceived;
        Core.Instance.Match.OnMatchNotFound += OnMatchLost;
        StartCoroutine(CheckBattleState());
        IsLoading = true;
    }

    private void OnMatchLost()
    {
        Root.Destroy();
        // TODO: match not found error
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Core.Instance.Match.OnMatchReceived -= OnMatchReceived;
        Core.Instance.Match.OnMatchNotFound -= OnMatchLost;
    }

    private void OnMatchReceived(Data.Match match)
    {
        if (_gameContent == null)
        {
            _gameContent = new GameContent();
            AddLast(_gameContent);
        }
        _gameContent.UpdateData(match);
        IsLoading = false;
        _lastRequestSuccess = true;
    }

    private IEnumerator CheckBattleState()
    {
        do
        {
            if (!_lastRequestSuccess) IsLoading = true;

            _lastRequestSuccess = false;
            Core.Instance.Match.GetCurrentState();

            yield return new WaitForSecondsRealtime(2f);
        }
        while (true);
    }
}
