using UnityEngine;

public class BindContextMonoBehaviour : MonoBehaviour
{
    public interface IUnityListener
    {
        void Start();
        void Update();
        void LateUpdate();
        void OnDestroy();
    }

    private IBindContext _context;
    private IUnityListener _unityListener;

    public IBindContext GetContext()
    {
        if (_context == null) _context = new BindContext(this);
        return _context;
    }

    public void SetContext(IBindContext context)
    {
        _context = context;
        _unityListener = context as IUnityListener;
    }

    private void Start()
    {
        if (_unityListener != null) _unityListener.Start();
    }

    private void Update()
    {
        if (_unityListener != null) _unityListener.Update();
    }

    private void LateUpdate()
    {
        if (_unityListener != null) _unityListener.LateUpdate();
    }

    private void OnDestroy()
    {
        if (_unityListener != null) _unityListener.OnDestroy();
    }
}