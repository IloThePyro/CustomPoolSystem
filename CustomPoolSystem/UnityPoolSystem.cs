using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomPoolSystem
{
	public class UnityPoolSystem : PoolSystem
	{
		// Key: Prefab; Value: Pool
		private Dictionary<GameObject, UnityPool> _pools = new Dictionary<GameObject, UnityPool>();

		// Key: Spawned Instance; Value: Its pool
		private Dictionary<GameObject, UnityPool> _instanceToPool = new Dictionary<GameObject, UnityPool>();

		public override void CreatePool(GameObject prefab, Transform parent, int preheatSize = 1, Action<GameObject> onTaken = null, Action<GameObject> onReleased = null)
		{
			if (!_pools.ContainsKey(prefab))
			{
				_pools.Add(prefab, new UnityPool(prefab, parent, preheatSize, onTaken, onReleased));
			}
			else
			{
				// We could do nothing or we could also increase the size of the pool, if we want to (e.g. there might be a reason why another script tried to create the same pool)
			}
		}

		public override GameObject Spawn(GameObject prefab, Transform parent)
		{
			if (!_pools.ContainsKey(prefab))
			{
				// Philosophy: Do we FORCE a pool if none has been created? Let's say yes
				CreatePool(prefab, parent, 1);
			}

			// We could also check the size of all objects in the pool and NOT spawn if we think max is reached, or delete older objects. Because UnityPool does not do that

			GameObject instance = _pools[prefab].Pool.Get(); // OnObjectTaken is called right after!
															 //instance.transform.SetParent(parent);
			_instanceToPool.Add(instance, _pools[prefab]);

			return instance;
		}

		public override GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
		{
			if (!_pools.ContainsKey(prefab))
			{
				// Philosophy: Do we FORCE a pool if none has been created? Let's say yes
				CreatePool(prefab, null, 1);
			}

			// We could also check the size of all objects in the pool and NOT spawn if we think max is reached, or delete older objects. Because UnityPool does not do that

			GameObject instance = _pools[prefab].Pool.Get(); // OnObjectTaken is called right after!
			instance.transform.SetPositionAndRotation(position, rotation);
			_instanceToPool.Add(instance, _pools[prefab]);

			return instance;
		}

		public override GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
		{
			if (!_pools.ContainsKey(prefab))
			{
				// Philosophy: Do we FORCE a pool if none has been created? Let's say yes
				CreatePool(prefab, parent, 1);
			}

			// We could also check the size of all objects in the pool and NOT spawn if we think max is reached, or delete older objects. Because UnityPool does not do that

			GameObject instance = _pools[prefab].Pool.Get(); // OnObjectTaken is called right after!
			instance.transform.SetPositionAndRotation(position, rotation);
			instance.transform.SetParent(parent);
			_instanceToPool.Add(instance, _pools[prefab]);

			return instance;
		}

		public override void Despawn(GameObject instance)
		{
			if (_instanceToPool.ContainsKey(instance) && _instanceToPool[instance] != null)
			{
				_instanceToPool[instance].Pool.Release(instance);
				_instanceToPool.Remove(instance);
			}
			else
			{
				Destroy(instance);
			}
		}

		public override void ClearPool(GameObject prefab)
		{
			if (_pools.ContainsKey(prefab) && _pools[prefab] != null)
			{
				_pools[prefab].Pool.Clear();
			}
		}
	}
}