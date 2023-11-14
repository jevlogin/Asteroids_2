using System;
using System.Collections.Generic;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class PlayerShooterController : IExecute, ICleanup
    {
        private IUserInputBool _userInputMouse;
        private PlayerInitialization _playerInitialization;
        private AmmunitionModel _ammunitionFactoryModel;
        private readonly Transform _barrelTransform;
        private readonly SceneController _sceneController;
        private float _refireTimer;
        private float _fireTimer;
        public float MoveSpeed = 10.0f;
        private List<Bullet> _listBullets;
        private Dictionary<int, Rigidbody2D> _rigidbodyBullets;
        private bool _isStopControl;
        private bool _isCanShoot;

        public event Action<Vector3> IsCollisionBullet;

        public event Action<bool> IsShotInvoke;

        public PlayerShooterController(PlayerInitialization playerInitialization, AmmunitionModel ammunitionFactoryModel, SceneController sceneController)
        {
            _playerInitialization = playerInitialization;
            _ammunitionFactoryModel = ammunitionFactoryModel;

            _barrelTransform = _playerInitialization.PlayerModel.Components.BarrelTransform;
            _refireTimer = ammunitionFactoryModel.AmmunitionStruct.RefireTimer;
            _fireTimer = 0.0f;
            _listBullets = new List<Bullet>();
            _rigidbodyBullets = new Dictionary<int, Rigidbody2D>();

            _sceneController = sceneController;
            _sceneController.IsStopControl += OnChangeIsStopControl;
            _playerInitialization.PlayerModel.PlayerStruct.Player.IsCanShoot += OnChangeCanShoot;
        }

        private void OnChangeCanShoot(bool value)
        {
            _isCanShoot = value;
        }

        private void OnChangeIsStopControl(bool value)
        {
            _isStopControl = value;
        }

        public void Execute(float deltatime)
        {
            if (_isStopControl)
            {
                return;
            }

            _fireTimer += deltatime;
            if (_isCanShoot)
            {
                BulletShoot();
            }
            BulletControl(deltatime);
        }

        private void BulletControl(float deltatime)
        {
            for (int i = _listBullets.Count - 1; i >= 0; i--)
            {
                if (_listBullets[i].isActiveAndEnabled)
                {
                    MoveConcreteBullet(deltatime, _listBullets[i]);

                    _listBullets[i].LifeTime += deltatime;
                    if (_listBullets[i].LifeTime >= _listBullets[i].MaxLifeTimeOutsideThePool)
                    {
                        _listBullets[i].LifeTime = 0.0f;
                        _ammunitionFactoryModel.AmmunitionStruct.PoolBulletGeneric.ReturnToPool(_listBullets[i]);
                        _listBullets.RemoveAt(i);
                    }
                }
            }
        }

        private void MoveConcreteBullet(float deltatime, Bullet bullet)
        {
            if (_rigidbodyBullets.TryGetValue(bullet.GetInstanceID(), out var rigidbody))
            {
                if (rigidbody.velocity.magnitude <= 0.1f)
                {
                    rigidbody.velocity = rigidbody.transform.up * MoveSpeed;
                }
            }
            else
            {
                bullet.transform.Translate(Vector2.up * MoveSpeed * deltatime);
            }
        }

        private void BulletShoot()
        {
            if (_fireTimer >= _refireTimer)
            {
                IsShotInvoke?.Invoke(true);

                _fireTimer = 0;
                var bullet = GetBullet();
                bullet.OnCollisionEnterDetect += Bullet_OnCollisionEnterDetect;
                _listBullets.Add(bullet);
            }
        }

        private void Bullet_OnCollisionEnterDetect(Collider2D collider)
        {
            IsCollisionBullet?.Invoke(collider.transform.position);
        }

        private Bullet GetBullet()
        {
            var bullet = _ammunitionFactoryModel.AmmunitionStruct.PoolBulletGeneric.Get();
            bullet.transform.SetParent(null);
            bullet.transform.localPosition = _barrelTransform.position;
            bullet.transform.rotation = _barrelTransform.rotation;
            bullet.gameObject.SetActive(true);
            _rigidbodyBullets[bullet.GetInstanceID()] = bullet.gameObject.GetOrAddComponent<Rigidbody2D>();
            return bullet;
        }

        public void Cleanup()
        {
            _sceneController.IsStopControl -= OnChangeIsStopControl;
        }
    }
}