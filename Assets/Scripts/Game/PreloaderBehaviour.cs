using UnityEngine;

public class PreloaderBehaviour : MonoBehaviour
{
    private void Awake()
    {
        new Preloader();
    }
}
