using UnityEngine.SceneManagement;
using System.Collections;

public class Preloader : BindScreen
{
    public Bind<float> Progress;

    public Preloader() : base("Preloader/Preloader")
    {
        Progress.Value = 0f;
        StartCoroutine(LoadMainScene());
    }

    private IEnumerator LoadMainScene()
    {
        var total = 3f;
        var fakeLoading = 0f;
        while (fakeLoading < total)
        {
            fakeLoading += UnityEngine.Time.unscaledDeltaTime;
            Progress.Value = fakeLoading / total;
            yield return null;
        }

        var loading = SceneManager.LoadSceneAsync("Main");
        while (!loading.isDone)
        {
            //Progress.Value = loading.progress;
            yield return null;
        }

        //Progress.Value = loading.progress;

        Destroy();

        SceneManager.UnloadScene(0);
    }
}
