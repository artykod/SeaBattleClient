using UnityEngine;

public class Core : MonoBehaviour
{
#if UNITY_EDITOR
    static Core()
    {
        Game.DebugImpl.Instance = new DebugUnity();
    }
#endif

    private PlayerData _player;

    private void Awake()
    {
        Game.DebugImpl.Instance = new DebugUnity();
        DebugConsole.Instance.Init();
        LanguageController.Instance.Initialize();

        _player = new PlayerData
        {
            Avatar = "Textures/avatar",
            Name = "Player " + Random.Range(1, 100500),
            Gold = Random.Range(1, 100500),
            Silver = Random.Range(1, 100500),
        };

        new Menu(_player);
    }
}