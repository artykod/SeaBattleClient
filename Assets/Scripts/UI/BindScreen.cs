using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class BindScreen : BindModel
{
    private static Stack<BindScreen> _screens = new Stack<BindScreen>();

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

    public BindScreen(string screenName) : base("UI/" + screenName)
    {
        _canvas = Instance.GetComponent<Canvas>();
        _caster = Instance.GetComponent<GraphicRaycaster>();

        if (_screens.Count > 0) _screens.Peek().IsVisible = false;

        _screens.Push(this);
    }

    protected override void OnDestroy()
    {
        _screens.Pop();

        base.OnDestroy();

        if (_screens.Count > 0) _screens.Peek().IsVisible = true;
    }
}