using static Core.RoomType;

namespace Core
{
    [System.Flags]
    public enum RoomType
    {
        Empty = 0,
        Normal = 1 << 0,
        Start = 1 << 1,
        End = 1 << 2,
        Healing = 1 << 3,
        Chest = 1 << 4,
        Wall = 1 << 5,
        Merchant = 1 << 6,
        Boss = 1 << 7
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
            Boss => nameof(Boss),
            _ => default,
        };

        public static bool FastHasFlag(this RoomType current, RoomType flag)
            => (current & flag) == flag;
    }
}
