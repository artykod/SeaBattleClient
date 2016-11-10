using UnityEngine;
using System.Collections;

public class Game : EmptyScreenWithBackground
{
    private GameContent _gameContent;

    public Game() : base("Game/Game")
    {
        Core.Instance.Match.OnMatchReceived += OnMatchReceived;
        StartCoroutine(CheckBattleState());
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Core.Instance.Match.OnMatchReceived -= OnMatchReceived;
    }

    private void OnMatchReceived(Data.Match match)
    {
        if (_gameContent == null)
        {
            _gameContent = new GameContent();
            AddLast(_gameContent);
        }
        _gameContent.UpdateData(match);
    }

    private IEnumerator CheckBattleState()
    {
        do
        {
            Core.Instance.Match.GetCurrentState();
            yield return new WaitForSecondsRealtime(2f);
        }
        while (true);
    }
}
