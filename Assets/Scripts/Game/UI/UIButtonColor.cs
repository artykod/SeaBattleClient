using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonColor : MonoBehaviour
{
    private CustomButton _button;
    private Text _text;

    private void Awake()
    {
        _button = GetComponent<CustomButton>();
        _text = GetComponentInChildren<Text>();

        if (!_button || !_text)
        {
            enabled = false;
            return;
        }

        _button.onStateChanged += OnButtonTransitionStateChanged;
    }

    private void OnButtonTransitionStateChanged(CustomButton.SelectionStatePublic state)
    {
        switch (state)
        {
            case CustomButton.SelectionStatePublic.Normal:
                _text.color = _button.colors.normalColor;
                break;
            case CustomButton.SelectionStatePublic.Highlighted:
                _text.color = _button.colors.highlightedColor;
                break;
            case CustomButton.SelectionStatePublic.Pressed:
                _text.color = _button.colors.pressedColor;
                break;
            case CustomButton.SelectionStatePublic.Disabled:
                _text.color = _button.colors.disabledColor;
                break;
        }
    }
}
