using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageBinding : BindViewMonoBehaviour
{
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    protected override void ValueChangedHandler(object value)
    {
        _image.sprite = value as Sprite;
    }

    protected override object GetValueHandler()
    {
        return _image.sprite;
    }
}
