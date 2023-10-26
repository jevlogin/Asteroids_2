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

            var playerFactory = new PlayerFactory(_data.PlayerData);
            var playerInitialization = new PlayerInitialization(playerFactory);

            var inputInitialization = new InputInitialization();
            _controllers.Add(new InputController(inputInitialization));

            var ammunitionFactory = new AmmunitionFactory(_data.AmmunitionData);
            var ammunitionInitialization = new AmmunitionInitialization(ammunitionFactory);

            var enemyFactory = new EnemyFactory(_data.EnemyData);
            var enemyInitialization = new EnemyInitialization(enemyFactory);

            _controllers.Add(new PlayerController(inputInitialization, playerInitialization, camera));

            _controllers.Add(new CameraController(camera.GetComponent<CameraView>(), playerInitialization.PlayerModel.Components.PlayerTransform));

            _controllers.Add(new PlayerShooterController(inputInitialization.GetInputMouse(), playerInitialization, ammunitionInitialization.AmmunitionFactoryModel));

            _controllers.Add(new EnemyController(enemyInitialization.Model));

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