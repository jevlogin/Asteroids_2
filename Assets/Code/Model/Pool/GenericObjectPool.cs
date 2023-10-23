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

        internal Pool<T> Pool { get => _pool; private set => _pool = value; } 

        #endregion

        #region ClassLifeCycles

        internal GenericObjectPool(Pool<T> pool, Transform transformParent)
        {
            Pool = pool;
            _transformParent = transformParent;

            if (_transformParent == null)
            {
                _transformParent = new GameObject(nameof(Pool.Prefab)).transform;
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

        private void AddObjects(int count)
        {
            string name = "ObjectPool";

            switch (Pool.Prefab.GetType().Name)
            {
                case ManagerName.BULLET:
                    _transformPool = new GameObject(ManagerName.POOL_BULLET).transform;
                    name = ManagerName.BULLET;
                    break;
                case ManagerName.ASTEROID:
                    _transformPool = new GameObject(ManagerName.POOL_ASTEROID).transform;
                    name = ManagerName.ASTEROID;
                    break;
                default:
                    throw new System.ArgumentException("Нет такого типа", nameof(T));
            }

            _transformPool.SetParent(_transformParent);

            for (int i = 0; i < count; i++)
            {
                var newObject = Object.Instantiate(Pool.Prefab);
                newObject.gameObject.name = name;

                newObject.transform.SetParent(_transformPool);
                newObject.gameObject.SetActive(false);

                _objects.Enqueue(newObject);
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