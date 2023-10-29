using System.Collections.Generic;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal abstract class GenericObjectPool<T> where T : Component
    {
        #region Fields

        private Queue<T> _objects = new Queue<T>();
        [SerializeField] private Pool<T> _poolPrefab;
        private Transform _transformParent;
        private Transform _transformPool;
        internal event System.Action<List<T>, T> OnUpdatePoolAfterAddedNewPoolObjects;

        #endregion


        #region Properties

        internal int PoolSize => _objects.Count;
        public Pool<T> PoolPrefab { get => _poolPrefab; protected set => _poolPrefab = value; }
        internal Transform TransformParent => _transformParent;

        #endregion

        #region ClassLifeCycles

        internal GenericObjectPool(Pool<T> pool, Transform transformParent)
        {
            PoolPrefab = pool;
            _transformParent = transformParent;

            if (_transformParent == null && pool != null)
            {
                _transformParent = new GameObject(nameof(PoolPrefab.Prefab)).transform;
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
                AddObjects(PoolPrefab.Size);
            }
            return _objects.Dequeue();
        }

        public List<T> GetList()
        {
            List<T> result = new List<T>();

            if (_objects.Count == 0)
            {
                AddObjects(PoolPrefab.Size);
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
                AddObjects(PoolPrefab.Size);
            }
            ReturnToPool(obj);
        }

        private void AddObjects(int count)
        {
            string name = PoolPrefab.Prefab.name;

            SetParentTransformPool();

            for (int i = 0; i < count; i++)
            {
                var newObject = Object.Instantiate(PoolPrefab.Prefab);
                newObject.gameObject.name = name;

                newObject.transform.SetParent(_transformPool);
                newObject.gameObject.SetActive(false);

                _objects.Enqueue(newObject);
            }

            OnUpdatePoolAfterAddedNewPoolObjects?.Invoke(GetList(), PoolPrefab.Prefab);
        }

        private void SetParentTransformPool()
        {
            if (_transformPool == null)
            {
                switch (PoolPrefab.Prefab.GetType().Name)
                {
                    case ManagerName.BULLET:
                        _transformPool = new GameObject(ManagerName.POOL_BULLET).transform;
                        break;
                    case ManagerName.ASTEROID:
                        _transformPool = new GameObject($"[Pool_{PoolPrefab.Prefab.name}]").transform;
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