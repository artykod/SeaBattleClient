using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class MockupSceneTracker
{
    private const string MOCKUP_SCENE_NAME = "mockup";
    private const string PATH_TO_MOCKUP_SCENE = "Assets/Scenes/{0}.unity";

    private static bool IsLoaded
    {
        get
        {
            return PlayerPrefs.GetInt("__mockupLoaded", 0) > 0;
        }
        set
        {
            PlayerPrefs.SetInt("__mockupLoaded", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    private static bool IsOpened
    {
        get
        {
            return PlayerPrefs.GetInt("__mockupOpened", 0) > 0;
        }
        set
        {
            PlayerPrefs.SetInt("__mockupOpened", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    static MockupSceneTracker()
    {
        EditorApplication.playmodeStateChanged -= ChangedPlayingMode;
        EditorApplication.playmodeStateChanged += ChangedPlayingMode;
    }

    private static void ChangedPlayingMode()
    {
        if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
        {
            var scene = EditorSceneManager.GetSceneByName(MOCKUP_SCENE_NAME);
            IsLoaded = scene.isLoaded;
            IsOpened = EditorSceneManager.CloseScene(scene, false);
        }

        if (IsOpened && !EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
        {
            EditorSceneManager.OpenScene(string.Format(PATH_TO_MOCKUP_SCENE, MOCKUP_SCENE_NAME), 
                IsLoaded ? OpenSceneMode.Additive : OpenSceneMode.AdditiveWithoutLoading);
        }
    }
}
