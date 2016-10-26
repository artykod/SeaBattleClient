using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextBinding : BindViewMonoBehaviour
{
    [Serializable]
    protected class StringBinding : BindView
    {
        public string Value { get; private set; }

        protected override void ValueChangedHandler(object value)
        {
            Value = value != null ? value.ToString() : string.Empty;
        }

        protected override object GetValueHandler()
        {
            return Value;
        }
    }

    [SerializeField]
    private string _format = "{0}";
    [SerializeField]
    private StringBinding[] _additionalValues;

    private string _value;
    private Text _text;
    private string[] _valuesCache;

    private void Awake()
    {
        _text = GetComponent<Text>();
    }

    protected override void OnBind()
    {
        base.OnBind();

        InvalidateCache();

        for (int i = 0; i < _additionalValues.Length; i++)
        {
            _additionalValues[i].OnValueChanged -= OnDataChanged;
            _additionalValues[i].OnValueChanged += OnDataChanged;
            _additionalValues[i].Bind(_rootContext);
        }

        RebuildText();
    }

    protected override void OnUnbind()
    {
        base.OnUnbind();

        for (int i = 0; i < _additionalValues.Length; i++)
        {
            _additionalValues[i].Unbind();
        }
    }

    protected override void ValueChangedHandler(object value)
    {
        _value = value != null ? value.ToString() : string.Empty;
        RebuildText();
    }

    protected override object GetValueHandler()
    {
        return _value;
    }

    private void OnDataChanged(IBindView binder)
    {
        RebuildText();
    }

    private void InvalidateCache()
    {
        if (_valuesCache == null || _additionalValues.Length + 1 != _valuesCache.Length)
        {
            _valuesCache = new string[_additionalValues.Length + 1];
        }

        _valuesCache[0] = _value;
        for (int i = 0; i < _additionalValues.Length; i++)
        {
            _valuesCache[i + 1] = _additionalValues[i].Value;
        }
    }

    private void RebuildText()
    {
        if (string.IsNullOrEmpty(_format))
        {
            _text.text = _value;
        }
        else if (_additionalValues.Length < 1)
        {
            _text.text = string.Format(_format, _value);
        }
        else
        {
            InvalidateCache();

            _valuesCache[0] = _value;
            for (int i = 0; i < _additionalValues.Length; i++)
            {
                _valuesCache[i + 1] = _additionalValues[i].Value;
            }

            _text.text = string.Format(_format, _valuesCache);
        }
    }
}
