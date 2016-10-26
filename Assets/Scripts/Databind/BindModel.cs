using UnityEngine;
using System.Collections;

public abstract class BindModel : BindContext, BindContextMonoBehaviour.IUnityListener
{
    private BindContextMonoBehaviour _contextBehaviour;
    private Transform _cachedTransform;

    public GameObject Instance
    {
        get;
        private set;
    }

    public Transform transform
    {
        get
        {
            if ((object)_cachedTransform == null)
            {
                _cachedTransform = Instance.transform;
            }
            return _cachedTransform;
        }
    }

    protected BindModel Parent
    {
        get;
        set;
    }

    protected virtual Transform Content
    {
        get
        {
            return transform;
        }
    }

    public BindModel(string prefabName = null) : base(null)
    {
        GameObject prefab = null;
        if (!string.IsNullOrEmpty(prefabName))
        {
            prefabName = "Prefabs/" + prefabName;
            prefab = Resources.Load<GameObject>(prefabName);
            if (prefab == null)
            {
                Debug.LogError("Cannot load prefab from resources: " + prefabName);
            }
        }
        Instance = prefab ? Object.Instantiate(prefab) : new GameObject();
        Instance.name = GetType().Name;
        Instance.SetActive(false);
        _contextBehaviour = Instance.AddComponent<BindContextMonoBehaviour>();
        _contextBehaviour.SetContext(this);
        Instance.SetActive(true);
        OnCreate();
    }

    public void Destroy()
    {
        if (Parent != null)
        {
            Parent.RemoveChild(this);
            Parent = null;
        }

        if (Instance)
        {
            Object.Destroy(Instance);
            Instance = null;
        }
    }

    protected BindModel AddFirst(BindModel child, bool forceToRoot = false)
    {
        AddChild(child, forceToRoot).transform.SetAsFirstSibling();
        return child;
    }

    protected BindModel AddLast(BindModel child, bool forceToRoot = false)
    {
        AddChild(child, forceToRoot).transform.SetAsLastSibling();
        return child;
    }

    protected void RemoveChild(BindModel child)
    {
        if (child.Parent == this)
        {
            child.Parent = null;
            child.transform.SetParent(null, false);
        }
    }

    private BindModel AddChild(BindModel child, bool forceToRoot)
    {
        child.transform.SetParent(forceToRoot ? transform : Content, false);
        child.Parent = this;
        return child;
    }

    protected virtual void OnCreate() { }
    protected virtual void Update() { }
    protected virtual void LateUpdate() { }
    protected virtual void OnDestroy() { }

    protected Coroutine StartCoroutine(IEnumerator coroutine)
    {
        return _contextBehaviour.StartCoroutine(coroutine);
    }

    protected void StopCoroutine(Coroutine coroutine)
    {
        _contextBehaviour.StopCoroutine(coroutine);
    }

    void BindContextMonoBehaviour.IUnityListener.Update() { Update(); }
    void BindContextMonoBehaviour.IUnityListener.LateUpdate() { LateUpdate(); }
    void BindContextMonoBehaviour.IUnityListener.OnDestroy() { OnDestroy(); }
}