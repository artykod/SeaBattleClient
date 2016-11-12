using UnityEngine.SceneManagement;
using System.Collections;

public class Preloader : EmptyScreenWithBackground
{
    public Bind<float> Progress;
    public Bind<string> LoadingStatus;

    public Preloader() : base("Preloader")
    {
        AddFirst(new BackgroundArt());
        
        Progress.Value = 0f;
        StartCoroutine(LoadMainScene());
    }

    private IEnumerator LoadMainScene()
    {
        var loading = SceneManager.LoadSceneAsync("Main", LoadSceneMode.Additive);
        while (!loading.isDone)
        {
            Progress.Value = loading.progress;
            LoadingStatus.Value = "Loading scene: " + (loading.progress * 100f) + "%";
            yield return null;
        }
        Progress.Value = loading.progress;
        Destroy();
        SceneManager.UnloadScene(0);

        Core.Instance.StartGame();
    }
}
