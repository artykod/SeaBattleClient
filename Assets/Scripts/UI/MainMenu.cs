using UnityEngine;
using Foundation.Databinding;

public interface IBindProperty
{
    object GetValue();
    System.Type GetType();
}

public class Property<T> : IBindProperty
{
    public T Value;
    public object GetValue()
    {
        return Value;
    }
    public System.Type GetType()
    {
        return typeof(T);
    }
}

public class MainMenu : ObservableBehaviour
{
    private string _bindedText;
    public string BindedText
    {
        get
        {
            return _bindedText;
        }
        set
        {
            _bindedText = value;
            NotifyProperty("BindedText", _bindedText);
        }
    }

    public Property<string> StrVal
    {
        get;
        set;
    }

    private void Start()
    {
        BindedText = "test text";
        StrVal = new Property<string>();
        StrVal.Value = "str 2";
    }
}
