namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class InputController : IExecute
    {
        #region Fields

        private readonly IUserInputProxy _horizontal;
        private readonly IUserInputProxy _vertical;
        private readonly IUserInputBool _inputMouse;

        #endregion


        #region ClassLifeCycles

        public InputController(InputInitialization inputInitialization)
        {
            _horizontal = inputInitialization.GetInput().inputHorizontal;
            _vertical = inputInitialization.GetInput().inputVertical;
            _inputMouse = inputInitialization.GetInputMouse();
        }

        #endregion


        #region IExecute
        
        public void Execute(float deltatime)
        {
            _horizontal.GetAxis();
            _vertical.GetAxis();
            _inputMouse.GetButtonDown();
        } 

        #endregion
    }
}