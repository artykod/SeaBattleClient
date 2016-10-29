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
            var prev = _canvas.enabled;

            _canvas.enabled = value;
            _caster.enabled = value;

            if (prev != value)
            {
                if (value)
                    OnShowScreen();
                else
                    OnHideScreen();
            }
        }
    }

    public BindScreen(string screenName) : base("UI/" + screenName)
    {
        _canvas = Instance.GetComponent<Canvas>();
        _caster = Instance.GetComponent<GraphicRaycaster>();

        if (_screens.Count > 0) _screens.Peek().IsVisible = false;

        _screens.Push(this);

        OnShowScreen();
    }

    protected override void OnDestroy()
    {
        _screens.Pop();

        base.OnDestroy();

        if (_screens.Count > 0) _screens.Peek().IsVisible = true;
    }

    protected virtual void OnShowScreen() { }
    protected virtual void OnHideScreen() { }
}