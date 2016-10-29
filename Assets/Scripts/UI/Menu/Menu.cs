﻿using UnityEngine;

public class Menu : BindScreen
{
    private PlayerData _player;

    public SocialContext Social { get; private set; }
    public PlayerContext Player { get; private set; }

    public Bind<string> TextPlay { get; private set; }
    public Bind<string> TextDashboard { get; private set; }
    public Bind<string> TextSettings { get; private set; }
    public Bind<string> TextRules { get; private set; }
    public Bind<bool> IsSoundEnabled { get; private set; }

    public Menu(PlayerData player) : base("Menu/Menu")
    {
        _player = player;

        Player.Avatar.Value = Resources.Load<Texture2D>(_player.Avatar);
        Player.Name.Value = _player.Name;
        Player.Gold.Value = _player.Gold;
        Player.Silver.Value = _player.Silver;

        TextPlay.Value = "Играть";
        TextDashboard.Value = "Личный кабинет";
        TextSettings.Value = "Настройки";
        TextRules.Value = "Правила";

        IsSoundEnabled.Value = true;
    }

    [BindCommand]
    private void Play()
    {

    }

    [BindCommand]
    private void Dashboard()
    {
        new Dashboard(_player);
    }

    [BindCommand]
    private void Settings()
    {
        new Settings();
    }

    [BindCommand]
    private void Rules()
    {
        new Rules();
    }
}