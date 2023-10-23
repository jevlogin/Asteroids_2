namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class PlayerModel
    {
        public PlayerStruct PlayerStruct;
        public PlayerComponents Components;
        public PlayerSettings Settings;

        public PlayerModel(PlayerStruct playerStruct, PlayerComponents components, PlayerSettings settings)
        {
            PlayerStruct = playerStruct;
            Components = components;
            Settings = settings;
        }
    }
}