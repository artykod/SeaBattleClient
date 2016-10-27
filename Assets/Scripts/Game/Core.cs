using UnityEngine;

public class Core : MonoBehaviour
{
    private PlayerData _player;

    private void Awake()
    {
        _player = new PlayerData
        {
            Avatar = "Textures/avatar",
            Name = "Имя игрока",
            Gold = 1000,
            Silver = 9999,
        };

        new MenuScreen(_player);
    }
}