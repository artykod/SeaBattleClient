using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Preloader : EmptyScreenWithBackground
{
    public Bind<float> Progress;
    public Bind<string> LoadingStatus;

    public Preloader() : base("Preloader")
    {
        AddFirst(new BackgroundArt());

        StartCoroutine(ProgressAnimation());
        StartCoroutine(LoadMainScene());
    }

    private IEnumerator ProgressAnimation()
    {
        var time = 2f;
        var value = 0f;
        while (value < time)
        {
            var progress = value / time;
            Progress.Value = progress;
            LoadingStatus.Value = "Loading " + (progress * 100f) + "%";
            value += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    private IEnumerator LoadMainScene()
    {
        yield return SceneManager.LoadSceneAsync("Main", LoadSceneMode.Additive);
        Core.Instance.StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        Destroy();
        yield return SceneManager.UnloadSceneAsync("Preloader");
        Core.Instance.StartGame();
    }
}
