using UnityEngine;

public class BindingPropertyNameAttribute : PropertyAttribute
{
    public readonly char separator;
    public readonly string helpMessage;

    public BindingPropertyNameAttribute()
    {
        separator = '.';
        helpMessage = "Use dot-separated path\ni.e. Value1 or FieldA.FieldB.Value1";
    }
}