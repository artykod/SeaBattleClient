using UnityEngine;
using UnityEngine.UI;

public abstract class BindingScreen : BindingObject
{
    protected Canvas _canvas;

    public BindingScreen(string screenName) : base("Screens/" + screenName)
    {
        var uiRoot = GameObject.Find("UIRoot").transform;
        Instance.transform.SetParent(uiRoot, false);
        Instance.transform.localScale = Vector3.one;
        Instance.transform.localPosition = Vector3.zero;
        Instance.transform.localRotation = Quaternion.identity;

        _canvas = Instance.AddComponent<Canvas>();
        Instance.AddComponent<GraphicRaycaster>();
    }
}