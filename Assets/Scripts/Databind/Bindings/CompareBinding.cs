using UnityEngine;
using System;

public abstract class CompareBinding : BindViewMonoBehaviour
{
    public enum CompareMethod
    {
        Equal,
        Greater,
        GreaterOrEqual,
        Less,
        LessOrEqual,
    }

    [SerializeField]
    private bool _inverted;
    [SerializeField]
    private CompareMethod _compareMethod;
    [SerializeField]
    private int _compareValue = 1;

    private bool _isVisible;

    protected override void ValueChangedHandler(object value)
    {
        var ivalue = (int)Convert.ChangeType(value, typeof(int));
        var result = false;

        switch (_compareMethod)
        {
            case CompareMethod.Equal:
                result = _compareValue == ivalue;
                break;
            case CompareMethod.Greater:
                result = _compareValue > ivalue;
                break;
            case CompareMethod.GreaterOrEqual:
                result = _compareValue >= ivalue;
                break;
            case CompareMethod.Less:
                result = _compareValue < ivalue;
                break;
            case CompareMethod.LessOrEqual:
                result = _compareValue <= ivalue;
                break;
            default:
                result = false;
                break;
        }

        _isVisible = result ^ _inverted;

        ComparingHandler(_isVisible);
    }

    protected override object GetValueHandler()
    {
        return _isVisible;
    }

    protected abstract void ComparingHandler(bool isVisible);
}
