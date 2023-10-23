namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class InputInitialization
    {
        #region Fields

        private IUserInputProxy _inputHorizontal;
        private IUserInputProxy _inputVertical;
        private IUserInputBool _inputMouse;

        #endregion


        #region ClassLifeCycles
        
        public InputInitialization()
        {
            _inputHorizontal = new PCInputHorizontal();
            _inputVertical = new PCInputVertical();
            _inputMouse = new PCInputMouse();
        } 

        #endregion


        #region Methods

        public (IUserInputProxy inputHorizontal, IUserInputProxy inputVertical) GetInput()
        {
            (IUserInputProxy inputHorizontal, IUserInputProxy inputVertical) result = (_inputHorizontal, _inputVertical);
            return result;
        }

        public IUserInputBool GetInputMouse()
        {
            return _inputMouse;
        }

        #endregion
    }
}