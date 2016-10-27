using UnityEngine.SceneManagement;
using System.Collections;

public class PreloaderScreen : BindScreen
{
    public Bind<float> Progress;

    public PreloaderScreen() : base("Preloader/Preloader")
    {
        Progress.Value = 0.5f;

        StartCoroutine(LoadMainScene());
    }

    private IEnumerator LoadMainScene()
    {
        var loading = SceneManager.LoadSceneAsync("Main");
        while (!loading.isDone)
        {
            Progress.Value = loading.progress;
            yield return null;
        }

        Progress.Value = loading.progress;

        Destroy();

        SceneManager.UnloadScene(0);
    }
}
