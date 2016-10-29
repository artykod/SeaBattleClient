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

        _player = new PlayerData
        {
            Avatar = "Textures/avatar",
            Name = "Имя игрока",
            Gold = 1000,
            Silver = 9999,
        };

        new Menu(_player);
    }
}