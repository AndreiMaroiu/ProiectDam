namespace Gameplay.Generation
{
    public enum TileType
    {
        None,
        Wall,
        Enemy,
        Door,
        Chest,
        Heal,
        Trap,
        PickUp,
        Obstacle,
        Portal,
        Player,
        Merchant
    }

    public static class TileTypeExtension
    {
        public static bool CanMovePlayer(this TileType tile)
        {
            switch (tile)
            {
                case TileType.None:
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

        public static bool CanMove(this TileType tile) => tile switch
        {
            TileType.None => true,
            TileType.Trap => true,
            _ => false,
        };
    }
}
