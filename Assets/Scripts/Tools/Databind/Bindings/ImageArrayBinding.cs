using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageArrayBinding : BindViewMonoBehaviour
{
    [SerializeField]
    private Sprite[] _array;
    private Image _image;
    private int _index;

    public void ReplaceArray(Sprite[] array)
    {
        _array = array;
    }

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    protected override void ValueChangedHandler(object value)
    {
        _index = (int)value;
        _image.sprite = _index >= 0 && _index < _array.Length ? _array[_index] : null;
    }

    protected override object GetValueHandler()
    {
        return _index;
    }
}
