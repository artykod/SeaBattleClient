using UnityEngine;
using System.Collections;

public class TempLoginBehaviour : MonoBehaviour
{
    private IEnumerator Start()
    {
        Core.Init();

        if (GameConfig.Instance.Config.TempLogin)
        {
            yield return null;
            new TempLogin(LoadGame);
        }
        else
        {
            LoadGame();
        }
    }

    private void LoadGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Preloader", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
