using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal class AmmunitionFactory
    {
        private readonly AmmunitionData _ammunitionData;
        private AmmunitionModel _ammunitionModel;

        public AmmunitionFactory(AmmunitionData ammunitionData)
        {
            _ammunitionData = ammunitionData;
        }

        internal AmmunitionModel CreateAmmunitionModel()
        {
            if (_ammunitionModel == null)
            {
                var ammunitionStruct = _ammunitionData.AmmunitionStruct;
                var components = new AmmunitionComponents();
                var settings = new AmmunitionSettings();


                ammunitionStruct.Bullet = Object.Instantiate(_ammunitionData.AmmunitionSettings.BulletPrefab);
                components.BulletView = ammunitionStruct.Bullet;

                ammunitionStruct.Bullet.name = _ammunitionData.AmmunitionSettings.NameBullet;

                ammunitionStruct.PoolBullet = new Pool<Bullet>(ammunitionStruct.Bullet, ammunitionStruct.PoolSize);

                var ammunitionTransformPoolParent = new GameObject(ManagerName.POOL_AMMUNITION);
                ammunitionStruct.PoolBulletGeneric = new BulletPool(ammunitionStruct.PoolBullet, ammunitionTransformPoolParent.transform);
                ammunitionStruct.PoolBulletGeneric.AddObjects(ammunitionStruct.Bullet);

                components.BulletView = ammunitionStruct.Bullet;
                components.Collider2D = components.BulletView.GetComponent<Collider2D>();
                components.Rigidbody2D = components.BulletView.GetComponent<Rigidbody2D>();

                _ammunitionModel = new AmmunitionModel(ammunitionStruct, components, settings);
            }

            return _ammunitionModel;
        }
    }
}