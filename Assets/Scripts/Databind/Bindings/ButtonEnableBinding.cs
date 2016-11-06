using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonEnableBinding : CompareBinding
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    protected override void ComparingHandler(bool isVisible)
    {
        _button.interactable = isVisible;
    }
}
