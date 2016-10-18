using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public static class EditorTools
{
    [MenuItem("GameObject/Game/Create new screen", false, -100)]
    private static void CreateScreen()
    {
        var screen = new GameObject("Screen");
        var canvas = screen.AddComponent<Canvas>();
        var scaler = screen.AddComponent<CanvasScaler>();
        screen.AddComponent<GraphicRaycaster>();

        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.planeDistance = 100;

        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 1f;
    }

    [MenuItem("Assets/Edit in scene", false, -1000)]
    private static void EditInScene()
    {
        var uiRoot = GameObject.Find("UIRoot").transform;
        var go = (PrefabUtility.InstantiatePrefab(Selection.activeGameObject) as GameObject).transform;
        if (go.GetComponent<CanvasScaler>() == null)
        {
            go.SetParent(uiRoot, false);
            go.localScale = Vector3.one;
            go.localPosition = Vector3.zero;
            go.localRotation = Quaternion.identity;
        }
    }
}
