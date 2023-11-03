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
            }
            else
            {
                if (_isPaused)
                {
                    _audioSource.Source.UnPause();
                    _isPaused = false;
                }

                if (!_audioSource.Source.isPlaying)
                {
                    _audioSource.Source.Play();
                }
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

            if(_isContinuedPlay)
            {
                PauseOrPlay();
            }
        }

        internal void Prev()
        {
            _isContinuedPlay = _audioSource.Source.isPlaying;
            _audioSource.Source.Stop();
            _indexClip--;
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
            if (_audioSource.Source.isPlaying)
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