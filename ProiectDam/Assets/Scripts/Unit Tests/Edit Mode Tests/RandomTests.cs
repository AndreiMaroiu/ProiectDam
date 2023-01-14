using Core;
using NUnit.Framework;

public class RandomTests
{
    private class TileData
    {
        public int Weight { get; set; }
        public RoomType Flags { get; set; }
    }

    [Test]
    public void WeightedRandomSingleElementTest()
    {
        TileData[] elems =
        {
            new()
            {
                Weight = 0,
                Flags = RoomType.Start
            },
            new()
            {
                Weight = 0,
                Flags = RoomType.Start
            },
            new()
            {
                Weight = 1,
                Flags = RoomType.Merchant
            },
            new()
            {
                Weight = 0,
                Flags = RoomType.Start
            },
        };

        WeightedRandom<TileData> weightedRandom = new(elems, (i, elem) => elem.Weight);

        for (int i = 0; i < 1000; i++)
        {
            TileData data = weightedRandom.Take();
            Assert.IsTrue(data.Flags == RoomType.Merchant);
        }
    }

    [Test]
    public void WeightedRandomTest()
    {
        TileData[] elems =
        {
            new()
            {
                Weight = 1,
                Flags = RoomType.Start
            },
            new()
            {
                Weight = 1,
                Flags = RoomType.Start
            },
            new()
            {
                Weight = 0,
                Flags = RoomType.Merchant
            },
            new()
            {
                Weight = 1,
                Flags = RoomType.Start
            },
        };

        WeightedRandom<TileData> weightedRandom = new(elems, (i, elem) => elem.Weight);

        for (int i = 0; i < 1000; i++)
        {
            TileData data = weightedRandom.Take();
            Assert.IsTrue(data.Flags == RoomType.Start);
        }
    }

    [Test]
    public void WeightedRandomTestSameWeight()
    {
        TileData[] elems =
        {
            new()
            {
                Weight = 1,
                Flags = RoomType.Start
            },
            new()
            {
                Weight = 1,
                Flags = RoomType.Start
            },
            new()
            {
                Weight = 1,
                Flags = RoomType.Merchant
            },
        };

        WeightedRandom<TileData> weightedRandom = new(elems, GetTileChance);

        for (int i = 0; i < 1000; i++)
        {
            TileData data = weightedRandom.Take();
            Assert.IsTrue(data.Flags is RoomType.Merchant);
        }

        static int GetTileChance(int i, TileData elem)
        {
            if (elem.Flags.FastHasFlag(RoomType.Merchant))
            {
                return elem.Weight;
            }

            return 0;
        }
    }
}
