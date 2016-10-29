using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageArrayBinding : BindViewMonoBehaviour
{
    [SerializeField]
    private Sprite[] _array;
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    protected override void ValueChangedHandler(object value)
    {
        var index = (int)value;
        _image.sprite = index >= 0 && index < _array.Length ? _array[index] : null;
    }

    protected override object GetValueHandler()
    {
        return _image.sprite;
    }
}
