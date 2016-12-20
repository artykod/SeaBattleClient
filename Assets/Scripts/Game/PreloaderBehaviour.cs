using UnityEngine;

public class PreloaderBehaviour : MonoBehaviour
{
    public static bool Used { get; private set; }

    private void Awake()
    {
        Used = true;
        new Preloader();
    }
}
