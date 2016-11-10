using UnityEngine;

public class PreloaderBehaviour : MonoBehaviour
{
    public static PreloaderBehaviour Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        new Preloader();
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
