﻿using System.Collections.Generic;

public interface IObjectPool
{
	void Return(IPoolableObject obj, string listId);
}

/// <summary>
/// Класс для создания объектов заранее и хранении их в пуле для 
/// сокращения времени на создание новых экземпляров из префабов.
/// </summary>
public class ObjectPool : IObjectPool
{
	private Dictionary<string, LinkedList<IPoolableObject>> pools = new Dictionary<string, LinkedList<IPoolableObject>>();

	public T Get<T>(T prefab, string listId) where T : PoolableObject
	{
		var list = default(LinkedList<IPoolableObject>);
		if (!pools.TryGetValue(listId, out list))
		{
			list = pools[listId] = new LinkedList<IPoolableObject>();
		}

		if (list.Count < 1)
		{
			for (int i = 0; i < prefab.PoolCapacity; i++)
			{
				var obj = UnityEngine.Object.Instantiate(prefab) as IPoolableObject;
				obj.OnCreate(this, listId);
				ReturnObject(obj, listId);
			}
		}

		var result = list.First.Value;

		list.RemoveFirst();
		result.OnGet();

		return result as T;
	}

	void IObjectPool.Return(IPoolableObject obj, string listId)
	{
		ReturnObject(obj, listId);
	}

	private void ReturnObject(IPoolableObject obj, string listId)
	{
		obj.OnReturn();

		var list = pools[listId];
		list.AddLast(obj);
	}
}
