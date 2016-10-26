using UnityEngine;

public abstract class BindView : IBindView
{
    [SerializeField, BindingPropertyName]
    private string _propertyName;
    private string _croppedPropertyName;
    private bool _isPropertyCropped = false;

    protected IBindContext _rootContext;
    protected IBindContext _context;

    protected string Name
    {
        get
        {
            if (!_isPropertyCropped)
            {
                var names = _propertyName.Split('.');
                _croppedPropertyName = names[names.Length - 1];
            }
            return _croppedPropertyName;
        }
    }

    public event System.Action<IBindView> OnValueChanged = delegate { };

    protected abstract object GetValueHandler();
    protected abstract void ValueChangedHandler(object value);
    protected virtual void OnBind() { }
    protected virtual void OnUnbind() { }

    public virtual void Bind(IBindContext context)
    {
        _isPropertyCropped = false;
        if (context != null && !string.IsNullOrEmpty(_propertyName))
        {
            _rootContext = context;
            _context = _rootContext.GetTargetContext(_propertyName);
            _context.AddBinding(this);
            ValueChangedHandler(_context.GetValue(Name));
            OnBind();
        }
    }

    public virtual void Unbind()
    {
        if (_context != null)
        {
            OnUnbind();
            _context.RemoveBinding(this);
            _context = null;
        }
    }

    protected void CommitValue()
    {
        _context.ValueChangedFromView(this);
    }

    string IBindView.GetName()
    {
        return Name;
    }

    object IBindView.GetValue()
    {
        return GetValueHandler();
    }

    void IBindView.ValueChanged(object value)
    {
        ValueChangedHandler(value);
        OnValueChanged(this);
    }
}
