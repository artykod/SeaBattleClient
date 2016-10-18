using UnityEngine;
using Foundation.Databinding;

public abstract class BindingObject : ObservableObject
{
    private Transform _cachedTransform;

    protected GameObject Instance
    {
        get;
        private set;
    }

    protected Transform transform
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

    protected BindingObject Parent
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

    public virtual void Destroy()
    {
        if (Parent != null)
        {
            Parent.RemoveChild(this);
            Parent = null;
        }
        Object.Destroy(Instance);
        Instance = null;
    }

    protected BindingObject AddChild(BindingObject child)
    {
        child.transform.SetParent(Content, false);
        child.Parent = this;
        return child;
    }

    protected void RemoveChild(BindingObject child)
    {
        if (child.Parent == this)
        {
            child.Parent = null;
            child.transform.SetParent(null, false);
        }
    }
}