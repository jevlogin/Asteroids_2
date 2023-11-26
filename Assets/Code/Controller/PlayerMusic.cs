using System;
using System.Collections.Generic;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    public sealed class PlayerMusic
    {
        internal AudioSourceInfo _audioSource;
        private List<AudioClip> _audioClips;
        private int _currentClipIndex = 0;
        private bool _isPlayNextTrack;

        public PlayerMusic(AudioSourceInfo audioSource, List<AudioClip> audioClips)
        {
            _audioSource = audioSource ?? new AudioSourceInfo(new GameObject(ManagerName.AUDIOSOURCE).GetOrAddComponent<AudioSource>(), 0.0f);
            _audioClips = audioClips ?? new List<AudioClip>();

            if (!_audioSource.Source.gameObject.activeSelf)
                _audioSource.Source.gameObject.SetActive(true);

            if (_audioSource.Source.clip == null)
            {
                if (_audioClips.Count > 0)
                    _audioSource.Source.clip = _audioClips[0];
                else
                    throw new ArgumentNullException(nameof(_audioSource.Source.clip), "Отсутствует клип");
            }
        }

        public void Play()
        {
            if (_audioSource.Source.isPlaying)
            {
                return;
            }
            if (_audioSource.IsPaused)
            {
                _audioSource.Source.time = _audioSource.CurrentPlaybackPosition;
                _audioSource.Source.UnPause();
            }
            else
            {
                _audioSource.Delay = _audioSource.Source.clip.length;
                _audioSource.Source.Play();
            }
            _audioSource.IsPaused = false;
        }

        public void Pause()
        {
            if (_audioSource.Source.isPlaying)
            {
                _audioSource.IsPaused = true;
                _audioSource.CurrentPlaybackPosition = _audioSource.Source.time;

                _audioSource.Source.Pause();
            }
        }

        public void NextTrack()
        {
            _currentClipIndex = (_currentClipIndex + 1) % _audioClips.Count;
            PlayCurrentTrack();
        }

        public void PreviousTrack()
        {
            _currentClipIndex = (_currentClipIndex - 1 + _audioClips.Count) % _audioClips.Count;
            PlayCurrentTrack();
        }

        private void PlayCurrentTrack()
        {
            _audioSource.Source.Stop();
            _audioSource.Source.clip = _audioClips[_currentClipIndex];
            Play();
        }


        internal void TimeLeft(float deltaTime)
        {
            if (_audioSource.Source.isPlaying)
            {
                _audioSource.Delay -= deltaTime;
                return;
            }
            if (_audioSource.Delay <= 0)
            {
                NextTrack();
            }
        }
    }
}