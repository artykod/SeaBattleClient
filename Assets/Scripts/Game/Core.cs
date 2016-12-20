using UnityEngine;
using Networking;
using System.Collections;
using System.Collections.Generic;

public class Core : MonoBehaviour
{
    private static Core _instance;
    public static Core Instance { get; private set; }

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
        if (Instance == this) return;
        
        SoundController.StartButtonsClickTracker();

        Instance = this;

        Connection.Instance.OnErrorReceived += OnConnectionError;

        GameImpl.DebugImpl.Instance = new DebugUnity();
        if (GameConfig.Instance.Config.DebugMode) DebugConsole.Instance.Init();
        LanguageController.Instance.Initialize();

        Auth = new ServerApi.Auth();
        Lobby = new ServerApi.Lobby();

        Auth.OnLogin += OnLoginHandler;
        IsLoginDone = false;

        if (!PreloaderBehaviour.Used) StartGame();
    }

    private void OnDestroy()
    {
        Connection.Instance.OnErrorReceived -= OnConnectionError;
    }

    private void OnConnectionError(int errorCode)
    {
        switch (errorCode)
        {
            case 400:
            case 404:
            case 500:
            case -1:
                try
                {
                    Connection.Instance.DisconnectAll();
                    EmptyScreenWithBackground.CloseAll();
                }
                catch (System.Exception e)
                {
                    Debug.Log("Exception on disconnect: " + e.Message);
                }
                var caption = "ERROR";
                if (errorCode > 0) caption += " (" + errorCode + ")";
                new EmptyScreen();
                new ErrorDialog(caption).OnClose += (d) => UnityEngine.SceneManagement.SceneManager.LoadScene("Preloader", UnityEngine.SceneManagement.LoadSceneMode.Single);
                break;
        }
    }

    private IEnumerator Start()
    {
        Auth.Login();
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
                Debug.LogError("Error while loading avatar " + url + ": " + loader.error);
                texture.Value = Resources.Load<Texture2D>("Textures/avatar");
            }
        }
    }
}