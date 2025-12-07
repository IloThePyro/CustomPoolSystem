using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace CustomPoolSystem
{
	public class UnityPool
	{
		public ObjectPool<GameObject> Pool
		{
			get; protected set;
		}

		private GameObject _prefab;
		private Transform _parent;

		public UnityPool(GameObject prefab, Transform parent = null, int preheatSize = 1, Action<GameObject> onTaken = null, Action<GameObject> onReleased = null)
		{
			_prefab = prefab;
			_parent = parent;

			//UnityPool works like that:
			// If max capacity is reached, it will still spawn!
			// But returned objects will be destroyed if the capacity is over max
			Pool = new ObjectPool<GameObject>
			(
				createFunc: CreateObject,
				defaultCapacity: preheatSize,
				actionOnGet: onTaken ?? OnObjectTaken, // ?? takes first argument if not null
				actionOnRelease: onReleased ?? OnObjectReturned,
				maxSize: 20,
				actionOnDestroy: (obj) => UnityEngine.Object.Destroy(obj)
			);

			Preheat(preheatSize);
		}

		private void Preheat(int size)
		{
			List<GameObject> objects = new List<GameObject>();

			for (int i = 0;i < size;i++)
			{
				objects.Add(Pool.Get());
			}

			for (int i = 0;i < size;i++)
			{
				Pool.Release(objects[i]);
			}
		}

		private GameObject CreateObject()
		{
			GameObject obj = !_parent ? UnityEngine.Object.Instantiate(_prefab) : UnityEngine.Object.Instantiate(_prefab, _parent);
			obj.SetActive(false);
			return obj;
		}

		private void OnObjectTaken(GameObject obj)
		{
			obj.SetActive(true); // Watch out, this calls OneEnable immediately
		}

		private void OnObjectReturned(GameObject obj)
		{
			obj.SetActive(false);
			obj.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
		}
	}
}