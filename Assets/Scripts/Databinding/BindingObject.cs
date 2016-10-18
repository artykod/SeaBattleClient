using UnityEngine;
using Foundation.Databinding;

public abstract class BindingObject : ObservableObject
{
    public GameObject Instance
    {
        get;
        private set;
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
        child.Instance.transform.SetParent(Instance.transform, false);
        return child;
    }
}