//#define DEBUG_BATTLE

using UnityEngine;
using Networking;
using System.Collections;
using System.Collections.Generic;

public class Core : MonoBehaviour
{
    private static Core _instance;
    public static Core Instance { get; private set; }

#if UNITY_EDITOR
    static Core()
    {
        GameImpl.DebugImpl.Instance = new DebugUnity();
    }
#endif

    public bool IsLoginDone { get; private set; }
    public ServerApi.Auth Auth { get; private set; }
    public ServerApi.Lobby Lobby { get; private set; }
    public ServerApi.Match Match { get; private set; }
    public Data.CharacterData Character { get; private set; }

    private Dictionary<string, Texture2D> _avatarsCache = new Dictionary<string, Texture2D>();

    public static void OpenUrl(string url)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        Application.ExternalEval(string.Format("window.open(\"{0}\")", url));
#else
        Application.OpenURL(url);
#endif
    }

    public void MakeApiForMatch(string matchToken)
    {
        Match = new ServerApi.Match(matchToken);
        SaveMatchAuth(matchToken);
    }

    private void SaveMatchAuth(string token)
    {
        PlayerPrefs.SetString("match_token", token);
        PlayerPrefs.SetString("auth_cookie", Connection.Instance.GetAuthCookies());
        PlayerPrefs.Save();
    }

    private void LoadMatchAuth()
    {
        Connection.Instance.SetAuthCookies(PlayerPrefs.GetString("auth_cookie"));
        MakeApiForMatch(PlayerPrefs.GetString("match_token"));
    }

    private void Awake()
    {
        //SoundController.Music(SoundController.MUSIC);
        SoundController.StartButtonsClickTracker();

        Instance = this;

        GameImpl.DebugImpl.Instance = new DebugUnity();
        if (GameConfig.Instance.Config.DebugMode) DebugConsole.Instance.Init();
        LanguageController.Instance.Initialize();

        Auth = new ServerApi.Auth();
        Lobby = new ServerApi.Lobby();

#if DEBUG_BATTLE
        IsLoginDone = true;
        LoadMatchAuth();

        new Layout();
#else
        Auth.OnLogin += OnLoginHandler;
        IsLoginDone = false;
#endif

        if (PreloaderBehaviour.Instance == null) StartGame();
    }

    private IEnumerator Start()
    {
#if !DEBUG_BATTLE
        Auth.Login();
#endif
        while (!IsLoginDone) yield return null;
    }

    private void OnLoginHandler(Data.CharacterData character)
    {
        IsLoginDone = true;
        Character = character;
    }

    public void StartGame()
    {
        new Menu();
    }

    public void LoadUserAvatar(int userId, Bind<Texture2D> texture)
    {
        var url = string.Format("{0}/{1}.png", GameConfig.Instance.Config.AvatarsServerUrl, userId);
        if (_avatarsCache.ContainsKey(url))
        {
            texture.Value = _avatarsCache[url];
            return;
        }
        StartCoroutine(LoadUserAvatar(url, texture));
    }

    private IEnumerator LoadUserAvatar(string url, Bind<Texture2D> texture)
    {
        using (var loader = new WWW(url))
        {
            while (!loader.isDone) yield return null;

            if (string.IsNullOrEmpty(loader.error))
            {
                var tex = loader.textureNonReadable;
                if (_avatarsCache.ContainsKey(url))
                {
                    Destroy(_avatarsCache[url]);
                }
                _avatarsCache[url] = tex;
                texture.Value = tex;
            }
            else
            {
                texture.Value = Resources.Load<Texture2D>("Textures/avatar");
            }
        }
    }
}