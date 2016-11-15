using UnityEngine;

public class TempLoginBehaviour : MonoBehaviour
{
    private void Start()
    {
        GameImpl.DebugImpl.Instance = new DebugUnity();
        if (GameConfig.Instance.Config.DebugMode) DebugConsole.Instance.Init();

        new TempLogin();
    }
}
