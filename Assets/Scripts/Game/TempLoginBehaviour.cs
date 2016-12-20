using UnityEngine;
using System.Collections;

public class TempLoginBehaviour : MonoBehaviour
{
    private IEnumerator Start()
    {
        GameImpl.DebugImpl.Instance = new DebugUnity();
        if (GameConfig.Instance.Config.DebugMode) DebugConsole.Instance.Init();

        yield return null;

        new TempLogin();
    }
}
