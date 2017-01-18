using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public abstract class BindModel : BindContext, BindContextMonoBehaviour.IUnityListener
{
    private BindContextMonoBehaviour _contextBehaviour;
    private Transform _cachedTransform;
    private int _firstSiblingCounter;
    private HashSet<BindModel> _children;

    // TODO move to game class
    public Bind<bool> IsSoundEnabled { get; private set; }

    public GameObject Instance
    {
        get;
        private set;
    }

    public Transform transform
    {
        get
        {
            if ((object)_cachedTransform == null) _cachedTransform = Instance.transform;
            return _cachedTransform;
        }
    }

    protected BindModel Parent
    {
        get;
        set;
    }

    protected BindModel Root
    {
        get
        {
            return Parent == null ? this : Parent.Root;
        }
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
            if (prefab == null) Debug.LogError("Cannot load prefab from resources: " + prefabName);
        }
        Instance = prefab ? Object.Instantiate(prefab) : new GameObject();
        Instance.name = GetType().Name;
        Instance.SetActive(false);
        _contextBehaviour = Instance.AddComponent<BindContextMonoBehaviour>();
        _contextBehaviour.SetContext(this);
        _children = new HashSet<BindModel>();
        Instance.SetActive(true);
        OnCreate();

        // TODO move to game class
        IsSoundEnabled.Value = SoundController.IsSoundEnabled;
        IsSoundEnabled.OnValueChanged += val =>
        {
            SoundController.IsSoundEnabled = val.Value;
            Core.Instance.SaveSettings();
        };
    }

    public void Destroy()
    {
        Clear();

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

    protected BindModel AddFirst(BindModel child)
    {
        AddChild(child).transform.SetSiblingIndex(_firstSiblingCounter++);
        return child;
    }

    protected BindModel AddLast(BindModel child)
    {
        AddChild(child).transform.SetAsLastSibling();
        return child;
    }

    protected void RemoveChild(BindModel child)
    {
        _children.Remove(child);
        if (child.Parent == this)
        {
            child.Parent = null;
            child.transform.SetParent(null, false);
        }
    }

    protected void Clear()
    {
        var children = new List<BindModel>(_children.Count);
        foreach (var i in _children) children.Add(i);
        foreach (var i in children) i.Destroy();
        _children.Clear();
    }

    private BindModel AddChild(BindModel child)
    {
        _children.Add(child);
        child.transform.SetParent(Content, false);
        child.Parent = this;
        return child;
    }

    protected virtual void OnCreate() { }
    protected virtual void OnStart()
    {
        var allButtons = Instance.GetComponentsInChildren<Button>(true);
        for (int i = 0; i < allButtons.Length; i++)
        {
            var btnSound = allButtons[i].GetComponent<ButtonSound>();
            if (btnSound == null) btnSound = allButtons[i].gameObject.AddComponent<ButtonSound>();
        }

        var allToggles = Instance.GetComponentsInChildren<Toggle>(true);
        for (int i = 0; i < allToggles.Length; i++)
        {
            var btnSound = allToggles[i].GetComponent<ButtonSound>();
            if (btnSound == null) btnSound = allToggles[i].gameObject.AddComponent<ButtonSound>();
        }
    }
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

    void BindContextMonoBehaviour.IUnityListener.Start() { OnStart(); }
    void BindContextMonoBehaviour.IUnityListener.Update() { Update(); }
    void BindContextMonoBehaviour.IUnityListener.LateUpdate() { LateUpdate(); }
    void BindContextMonoBehaviour.IUnityListener.OnDestroy() { OnDestroy(); }
}