using System.Collections.Generic;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal abstract class GenericObjectPool<T> where T : Component
    {
        #region Fields

        private Queue<T> _objects = new Queue<T>();
        private Pool<T> _pool;
        private Transform _transformParent;
        private Transform _transformPool;

        #endregion


        #region Properties

        internal int PoolSize => _objects.Count;
        public Pool<T> Pool { get => _pool; protected set => _pool = value; }

        #endregion

        #region ClassLifeCycles

        internal GenericObjectPool(Pool<T> pool, Transform transformParent)
        {
            Pool = pool;
            _transformParent = transformParent;

            if (_transformParent == null && pool != null)
            {
                _transformParent = new GameObject(nameof(Pool.Prefab)).transform;
            }
            else
            {
                return;
            }
        }

        #endregion


        #region Methods

        public T Get()
        {
            if (_objects.Count == 0)
            {
                AddObjects(Pool.Size);
            }
            return _objects.Dequeue();
        }

        public List<T> GetList()
        {
            List<T> result = new List<T>();

            if (_objects.Count == 0)
            {
                AddObjects(Pool.Size);
            }
            foreach (var item in _objects)
            {
                result.Add(item);
            }

            return result;
        }

        public void AddObjects(T obj)
        {
            if (_objects.Count == 0)
            {
                AddObjects(Pool.Size);
            }
            ReturnToPool(obj);
        }

        protected void ExpandPool(Pool<T> pool, T expandObject)
        {
            Pool = pool;

            AddObjects(Pool.Size);
            ReturnToPool(expandObject);
        }

        private void AddObjects(int count)
        {
            string name = Pool.Prefab.name;

            SetParentTransformPool();

            for (int i = 0; i < count; i++)
            {
                var newObject = Object.Instantiate(Pool.Prefab);
                newObject.gameObject.name = name;

                newObject.transform.SetParent(_transformPool);
                newObject.gameObject.SetActive(false);

                _objects.Enqueue(newObject);
            }
        }

        private void SetParentTransformPool()
        {
            if (_transformPool == null)
            {
                switch (Pool.Prefab.GetType().Name)
                {
                    case ManagerName.BULLET:
                        _transformPool = new GameObject(ManagerName.POOL_BULLET).transform;
                        break;
                    case ManagerName.ASTEROID:
                        _transformPool = new GameObject(ManagerName.POOL_ASTEROID).transform;
                        break;
                    default:
                        throw new System.ArgumentException("Нет такого типа", nameof(T));
                }
                _transformPool.SetParent(_transformParent);
            }
        }

        public void ReturnToPool(T objectToReturn)
        {
            objectToReturn.gameObject.SetActive(false);
            objectToReturn.transform.position = Vector3.zero;
            objectToReturn.transform.rotation = Quaternion.identity;
            objectToReturn.transform.SetParent(_transformPool);
            _objects.Enqueue(objectToReturn);
        }

        #endregion
    }
}