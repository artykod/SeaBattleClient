using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class RawImageBinding : BindViewMonoBehaviour
{
    [SerializeField]
    private bool _tiled;
    [SerializeField]
    private Vector2 _scale = Vector2.one;

    private RawImage _image;

    private void Awake()
    {
        _image = GetComponent<RawImage>();
    }

    private void OnRectTransformDimensionsChange()
    {
        Refresh();
    }

    protected override void ValueChangedHandler(object value)
    {
        _image.texture = value as Texture;
        Refresh();
    }

    protected override object GetValueHandler()
    {
        return _image.texture;
    }

    private void Refresh()
    {
        if (!_image) return;

        var tex2D = _image.texture as Texture2D;
        if (_tiled && tex2D)
        {
            var rect = _image.rectTransform.rect;
            _image.uvRect = new Rect(0f, 0f, rect.width / tex2D.width * _scale.x, rect.height / tex2D.height * _scale.x);
        }
    }
}
