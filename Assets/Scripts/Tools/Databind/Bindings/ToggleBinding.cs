using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleBinding : BindViewMonoBehaviour
{
    [SerializeField]
    private bool _inverted;

    private Toggle _toggle;

    protected override object GetValueHandler()
    {
        return _toggle.isOn;
    }

    protected override void ValueChangedHandler(object value)
    {
        _toggle.isOn = (bool)value;
    }

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(ValueChanged);
    }

    private void ValueChanged(bool value)
    {
        CommitValue();
    }
}
