using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class BindingScreen : BindingObject
{
    private static Stack<BindingScreen> _screens = new Stack<BindingScreen>();

    protected Canvas _canvas;
    protected GraphicRaycaster _caster;

    protected bool IsVisible
    {
        get
        {
            return _canvas.enabled;
        }
        set
        {
            _canvas.enabled = value;
            _caster.enabled = value;
        }
    }

    public BindingScreen(string screenName) : base("Screens/" + screenName)
    {
        var uiRoot = GameObject.Find("UIRoot").transform;
        Instance.transform.SetParent(uiRoot, false);
        Instance.transform.localScale = Vector3.one;
        Instance.transform.localPosition = Vector3.zero;
        Instance.transform.localRotation = Quaternion.identity;

        _canvas = Instance.AddComponent<Canvas>();
        _caster = Instance.AddComponent<GraphicRaycaster>();

        if (_screens.Count > 0)
        {
            _screens.Peek().IsVisible = false;
        }

        _screens.Push(this);
    }

    public override void Destroy()
    {
        _screens.Pop();

        base.Destroy();

        if (_screens.Count > 0)
        {
            _screens.Peek().IsVisible = true;
        }
    }
}