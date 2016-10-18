using UnityEngine;

public class Core : MonoBehaviour
{
    public static void Log(string fmt, params object[] args)
    {
        Debug.LogFormat(fmt, args);
    }

    private void Awake()
    {
        new MainMenu();
    }
}