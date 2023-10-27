using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal sealed class PlayerSettings
    {
        #region Fields

        [SerializeField] private Speed _speed;
        [SerializeField] private Health _health;
        [SerializeField, Range(0, 1000)] private int _force;
        [SerializeField, Range(0, 100)] private float _damage;

        [SerializeField, Header("Спрайт для корабля"), Space(5)] private Sprite _spritePlayer;
        [SerializeField, Header("Система частиц для корабля"), Space(20)] private GameObject _particleSystem;
        [SerializeField, Space(10), Header("Настройки для TrailRenderer"), Space(20)] private Material _materialTrailRenderer;

        [SerializeField,] private Color _startColor = Color.red;
        [SerializeField] private Color _endColor = Color.blue;

        [SerializeField, Range(0, 1)] private float _startWidth;
        [SerializeField, Range(0, 1)] private float _endWidth;
        [SerializeField, Range(0, 1)] private float _time;
        [SerializeField, Header("Вектор смещения для Трайл рендера")] private Vector2 _offsetVectorTrailrenderer;

        [SerializeField, Space(10), Header("Вектор смещения для ствола пушки"), Space(20)] private Vector2 _offsetVectorBullet;

        #endregion


        #region Properties

        internal float Damage => _damage;
        internal int Force => _force;
        internal Speed Speed => _speed;
        internal Health Health => _health;
        internal Material MaterialTrailRenderer => _materialTrailRenderer;
        internal Sprite SpritePlayer => _spritePlayer;
        internal GameObject ParticleSystem => _particleSystem;
        internal Color StartColor => _startColor;
        internal Color EndColor => _endColor;
        internal Vector2 OffsetVectorTrailrenderer => _offsetVectorTrailrenderer;
        internal Vector2 OffsetVectorBurel => _offsetVectorBullet;
        internal float StartWidth => _startWidth;
        internal float EndWidth => _endWidth;
        internal float Time => _time;

        #endregion
    }
}