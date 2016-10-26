using UnityEngine;

public class PositionBinding : BindViewMonoBehaviour
{
    private Transform _selfTransform;

    private void Awake()
    {
        _selfTransform = transform;
    }

    protected override object GetValueHandler()
    {
        return _selfTransform.position;
    }

    protected override void ValueChangedHandler(object value)
    {
        _selfTransform.position = (Vector3)value;
    }
}
