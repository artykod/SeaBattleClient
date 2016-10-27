using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonColor : MonoBehaviour
{
    private Button _button;
    private Text _text;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _text = GetComponentInChildren<Text>();

        if (!_button || !_text) enabled = false;
    }
}
