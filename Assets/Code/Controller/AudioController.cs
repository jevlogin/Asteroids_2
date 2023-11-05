using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;
using Random = UnityEngine.Random;

namespace WORLDGAMEDEVELOPMENT
{
    internal class AudioController : IController, ICleanup, IFixedExecute, IInitialization
    {
        private PlayerModel _playerModel;
        private PlayerShooterController _playerShooterController;
        private readonly EnemyController _enemyController;
        private readonly CanvasController _canvasController;
        private readonly CanvasModel _canvasModel;
        private AudioModel _audioModel;
        private AudioClip _shotAudioClip;
        private readonly AudioClip _explosionAudioClip;
        private readonly List<AudioClip> _soundsListStartShip;
        private readonly List<AudioClip> _musicList;

        private readonly PanelResultsView _panelResults;


        private List<AudioSourceInfo> audioSourcesInUse = new List<AudioSourceInfo>();
        private PlayerMusic _playerMusic;
        private List<Button> _listButtons = new();

        public AudioController(AudioModel audioModel, PlayerModel playerModel,
            PlayerShooterController playerShooterController, EnemyController enemyController,
            CanvasController canvasController, CanvasModel canvasModel)
        {
            _audioModel = audioModel;
            _playerModel = playerModel;
            _playerShooterController = playerShooterController;
            _enemyController = enemyController;
            _canvasController = canvasController;
            _canvasModel = canvasModel;

            _playerModel.Components.AudioSource.outputAudioMixerGroup = _audioModel.AudioStruct.AudioMixer.FindMatchingGroups(AudioMixerGroupName.EFFECTS)[0];

            _shotAudioClip = _audioModel.AudioStruct.ClipByTypes
                .FirstOrDefault(clip => clip._type == AudioClipType.Shot)?
                .AudioClips.FirstOrDefault();
            _explosionAudioClip = _audioModel.AudioStruct.ClipByTypes
                .FirstOrDefault(clip => clip._type == AudioClipType.Explosion)?
                .AudioClips.FirstOrDefault();

            _soundsListStartShip = _audioModel.AudioStruct.ClipByTypes[4].AudioClips ?? new List<AudioClip>();
            _musicList = _audioModel.AudioStruct.ClipByTypes.FirstOrDefault(c => c._type == AudioClipType.Music).AudioClips ?? new List<AudioClip>();

            _canvasController.StartGame += StartGame;
            _playerShooterController.IsShotInvoke += IsShotInvoke;
            _enemyController.IsAsteroidExplosion += ExplosionEnemy;

            _panelResults = _canvasModel.CanvasStruct.CanvasView.panelViews.FirstOrDefault(p => p is PanelResultsView panelResults) as PanelResultsView;
        }


        public void Initialization()
        {
            var sourceMusic = _audioModel.AudioStruct.PoolsByMixerTypes[MixerGroupByName.Music].Get();
            sourceMusic.clip = _musicList.FirstOrDefault();
            sourceMusic.gameObject.SetActive(true);
            sourceMusic.Play();

            sourceMusic.volume = 0.02f;

            _playerMusic = new PlayerMusic(new AudioSourceInfo(sourceMusic, sourceMusic.clip.length), _musicList);

            _panelResults.WebPlayer.ButtonPlay.onClick.AddListener(_playerMusic.PauseOrPlay);
            _panelResults.WebPlayer.ButtonPause.onClick.AddListener(_playerMusic.PauseOrPlay);
            _panelResults.WebPlayer.ButtonNext.onClick.AddListener(_playerMusic.Next);
            _panelResults.WebPlayer.ButtonPrev.onClick.AddListener(_playerMusic.Prev);

            _listButtons.Add(_panelResults.WebPlayer.ButtonPause);
            _listButtons.Add(_panelResults.WebPlayer.ButtonPlay);
            _listButtons.Add(_panelResults.WebPlayer.ButtonPrev);
            _listButtons.Add(_panelResults.WebPlayer.ButtonNext);
        }

       

        private void StartGame(EventCanvas eventType)
        {
            switch (eventType)
            {
                case EventCanvas.None:
                    break;
                case EventCanvas.StartGame:
                    var source = _audioModel.AudioStruct.AudioSourcePoolEffects.Get();
                    source.clip = _soundsListStartShip[1];
                    source.transform.localPosition = _playerModel.Components.PlayerTransform.localPosition;
                    source.gameObject.SetActive(true);
                    source.Play();
                    audioSourcesInUse.Add(new AudioSourceInfo(source, source.clip.length));
                    break;
                case EventCanvas.StartShip:
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

            foreach (var button in _listButtons)
            {
                button.onClick.RemoveAllListeners();
            }
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
            _playerMusic.TimeLeft(fixedDeltatime);
        }
    }
}