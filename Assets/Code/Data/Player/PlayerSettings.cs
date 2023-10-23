using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    internal sealed class PlayerSettings
    {
        #region Fields

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

        public Material MaterialTrailRenderer => _materialTrailRenderer;
        public Sprite SpritePlayer => _spritePlayer;
        public GameObject ParticleSystem => _particleSystem;
        public Color StartColor => _startColor;
        public Color EndColor => _endColor;
        public Vector2 OffsetVectorTrailrenderer => _offsetVectorTrailrenderer;
        public Vector2 OffsetVectorBurel => _offsetVectorBullet;
        public float StartWidth => _startWidth;
        public float EndWidth => _endWidth;
        public float Time => _time;

        #endregion
    }
}