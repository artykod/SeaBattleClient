using System;

public interface IBindData
{
    string GetName();
    object GetValue();
    void ValueChanged(object value);
}

public sealed class Bind<T> : IBindData
{
    private IBindContext _context;
    private T _value;

    public event Action<Bind<T>> OnValueChanged = delegate { };

    public string Name
    {
        get;
        private set;
    }

    public T Value
    {
        get
        {
            return _value;
        }
        set
        {
            if (_value == null || !_value.Equals(value))
            {
                _value = value;
                _context.ValueChangedFromData(this);
                OnValueChanged(this);
            }
        }
    }

    public Bind(IBindContext context, string name)
    {
        Name = name;
        _context = context;
        _value = default(T);
    }

    string IBindData.GetName()
    {
        return Name;
    }

    object IBindData.GetValue()
    {
        return _value;
    }

    void IBindData.ValueChanged(object value)
    {
        Value = (T)Convert.ChangeType(value, typeof(T));
    }
}
