using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonBinding : BindViewMonoBehaviour
{
    private const string VALUE = "[command]";

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        if (_button) _button.onClick.AddListener(OnClickHandler);
    }

    private void OnDestroy()
    {
        if (_button) _button.onClick.RemoveListener(OnClickHandler);
    }

    protected override void ValueChangedHandler(object value)
    {
    }

    protected override object GetValueHandler()
    {
        return VALUE;
    }

    private void OnClickHandler()
    {
        _context.InvokeCommand(Name);
    }
}
