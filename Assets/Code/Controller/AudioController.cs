using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
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
        private readonly SceneController _sceneController;
        private AudioModel _audioModel;
        private AudioClip _shotAudioClip;
        private readonly AudioClip _explosionAudioClip;
        private readonly List<AudioClip> _soundsListStartShip;
        private readonly List<AudioClip> _musicList;

        private readonly PanelResultsView _panelResults;


        private List<AudioSourceInfo> audioSourcesInUse = new List<AudioSourceInfo>();
        private PlayerMusic _playerMusic;
        private List<Button> _listButtons = new();
        private bool _isStopControl;

        public AudioController(AudioModel audioModel, PlayerModel playerModel,
            PlayerShooterController playerShooterController, EnemyController enemyController,
            CanvasController canvasController, CanvasModel canvasModel, SceneController sceneController)
        {
            _audioModel = audioModel;
            _playerModel = playerModel;
            _playerShooterController = playerShooterController;
            _enemyController = enemyController;
            _canvasController = canvasController;
            _canvasModel = canvasModel;
            _sceneController = sceneController;
            _sceneController.IsStopControl += IsStopControl;
            _sceneController.IsCanPlayBackgroundMusic += IsCanPlayBackgroundMusic;

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
            _enemyController.IsAsteroidExplosionByType += PlayExplosionEnemy;

            _panelResults = _canvasModel.CanvasStruct.CanvasView.panelViews.FirstOrDefault(p => p is PanelResultsView panelResults) as PanelResultsView;
        }

        private void IsCanPlayBackgroundMusic(bool value)
        {
            if (_playerMusic.IsStopedMusic)
                return;

            if (value)
            {
                _playerMusic?.Play();
            }
            else
            {
                _playerMusic?.Pause();
            }
        }

        private void IsStopControl(bool value)
        {
            _isStopControl = value;
        }

        private void PlayExplosionEnemy(Vector3 vector, EnemyType type)
        {
            var source = _audioModel.AudioStruct.AudioSourcePoolEffects.Get();
            source.transform.localPosition = vector;
            source.gameObject.SetActive(true);
            source.pitch = Random.Range(_audioModel.AudioStruct.MinRandPitch, _audioModel.AudioStruct.MaxRandPitch);
            source.clip = _explosionAudioClip;
            source.Play();

            audioSourcesInUse.Add(new AudioSourceInfo(source, source.clip.length));
        }

        public void Initialization()
        {
            var sourceMusic = _audioModel.AudioStruct.PoolsByMixerTypes[MixerGroupByName.Music].Get();
            sourceMusic.clip = _musicList[0];
            sourceMusic.loop = false;
            sourceMusic.playOnAwake = false;

            _playerMusic = new PlayerMusic(new AudioSourceInfo(sourceMusic, sourceMusic.clip.length), _musicList);

            _panelResults.WebPlayer.ButtonPlay.onClick.AddListener(_playerMusic.Play);
            _panelResults.WebPlayer.ButtonPlay.onClick.AddListener(_playerMusic.PlayMusic);

            _panelResults.WebPlayer.ButtonPause.onClick.AddListener(_playerMusic.Pause);
            _panelResults.WebPlayer.ButtonPause.onClick.AddListener(_playerMusic.StopMusic);
            
            _panelResults.WebPlayer.ButtonNext.onClick.AddListener(_playerMusic.NextTrack);
            _panelResults.WebPlayer.ButtonPrev.onClick.AddListener(_playerMusic.PreviousTrack);

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



        public void Cleanup()
        {
            _sceneController.IsStopControl -= IsStopControl;
            _sceneController.IsCanPlayBackgroundMusic -= IsCanPlayBackgroundMusic;

            _playerShooterController.IsShotInvoke -= IsShotInvoke;
            _enemyController.IsAsteroidExplosionByType -= PlayExplosionEnemy;

            _canvasController.StartGame -= StartGame;

            foreach (var button in _listButtons)
            {
                button.onClick.RemoveAllListeners();
            }
        }

        private void IsShotInvoke(bool value)
        {
            if (_playerModel.PlayerStruct.Player.gameObject.activeSelf && value)
            {
                _playerModel.Components.AudioSource.PlayOneShot(_shotAudioClip);
            }
        }

        public void FixedExecute(float fixedDeltatime)
        {
            for (int i = audioSourcesInUse.Count - 1; i >= 0; i--)
            {
                var source = audioSourcesInUse[i];
                source.Delay -= fixedDeltatime;
                if (source.Delay <= 0)
                {
                    _audioModel.AudioStruct.AudioSourcePoolEffects.ReturnToPool(source.Source);
                    audioSourcesInUse.Remove(source);
                }
            }

            _playerMusic.TimeLeft(fixedDeltatime);
        }
    }
}