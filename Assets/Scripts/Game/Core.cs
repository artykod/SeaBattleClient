using UnityEngine;
using Networking;
using System.Collections;
using System.Collections.Generic;

public class Core : MonoBehaviour
{
    private static bool _firstInitDone;
    private static Core _instance;
    public static Core Instance { get; private set; }

    public bool IsLoginDone { get; private set; }
    public ServerApi.Auth Auth { get; private set; }
    public ServerApi.Lobby Lobby { get; private set; }
    public ServerApi.Match Match { get; private set; }
    public Data.CharacterData Character { get; private set; }

    private Dictionary<string, Texture2D> _avatarsCache = new Dictionary<string, Texture2D>();

    public static void Init()
    {
        if (_firstInitDone) return;
        _firstInitDone = true;
        
        if (GameConfig.Instance.Config.DebugMode)
        {
            GameImpl.DebugImpl.Instance = new DebugUnity();
            DebugConsole.Instance.Init();
        }
        else
        {
            GameImpl.DebugImpl.Instance = new DebugIgnore();
        }

        LanguageController.Instance.Initialize();
        SoundController.StartButtonsClickTracker();
    }

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
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        Init();

        Connection.Instance.OnErrorReceived += OnConnectionError;
        Auth = new ServerApi.Auth();
        Auth.OnLogin += OnLoginHandler;
        Lobby = new ServerApi.Lobby();

        IsLoginDone = false;

        if (!PreloaderBehaviour.Used) StartGame();
    }

    private void OnDestroy()
    {
        Connection.Instance.OnErrorReceived -= OnConnectionError;
    }

    private void OnConnectionError(int errorCode)
    {
        var caption = string.Empty;
        switch (errorCode)
        {
            case 400:
            case 404:
            case 500:
                caption = "ERROR " + errorCode;
                break;
            case -1:
                caption = "CONNECTION ERROR";
                break;
            case -2:
                caption = "Website login not implemented yet!";
                break;
        }

        try
        {
            Connection.Instance.DisconnectAll();
            EmptyScreenWithBackground.CloseAll();
        }
        catch (System.Exception e)
        {
            Debug.Log("Exception on disconnect: " + e.Message);
        }

        new EmptyScreen();
        new ErrorDialog(caption).OnClose += d => UnityEngine.SceneManagement.SceneManager.LoadScene("Preloader", UnityEngine.SceneManagement.LoadSceneMode.Single);
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