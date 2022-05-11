using static Core.RoomType;

namespace Core
{
    public enum RoomType
    {
        Empty,
        Normal,
        Start,
        End,
        Healing,
        Chest,
        Wall,
        Merchant
    }

    public static class RoomTypeExtension
    {
        public static string FastToString(this RoomType type) => type switch
        {
            Empty => nameof(Empty),
            Normal => nameof(Normal),
            Start => nameof(Start),
            End => nameof(End),
            Healing => nameof(Healing),
            Chest => nameof(Chest),
            Wall => nameof(Wall),
            Merchant => nameof(Merchant),
            _ => default,
        };
    }
}
