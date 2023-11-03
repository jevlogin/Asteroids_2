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
        private readonly CanvasController _canvasController;
        private AudioModel _audioModel;
        private AudioClip _shotAudioClip;
        private readonly AudioClip _explosionAudioClip;
        private readonly List<AudioClip> _soundsListStartShip;
        private List<AudioSourceInfo> audioSourcesInUse = new List<AudioSourceInfo>();


        public AudioController(AudioModel audioModel, PlayerModel playerModel, PlayerShooterController playerShooterController, EnemyController enemyController, CanvasController canvasController)
        {
            _audioModel = audioModel;
            _playerModel = playerModel;
            _playerShooterController = playerShooterController;
            _enemyController = enemyController;
            _canvasController = canvasController;


            _playerModel.Components.AudioSource.outputAudioMixerGroup = _audioModel.AudioStruct.AudioMixer.FindMatchingGroups(AudioMixerGroupName.EFFECTS)[0];

            _shotAudioClip = _audioModel.AudioStruct.ClipByTypes
                .FirstOrDefault(clip => clip._type == AudioClipType.Shot)?
                .AudioClips.FirstOrDefault();
            _explosionAudioClip = _audioModel.AudioStruct.ClipByTypes
                .FirstOrDefault(clip => clip._type == AudioClipType.Explosion)?
                .AudioClips.FirstOrDefault();

            _soundsListStartShip = _audioModel.AudioStruct.ClipByTypes[4].AudioClips;

            _canvasController.StartGame += StartGame;
            _playerShooterController.IsShotInvoke += IsShotInvoke;
            _enemyController.IsAsteroidExplosion += ExplosionEnemy;
        }

        private void StartGame(EventCanvas eventType)
        {
            switch (eventType)
            {
                case EventCanvas.None:
                    break;
                case EventCanvas.StartGame:
                    Debug.Log($"Старт игры. взять из пулла источник звука. Проиграть звук. Старта");
                    
                    var source = _audioModel.AudioStruct.AudioSourcePoolEffects.Get();
                    source.clip = _soundsListStartShip[1];
                    source.transform.localPosition = _playerModel.Components.PlayerTransform.localPosition;
                    source.gameObject.SetActive(true);
                    source.Play();

                    audioSourcesInUse.Add(new AudioSourceInfo(source, source.clip.length));

                    break;
                case EventCanvas.StartShip:
                    Debug.Log($"Взлет корабля. взять из пулла источник звука. Проиграть звук 2.");

                    var source2 = _audioModel.AudioStruct.AudioSourcePoolEffects.Get();
                    source2.clip = _soundsListStartShip[3];
                    source2.transform.localPosition = _playerModel.Components.PlayerTransform.localPosition;
                    source2.gameObject.SetActive(true);
                    source2.Play();

                    audioSourcesInUse.Add(new AudioSourceInfo(source2, source2.clip.length));

                    break;
            }
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
            _canvasController.StartGame -= StartGame;
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
            //TODO - сделать два пробега, в одном удаляем и возвращаем в пулл, во втором вычитаем время

            for (int i = 0; i < audioSourcesInUse.Count; i++)
            {
                audioSourcesInUse[i].Delay -= fixedDeltatime;
                if (audioSourcesInUse[i].Delay < 0)
                {
                    _audioModel.AudioStruct.AudioSourcePoolEffects.ReturnToPool(audioSourcesInUse[i].Source);
                    audioSourcesInUse.Remove(audioSourcesInUse[i]);
                    i--;
                }
            }
            
        }
    }
}