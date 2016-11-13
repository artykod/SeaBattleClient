using UnityEngine;

public class Rotation2DBinding : BindViewMonoBehaviour
{
    private Transform _selfTransform;
    private float _rotation;

    private void Awake()
    {
        _selfTransform = transform;
        _rotation = _selfTransform.localRotation.eulerAngles.z;
    }

    protected override object GetValueHandler()
    {
        return _rotation;
    }

    protected override void ValueChangedHandler(object value)
    {
        _rotation = (float)value;
        _selfTransform.localRotation = Quaternion.Euler(0f, 0f, _rotation);
    }
}
