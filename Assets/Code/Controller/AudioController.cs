using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WORLDGAMEDEVELOPMENT
{
    internal class AudioController : IController, ICleanup, IFixedExecute
    {
        private PlayerModel _playerModel;
        private PlayerShooterController _playerShooterController;
        private readonly EnemyController _enemyController;
        private AudioModel _audioModel;
        private AudioClip _shotAudioClip;
        private readonly AudioClip _explosionAudioClip;
        private List<AudioSourceInfo> audioSourcesInUse = new List<AudioSourceInfo>();


        public AudioController(AudioModel audioModel, PlayerModel playerModel, PlayerShooterController playerShooterController, EnemyController enemyController)
        {
            _audioModel = audioModel;
            _playerModel = playerModel;
            _playerShooterController = playerShooterController;
            _enemyController = enemyController;

            _playerModel.Components.AudioSource.outputAudioMixerGroup = _audioModel.AudioStruct.AudioMixer.FindMatchingGroups(AudioMixerGroupName.EFFECTS)[0];

            _shotAudioClip = _audioModel.AudioStruct.ClipByTypes
                .FirstOrDefault(clip => clip._type == AudioClipType.Shot)?
                .AudioClips.FirstOrDefault();
            _explosionAudioClip = _audioModel.AudioStruct.ClipByTypes
                .FirstOrDefault(clip => clip._type == AudioClipType.Explosion)?
                .AudioClips.FirstOrDefault();

            _playerShooterController.IsShotInvoke += IsShotInvoke;
            _enemyController.IsAsteroidExplosion += ExplosionEnemy;
        }

        private void ExplosionEnemy(Vector3 vector)
        {
            var source = _audioModel.AudioStruct.AudioSourcePoolEffects.Get();
            source.transform.position = vector;
            source.clip = _explosionAudioClip;
            source.gameObject.SetActive(true);
            source.pitch = Random.Range(0.8f, 1.2f);
            source.Play();

            audioSourcesInUse.Add(new AudioSourceInfo(source, source.clip.length));
        }

        public void Cleanup()
        {
            _playerShooterController.IsShotInvoke -= IsShotInvoke;
            _enemyController.IsAsteroidExplosion -= ExplosionEnemy;
        }

        private void IsShotInvoke(bool value)
        {
            if (value)
            {
                _playerModel.Components.AudioSource.PlayOneShot(_shotAudioClip);
            }
        }

        public void FixedExecute(float fixedDeltatime)
        {
            for (int i = 0; i < audioSourcesInUse.Count; i++)
            {
                audioSourcesInUse[i].Delay -= fixedDeltatime;
                if (audioSourcesInUse[i].Delay < 0)
                {
                    _audioModel.AudioStruct.AudioSourcePoolEffects.ReturnToPool(audioSourcesInUse[i].Source);
                    audioSourcesInUse.RemoveAt(i);
                    i--;
                }
            }
            
        }
    }
}