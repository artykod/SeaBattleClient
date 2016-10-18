using UnityEngine;
using Foundation.Databinding;

public abstract class BindingObject : ObservableObject
{
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

    protected virtual Transform Content
    {
        get
        {
            return transform;
        }
    }

    public BindingObject(string prefabName = null)
    {
        Instance = string.IsNullOrEmpty(prefabName) ? new GameObject() : Object.Instantiate(Resources.Load("Prefabs/" + prefabName)) as GameObject;
        Instance.SetActive(false);
        Instance.name = GetType().Name;
        var ctx = Instance.AddComponent<BindingContext>();
        ctx.ContextMode = BindingContext.BindingContextMode.Manual;
        ctx.Model = this;
        Instance.SetActive(true);
    }

    protected BindingObject AddChild(BindingObject child)
    {
        child.transform.SetParent(Content, false);
        return child;
    }
}