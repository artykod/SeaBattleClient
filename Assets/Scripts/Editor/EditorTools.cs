using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public static class EditorTools
{
    [MenuItem("GameObject/Game/Create new screen", false, -200)]
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
}
