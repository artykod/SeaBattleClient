using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class GameFieldWrongMoveAnimation : MonoBehaviour
{
    private Image _image;
    private Color _initialColor;

    public void Play()
    {
        _image.color = _initialColor;
        _image.enabled = true;
    }

    private void Awake()
    {
        _image = GetComponent<Image>();
        _initialColor = _image.color;
        _image.enabled = false;
    }

    private void Update()
    {
        if (!_image.enabled) return;

        var c = _image.color;
        c.a -= Time.deltaTime;
        if (c.a < 0.00001f) _image.enabled = false;
        _image.color = c;
    }
}
