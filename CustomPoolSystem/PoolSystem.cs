using System;
using UnityEngine;

namespace CustomPoolSystem
{
	public abstract class PoolSystem : MonoBehaviour
	{
		public abstract GameObject Spawn(GameObject prefab, Transform parent);
		public abstract GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation);
		public abstract GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent);
		public abstract void Despawn(GameObject instance);

		// take ==> take from pool (spawn); release => return to pool (despawn)
		public abstract void CreatePool(GameObject prefab, Transform parent, int preheatSize = 1, Action<GameObject> onTaken = null, Action<GameObject> onReleased = null);
		public abstract void ClearPool(GameObject prefab);

		// Plus lots of other use methods :]
	}
}