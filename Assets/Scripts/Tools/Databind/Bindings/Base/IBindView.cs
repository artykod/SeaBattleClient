public interface IBindView
{
    string GetName();
    object GetValue();
    void ValueChanged(object value);
}