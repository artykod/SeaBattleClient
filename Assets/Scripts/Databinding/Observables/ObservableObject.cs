// -------------------------------------
//  Domain		: Avariceonline.com
//  Author		: Nicholas Ventimiglia
//  Product		: Unity3d Foundation
//  Published		: 2015
//  -------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq.Expressions;

namespace Foundation.Databinding
{
    /// <summary>
    /// Implements the IObservableModel for clr objects
    /// </summary>
    public abstract class ObservableObject : IObservableModel
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

        private ObservableMessage _bindingMessage;

        protected ObservableObject()
        {
            _bindingMessage = new ObservableMessage { Sender = this };
            _binder = new ModelBinder(this);
        }

        public void RaiseBindingUpdate(string memberName, object paramater)
        {
            if (_onBindingEvent != null)
            {
                _bindingMessage.Name = memberName;
                _bindingMessage.Value = paramater;
                _onBindingEvent(_bindingMessage);
            }

            _binder.RaiseBindingUpdate(memberName, paramater);
        }

        public void SetValue(string memberName, object paramater)
        {
            _binder.RaiseBindingUpdate(memberName, paramater);
        }

        public void Command(string memberName)
        {
            _binder.Command(memberName);
        }

        public void Command(string memberName, object paramater)
        {
            _binder.Command(memberName, paramater);
        }

        [HideInInspector]
        public object GetValue(string memberName)
        {
            return _binder.GetValue(memberName);
        }

        public object GetValue(string memberName, object paramater)
        {
            return _binder.GetValue(memberName, paramater);
        }

        [HideInInspector]
        public virtual void Dispose()
        {
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

        /// <summary>
        /// Notifies listeners of a change
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="paramater"></param>
        public virtual void NotifyProperty(string memberName, object paramater)
        {
            RaiseBindingUpdate(memberName, paramater);
        }

        /// <summary>
        /// Via CoroutineHandler
        /// </summary>
        /// <param name="routine"></param>
        public Coroutine StartCoroutine(IEnumerator routine)
        {
            return ObservableHandler.Instance.StartCoroutine(routine);
        }

        /// <summary>
        /// Via CoroutineHandler
        /// </summary>
        /// <param name="routine"></param>
        public void StopCoroutine(IEnumerator routine)
        {
            ObservableHandler.Instance.StopCoroutine(routine);
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
}