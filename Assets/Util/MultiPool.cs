using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util
{
	[System.Serializable]
	public class PoolItem
	{
		public GameObject Object;
		public int InitialAmount;
	}

	public class MultiPool : MonoBehaviour
	{
		public string Name;
		public bool Expand = true;
		public bool Shrink = true;
		public int ShrinkTreshold = 5;
		public float CleanupPeriod = 2;
		public List<PoolItem> ItemsToPool;
		public Dictionary<string, List<GameObject>> ObjectPools;

		private void OnEnable()
		{
			InfoPool.Provide(Name, GetMultiPool);
		}

		private void OnDisable()
		{
			InfoPool.Unprovide(Name, GetMultiPool);
		}

		private MultiPool GetMultiPool()
		{
			return this;
		}

		private void Start()
		{
			if (ObjectPools == null) InitializePools();
		}
		
		private void InitializePools() {
			ObjectPools = new Dictionary<string, List<GameObject>>();
			foreach (PoolItem item in ItemsToPool)
			{
				List<GameObject> pool = new List<GameObject>();
				for (int i = 0; i < item.InitialAmount; i++) {
					GameObject obj = Instantiate(item.Object);
					obj.SetActive(false);
					pool.Add(obj);
				}
				ObjectPools.Add(item.Object.name, pool);
			}
			if(Shrink) StartCoroutine(CleanUp());
		}

		public GameObject GetObject()
		{
			PoolItem item = RandomUtil.RandomElement(ItemsToPool);
			if (ObjectPools == null) InitializePools();
			List<GameObject> pool = ObjectPools[item.Object.name];
			foreach (GameObject gameObjectToGet in pool)
			{
				if (!gameObjectToGet.activeInHierarchy)
				{
					return gameObjectToGet;
				}
			}
			if (!Expand) return null;
			GameObject obj = Instantiate(item.Object);
			obj.SetActive(false);
			pool.Add(obj);
			return obj;
		}
		
		public GameObject GetObject(string nameToGet) {
			if (ObjectPools == null) InitializePools();
			foreach (GameObject gameObjectToGet in ObjectPools[nameToGet])
			{
				if (!gameObjectToGet.activeInHierarchy)
				{
					return gameObjectToGet;
				}
			}
			if (!Expand) return null;
			foreach (PoolItem item in ItemsToPool)
			{
				if (item.Object.gameObject.name != nameToGet) continue;
				GameObject obj = Instantiate(item.Object);
				obj.SetActive(false);
				ObjectPools[nameToGet].Add(obj);
				return obj;
			}
			print("Lose ime pulovanog objekta!");
			return null;
		}

		private IEnumerator CleanUp()
		{
			while (true)
			{
				yield return new WaitForSeconds(CleanupPeriod);
				foreach (List<GameObject> pool in ObjectPools.Values)
				{
					int positionInList = 0;
					int inactiveNumber = 0;
					while (pool.Count > positionInList)
					{
						GameObject gObject = pool[positionInList];
						if (!gObject.activeInHierarchy)
						{
							if (inactiveNumber == ShrinkTreshold)
							{
								pool.Remove(gObject);
								Destroy(gObject);
							}
							else
							{
								positionInList++;
								inactiveNumber++;
							}
						}
						else
						{
							positionInList++;
						}
					}
				}
			}
		}
	}
}