using UnityEngine;

public abstract class BindViewMonoBehaviour : MonoBehaviour, IBindView
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

    private void OnEnable()
    {
        _isPropertyCropped = false;
        var bindContext = GetComponentInParent<BindContextMonoBehaviour>();
        if (bindContext && !string.IsNullOrEmpty(_propertyName))
        {
            _rootContext = bindContext.GetContext();
            _context = _rootContext.GetTargetContext(_propertyName);
            _context.AddBinding(this);
            ValueChangedHandler(_context.GetValue(Name));
            OnBind();
        }
    }

    private void OnDisable()
    {
        if (_context != null)
        {
            OnUnbind();
            (_context as IBindContext).RemoveBinding(this);
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
