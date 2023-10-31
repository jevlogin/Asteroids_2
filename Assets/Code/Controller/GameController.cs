using System;
using System.Collections;
using System.IO;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class GameController : MonoBehaviour, ICleanup
    {
        [SerializeField] private Data _data;
        private Controllers _controllers;

        private void Start()
        {
            Camera camera = Camera.main;
            _controllers = new Controllers();

            var sceneFactory = new SceneFactory(_data.SceneData);
            var sceneInitialization = new SceneInitialization(sceneFactory);
            var sceneController = new SceneController(sceneInitialization.SceneModel);
            _controllers.Add(sceneController);

            var canvasFactory = new CanvasFactory(_data.CanvasData);
            var canvasInitialization = new CanvasInitialization(canvasFactory);
            var canvasController = new CanvasController(canvasInitialization.CanvasModel);
            sceneController.Add(canvasInitialization.CanvasModel);
            _controllers.Add(sceneController);

            var playerFactory = new PlayerFactory(_data.PlayerData);
            var playerInitialization = new PlayerInitialization(playerFactory,
                sceneInitialization.SceneModel.SceneStruct.StartSceneView.StartSpaceTransform);

            var inputInitialization = new InputInitialization();
            _controllers.Add(new InputController(inputInitialization));

            var ammunitionFactory = new AmmunitionFactory(_data.AmmunitionData);
            var ammunitionInitialization = new AmmunitionInitialization(ammunitionFactory);

            var enemyFactory = new EnemyFactory(_data.EnemyData);
            var enemyInitialization = new EnemyInitialization(enemyFactory);

            var playerController = new PlayerController(inputInitialization, playerInitialization, camera, sceneController);
            sceneController.Add(playerController);
            _controllers.Add(playerController);

            var cameraController = new CameraController(camera.GetComponent<CameraView>(),
                playerInitialization.PlayerModel.Components.PlayerTransform, sceneController);

            sceneController.Add(cameraController);

            _controllers.Add(cameraController);

            var playerShooterController = new PlayerShooterController(inputInitialization.GetInputMouse(),
                playerInitialization, ammunitionInitialization.AmmunitionFactoryModel, sceneController);

            _controllers.Add(playerShooterController);

            var enemyController = new EnemyController(enemyInitialization.Model, sceneController);
            _controllers.Add(enemyController);

            var particleController = new ParticleController(playerInitialization.PlayerModel, sceneController);
            _controllers.Add(particleController);

            _controllers.Initialization();
        }

        private void Update()
        {
            _controllers.Execute(Time.deltaTime);
        }

        private void LateUpdate()
        {
            _controllers.LateExecute(Time.deltaTime);
        }

        public void Cleanup()
        {
            _controllers.Cleanup();
        }
    }
}