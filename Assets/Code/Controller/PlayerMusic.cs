using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    public sealed class PlayerMusic
    {
        internal AudioSourceInfo _audioSource;
        private List<AudioClip> _audioClips;
        private int _indexClip = 0;
        private bool _isPaused;
        private bool _isContinuedPlay;

        public PlayerMusic(AudioSourceInfo audioSource, List<AudioClip> audioClips)
        {
            _audioSource = audioSource ?? new AudioSourceInfo(new GameObject(ManagerName.AUDIOSOURCE).GetOrAddComponent<AudioSource>(), 0.0f);
            _audioClips = audioClips ?? new List<AudioClip>();
            if (!_audioSource.Source.gameObject.activeSelf)
                _audioSource.Source.gameObject.SetActive(true);
            if (_audioSource.Source.clip == null)
                _audioSource.Source.clip = _audioClips.First();
        }

        internal void PauseOrPlay()
        {
            if (_audioSource.Source.isPlaying)
            {
                _audioSource.Source.Pause();
                _isPaused = true;
                Debug.Log($"Звук играет. Поставил звук на паузу.");
            }
            else
            {
                Debug.Log($"Звук не играет.");

                if (_isPaused)
                {
                    _audioSource.Source.UnPause();
                    Debug.Log($"Стояла пауза. Снял с паузы.");
                }
                else
                {
                    Debug.Log($"Паузы не было. Воспроизвел звук текущий. если нет клипа, меняю.");
                    _audioSource.Source.Play();
                }
                _isPaused = false;
            }
        }

        internal void Next()
        {
            _isContinuedPlay = _audioSource.Source.isPlaying;
            _audioSource.Source.Stop();
            _indexClip++;
            _indexClip %= _audioClips.Count;
            _audioSource.Source.clip = _audioClips[_indexClip];
            _audioSource.Delay = _audioSource.Source.clip.length;
            _isPaused = false;

            if (_isContinuedPlay)
            {
                PauseOrPlay();
            }
        }

        internal void Prev()
        {
            _isContinuedPlay = _audioSource.Source.isPlaying;
            _audioSource.Source.Stop();
            _indexClip--;
            if (_indexClip < 0)
                _indexClip = _audioClips.Count - 1;
            else
                _indexClip %= _audioClips.Count;

            _audioSource.Source.clip = _audioClips[_indexClip];
            _audioSource.Delay = _audioSource.Source.clip.length;
            _isPaused = false;
            if (_isContinuedPlay)
            {
                PauseOrPlay();
            }
        }

        internal void TimeLeft(float deltaTime)
        {
            if (_audioSource.Source.isPlaying && !_isPaused)
            {
                _audioSource.Delay -= deltaTime;
                if (_audioSource.Delay <= 0)
                {
                    Next();
                }
            }
        }
    }
}