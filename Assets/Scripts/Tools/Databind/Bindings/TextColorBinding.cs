using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextColorBinding : BindViewMonoBehaviour
{
    private Text _text;

    private void Awake()
    {
        _text = GetComponent<Text>();
    }

    protected override void ValueChangedHandler(object value)
    {
        _text.color = (Color)value;
    }

    protected override object GetValueHandler()
    {
        return _text.color;
    }
}
