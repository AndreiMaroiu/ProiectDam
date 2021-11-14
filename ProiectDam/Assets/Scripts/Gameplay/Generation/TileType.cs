namespace Gameplay.Generation
{
    public enum TileType
    {
        None,
        Wall,
        Grass,
        Enemy,
        Door,
        Chest,
        Heal,
        Trap,
        PickUp,
        Obstacle,
        Portal
    }

    public static class TileTypeExtension
    {
        public static bool CanMove(this TileType tile)
        {
            switch (tile)
            {
                case TileType.None:
                case TileType.Grass:
                case TileType.Door:
                case TileType.Heal:
                case TileType.Trap:
                case TileType.PickUp:
                case TileType.Portal:
                    return true;
                default:
                    return false;
            }
        }
    }
}
