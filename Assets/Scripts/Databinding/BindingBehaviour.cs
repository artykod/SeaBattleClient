using System;
using System.Collections.Generic;
using UnityEngine;
using Foundation.Databinding;
using System.Linq.Expressions;

/// <summary>
/// Implements the IObservableModel for mono behavior objects
/// </summary>
public abstract class BindingBehaviour : BindingContext, IObservableModel
{
    private Action<ObservableMessage> _onBindingEvent = delegate { };
    public event Action<ObservableMessage> OnBindingUpdate
    {
        add
        {
            _onBindingEvent = (Action<ObservableMessage>)Delegate.Combine(_onBindingEvent, value);
        }
        remove
        {
            _onBindingEvent = (Action<ObservableMessage>)Delegate.Remove(_onBindingEvent, value);
        }
    }

    private ModelBinder _binder;

    private bool _isDisposed;

    private ObservableMessage _bindingMessage;

    private ModelBinder Binder
    {
        get
        {
            if (_binder == null && !_isDisposed)
                _binder = new ModelBinder(this);
            return _binder;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    protected bool IsApplicationQuit
    {
        get; private set;
    }

    /// <summary>
    /// Virtual, Initializes Binder
    /// </summary>
    protected virtual void Awake()
    {
        if (_bindingMessage == null)
            _bindingMessage = new ObservableMessage { Sender = this };
        if (_binder == null)
            _binder = new ModelBinder(this);
    }

    /// <summary>
    /// Virtual, Disposes
    /// </summary>
    protected virtual void OnDestroy()
    {
        if (IsApplicationQuit)
            return;

        Dispose();
    }

    [HideInInspector]
    public virtual void Dispose()
    {
        _isDisposed = true;

        if (_binder != null)
        {
            _binder.Dispose();
        }

        if (_bindingMessage != null)
        {
            _bindingMessage.Dispose();
        }

        _bindingMessage = null;
        _binder = null;
    }

    [HideInInspector]
    public void RaiseBindingUpdate(string memberName, object paramater)
    {
        if (_bindingMessage == null)
            _bindingMessage = new ObservableMessage { Sender = this };

        Binder.RaiseBindingUpdate(memberName, paramater);

        if (_onBindingEvent != null)
        {
            _bindingMessage.Name = memberName;
            _bindingMessage.Value = paramater;
            _onBindingEvent(_bindingMessage);
        }
    }


    [HideInInspector]
    public void Command(string memberName)
    {
        _binder.Command(memberName);
    }

    [HideInInspector]
    public void Command(string memberName, object paramater)
    {
        _binder.Command(memberName, paramater);
    }

    [HideInInspector]
    public object GetValue(string memberName)
    {
        return Binder.GetValue(memberName);
    }

    [HideInInspector]
    public object GetValue(string memberName, object paramater)
    {

        return Binder.GetValue(memberName, paramater);
    }

    [HideInInspector]
    public virtual void NotifyProperty(string memberName, object paramater)
    {
        RaiseBindingUpdate(memberName, paramater);
    }

    protected virtual void OnApplicationQuit()
    {
        IsApplicationQuit = true;
    }

    protected bool Set<T>(ref T variable, T value, Expression<Func<T>> propertyAccessor)
    {
        if (!EqualityComparer<T>.Default.Equals(variable, value))
        {
            variable = value;
            NotifyProperty((propertyAccessor.Body as MemberExpression).Member.Name, value);
            return true;
        }
        return false;
    }
}