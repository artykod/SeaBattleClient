using UnityEngine;

public class TransformBinding : BindViewMonoBehaviour
{
    [SerializeField]
    private bool _trackPosition;
    [SerializeField]
    private bool _trackRotation;
    [SerializeField]
    private bool _trackScale;

    private Transform _selfTransform;
    private Transform _trackedTransform;

    private void Awake()
    {
        _selfTransform = transform;
    }

    protected override object GetValueHandler()
    {
        return _trackedTransform;
    }

    protected override void ValueChangedHandler(object value)
    {
        _trackedTransform = value as Transform;
    }

    private void Update()
    {
        if (_trackedTransform)
        {
            if (_trackPosition) _selfTransform.position = _trackedTransform.position;
            if (_trackRotation) _selfTransform.rotation = _trackedTransform.rotation;
            if (_trackScale) _selfTransform.localScale = _trackedTransform.localScale;
        }
    }
}
