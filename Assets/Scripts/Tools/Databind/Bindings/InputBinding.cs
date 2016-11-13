using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class InputBinding : BindViewMonoBehaviour
{
    private InputField _inputField;

    private void Awake()
    {
        _inputField = GetComponent<InputField>();
        _inputField.onEndEdit.AddListener(ValueChanged);
    }

    private void OnDestroy()
    {
        _inputField.onValueChanged.RemoveListener(ValueChanged);
    }

    protected override void ValueChangedHandler(object value)
    {
        _inputField.text = value != null ? value.ToString() : string.Empty;
    }

    protected override object GetValueHandler()
    {
        return _inputField.text;
    }

    private void ValueChanged(string value)
    {
        CommitValue();
    }
}
