using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LanguageBinding : MonoBehaviour
{
    [SerializeField]
    private string _localeKey;
    private Text _text;

    private void Awake()
    {
        _text = GetComponent<Text>();
        LanguageController.Instance.OnLanguageChanged += OnLanguageChanged;
    }

    private void Start()
    {
        Refresh();
    }

    private void OnLanguageChanged(Language lang)
    {
        Refresh();
    }

    private void Refresh()
    {
        if (_text)
            _text.text = LanguageController.Localize(_localeKey);
        else
            LanguageController.Instance.OnLanguageChanged -= OnLanguageChanged;
    }
}
