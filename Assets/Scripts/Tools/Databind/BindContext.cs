using System;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

public interface IBindContext
{
    void ValueChangedFromData(IBindData data);
    void ValueChangedFromView(IBindView view);
    void AddBinding(IBindView view);
    void RemoveBinding(IBindView view);
    void InvokeCommand(string name);
    object GetValue(string name);
    IBindContext GetTargetContext(string name);
}

public interface INeastedBindContext : IBindContext
{
}

public sealed class BindCommandAttribute : Attribute
{
}

public class NeastedBindContext : BindContext, INeastedBindContext
{
}

public class BindContext : IBindContext
{
    private object _bindDomain;
    private Dictionary<string, IBindData> _data = new Dictionary<string, IBindData>();
    private Dictionary<string, MethodInfo> _commands = new Dictionary<string, MethodInfo>();
    private Dictionary<string, HashSet<IBindView>> _views = new Dictionary<string, HashSet<IBindView>>();
    private Dictionary<string, IBindContext> _neastedContext = new Dictionary<string, IBindContext>();

    public BindContext(object bindDomain = null)
    {
        _bindDomain = bindDomain != null ? bindDomain : this;

        var domainType = _bindDomain.GetType();
        var searchFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        var properties = domainType.GetProperties(searchFlags);
        var methods = domainType.GetMethods(searchFlags);
        var fields = domainType.GetFields(searchFlags);

        for (int i = 0, l = fields.Length; i < l; i++)
        {
            var f = fields[i];
            var type = f.FieldType;
            var name = f.Name;

            if (typeof(IBindData).IsAssignableFrom(type) && !name.Contains("__BackingField"))
            {
                var instance = f.GetValue(_bindDomain) as IBindData;
                if (instance == null)
                {
                    instance = Activator.CreateInstance(type, new object[] { this, name }) as IBindData;
                    f.SetValue(_bindDomain, instance);
                }
                _data[name] = instance;
            }

            if (typeof(INeastedBindContext).IsAssignableFrom(type))
            {
                var instance = f.GetValue(_bindDomain) as IBindContext;
                if (instance == null)
                {
                    instance = Activator.CreateInstance(type) as IBindContext;
                    f.SetValue(_bindDomain, instance);
                }
                _neastedContext[name] = instance;
            }
        }

        for (int i = 0, l = properties.Length; i < l; i++)
        {
            var p = properties[i];
            var type = p.PropertyType;
            var name = p.Name;

            if (typeof(IBindData).IsAssignableFrom(type))
            {
                var instance = p.GetValue(_bindDomain, null) as IBindData;
                if (instance == null)
                {
                    instance = Activator.CreateInstance(type, new object[] { this, name }) as IBindData;
                    p.SetValue(_bindDomain, instance, null);
                }
                _data[name] = instance;
            }

            if (typeof(INeastedBindContext).IsAssignableFrom(type))
            {
                var instance = p.GetValue(_bindDomain, null) as IBindContext;
                if (instance == null)
                {
                    instance = Activator.CreateInstance(type) as IBindContext;
                    p.SetValue(_bindDomain, instance, null);
                }
                _neastedContext[name] = instance;
            }
        }

        for (int i = 0, l = methods.Length; i < l; i++)
        {
            var m = methods[i];
            var attr = m.GetCustomAttributes(typeof(BindCommandAttribute), false);
            if (attr != null && attr.Length > 0) _commands[m.Name] = m;
        }
    }

    IBindContext IBindContext.GetTargetContext(string name)
    {
        var parts = name.Split('.');
        if (parts.Length > 1)
        {
            var contextName = parts[0];
            IBindContext context;
            if (_neastedContext.TryGetValue(contextName, out context))
            {
                var neastedName = new StringBuilder(name.Length);
                for (int i = 1; i < parts.Length; i++)
                {
                    neastedName.Append(parts[i]);
                    if (i < parts.Length - 1) neastedName.Append('.');
                }
                return context.GetTargetContext(neastedName.ToString());
            }
            else
            {
                Debug.LogError("Not found context: " + contextName);
                return this;
            }
        }
        else
        {
            return this;
        }
    }

    void IBindContext.AddBinding(IBindView view)
    {
        var name = view.GetName();
        HashSet<IBindView> set;
        if (!_views.TryGetValue(name, out set)) set = _views[name] = new HashSet<IBindView>();
        set.Add(view);
    }

    void IBindContext.RemoveBinding(IBindView view)
    {
        var name = view.GetName();
        HashSet<IBindView> set;
        if (_views.TryGetValue(name, out set))
        {
            set.Remove(view);
            if (set.Count < 1) _views.Remove(name);
        }
    }

    void IBindContext.ValueChangedFromData(IBindData data)
    {
        HashSet<IBindView> set;
        var name = data.GetName();

        if (_views.TryGetValue(name, out set))
        {
            try
            {
                foreach (var i in set) i.ValueChanged(data.GetValue());
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }

    void IBindContext.ValueChangedFromView(IBindView view)
    {
        var name = view.GetName();
        IBindData data;

        if (_data.TryGetValue(name, out data))
        {
            try
            {
                data.ValueChanged(view.GetValue());
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        else
        {
            Debug.LogError("Not found property: " + name);
        }
    }

    object IBindContext.GetValue(string name)
    {
        IBindData data;
        if (_data.TryGetValue(name, out data)) return data.GetValue();
        return null;
    }

    void IBindContext.InvokeCommand(string name)
    {
        MethodInfo mi;
        if (_commands.TryGetValue(name, out mi))
            mi.Invoke(_bindDomain, null);
        else
            Debug.LogError("Not found command: " + name);
    }

    #region Debug

    public string DumpContext(int outputDepth = 1)
    {
        var dump = new StringBuilder(1024);
        var offset = string.Empty;

        if (outputDepth > 1) for (int i = 0; i < outputDepth; i++) offset += '\t';

        dump.AppendLine(offset + "Data count: " + _data.Count);
        foreach (var i in _data)
        {
            dump.AppendLine(offset + string.Format("\tname:{0} value:{1}", i.Key, i.Value.GetValue()));
        }

        dump.AppendLine(offset + "Views count: " + _views.Count);
        foreach (var i in _views)
        {
            dump.AppendLine(offset + string.Format("\tname:{0} count:{1}", i.Key, i.Value.Count));
            foreach (var j in i.Value)
            {
                var name = j is MonoBehaviour ? (j as MonoBehaviour).gameObject.name : "CLR";
                dump.AppendLine(offset + string.Format("\t\ttype:{0} value:{1} ({2})", j.GetType(), j.GetValue(), name));
            }
        }

        dump.AppendLine(offset + "Commands count: " + _commands.Count);
        foreach (var i in _commands)
        {
            dump.AppendLine(offset + string.Format("\tname:{0}", i.Key));
        }

        dump.AppendLine(offset + "Neasted contexts count: " + _neastedContext.Count);
        foreach (var i in _neastedContext)
        {
            dump.AppendLine(offset + "\tContext: " + i.Key);
            dump.Append((i.Value as BindContext).DumpContext(outputDepth + 1));
        }

        return dump.ToString();
    }

    #endregion
}