using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    private Button _button;
    private Toggle _toggle;

    private void Awake()
    {
        _button = GetComponent<Button>();
        if (_button) _button.onClick.AddListener(OnClick);

        _toggle = GetComponent<Toggle>();
        if (_toggle) _toggle.onValueChanged.AddListener(OnToggle);
    }

    private void OnToggle(bool value)
    {
        SoundController.Sound(SoundController.SOUND_BUTTON_CLICK);
    }

    private void OnClick()
    {
        SoundController.Sound(SoundController.SOUND_BUTTON_CLICK);
    }
}
