using UnityEngine;

public class TweenRotation : MonoBehaviour
{
    public enum TweenMode
    {
        Normal,
        Loop,
        PingPong,
    }

    [SerializeField]
    private float _rotationStart;
    [SerializeField]
    private float _rotationEnd;
    [SerializeField]
    private float _tweenTime;
    [SerializeField]
    private TweenMode _tweenMode;

    private Transform _transform;
    private float _time;

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        var canTween = true;

        _time += Time.deltaTime;

        if (_tweenMode == TweenMode.Normal && _time > _tweenTime)
            canTween = false;

        if (_tweenMode == TweenMode.Loop)
            _time %= _tweenTime;

        if (_tweenMode == TweenMode.PingPong)
            _time %= _tweenTime; // TODO

        if (canTween)
        {
            var t = Mathf.Clamp01(_time / _tweenTime);
            _transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(_rotationStart, _rotationEnd, t));
        }
    }
}
