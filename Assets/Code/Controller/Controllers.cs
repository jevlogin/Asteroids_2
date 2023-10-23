using System.Collections.Generic;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class Controllers : IInitialization, IExecute, IFixedExecute, ILateExecute, ICleanup
    {
        #region Fields

        private readonly List<IInitialization> _initializeControllers;
        private readonly List<IExecute> _executeControllers;
        private readonly List<IFixedExecute> _fixedControllers;
        private readonly List<ILateExecute> _lateControllers;
        private readonly List<ICleanup> _cleanupControllers;

        #endregion


        #region Properties

        public Controllers()
        {
            _initializeControllers = new List<IInitialization>();
            _executeControllers = new List<IExecute>();
            _fixedControllers = new List<IFixedExecute>();
            _lateControllers = new List<ILateExecute>();
            _cleanupControllers = new List<ICleanup>();
        }

        #endregion


        #region Methods

        internal Controllers Add(IController controller)
        {
            if (controller is IInitialization initializationController)
            {
                _initializeControllers.Add(initializationController);
            }

            if (controller is IExecute executeController)
            {
                _executeControllers.Add(executeController);
            }

            if (controller is IFixedExecute fixedController)
            {
                _fixedControllers.Add(fixedController);
            }

            if (controller is ILateExecute lateUpdateController)
            {
                _lateControllers.Add(lateUpdateController);
            }

            if (controller is ICleanup cleanupController)
            {
                _cleanupControllers.Add(cleanupController);
            }

            return this;
        }

        #endregion


        #region Interfaces

        public void Cleanup()
        {
            for (int i = 0; i < _cleanupControllers.Count; i++)
            {
                _cleanupControllers[i].Cleanup();
            }
        }

        public void Execute(float deltatime)
        {
            for (int i = 0; i < _executeControllers.Count; i++)
            {
                _executeControllers[i].Execute(deltatime);
            }
        }

        public void FixedExecute(float fixedDeltatime)
        {
            for (int i = 0; i < _fixedControllers.Count; i++)
            {
                _fixedControllers[i].FixedExecute(fixedDeltatime);
            }
        }

        public void Initialization()
        {
            for (int i = 0; i < _initializeControllers.Count; i++)
            {
                _initializeControllers[i].Initialization();
            }
        }

        public void LateExecute(float deltatime)
        {
            for (int i = 0; i < _lateControllers.Count; i++)
            {
                _lateControllers[i].LateExecute(deltatime);
            }
        } 

        #endregion
    }
}