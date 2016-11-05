using UnityEngine;
using Networking;
using System.Collections;

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

    public Data.Character Character { get; private set; }

    private void Awake()
    {
        Instance = this;

        GameImpl.DebugImpl.Instance = new DebugUnity();
        DebugConsole.Instance.Init();
        LanguageController.Instance.Initialize();

        Auth = new ServerApi.Auth();
        Lobby = new ServerApi.Lobby();

        Auth.OnLogin += OnLoginHandler;
        IsLoginDone = false;
    }

    private IEnumerator Start()
    {
        Auth.Login();

        while (!IsLoginDone)
        {
            yield return null;
        }
    }

    private void OnLoginHandler(Data.Character character)
    {
        IsLoginDone = true;
        Character = character;
    }

    public void StartGame()
    {
        new Menu();
    }
}