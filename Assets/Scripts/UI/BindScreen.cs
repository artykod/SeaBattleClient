using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class EmptyScreenWithBackground : BindModel
{
    private static Stack<EmptyScreenWithBackground> _screens = new Stack<EmptyScreenWithBackground>();

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

    public EmptyScreenWithBackground(string screenName) : base("UI/" + screenName)
    {
        _canvas = Instance.GetComponent<Canvas>();
        _caster = Instance.GetComponent<GraphicRaycaster>();

        AddFirst(new EmptyBackground());

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