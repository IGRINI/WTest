using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Pool
{
    public class PrefabPool<TPoolable> where TPoolable : MonoBehaviour, IPoolable
    {
        private Stack<TPoolable> _itemsInPool;
        public IEnumerable<TPoolable> ItemsInPool => _itemsInPool;

        private Transform _objectsParent;

        private GameObject _prefab;

        public PrefabPool(GameObject prefab, int initialCapacity = 0, Transform parent = null)
        {
            _prefab = prefab;
            _itemsInPool = new Stack<TPoolable>(initialCapacity);
            
            if (parent != null)
            {
                _objectsParent = parent;
            }
            else
            {
                _objectsParent = new GameObject(typeof(PrefabPool<TPoolable>).Name).transform;
            }
            
            for (var i = 0; i < initialCapacity; i++)
            {
                ExpandPool();
            }
        }
        
        public TPoolable Spawn()
        {
            if (_itemsInPool.Count == 0)
            {
                ExpandPool();
            }
            
            TPoolable poolable = _itemsInPool.Pop();;
            
            poolable.transform.SetAsLastSibling();
                
            poolable.OnSpawn();
		
            return poolable;
        }

        public void Despawn(TPoolable poolable)
        {
            poolable.OnDespawn();
            _itemsInPool.Push(poolable);
        }

        private void ExpandPool()
        {
            TPoolable element = Object.Instantiate(_prefab, _objectsParent)
                                    .GetComponent<TPoolable>();
            _itemsInPool.Push(element);
        }

        public override string ToString()
        {
            return $"{typeof(PrefabPool<TPoolable>).Name}<{typeof(TPoolable).Name}>({_itemsInPool.Count})";
        }
    }
}