using Core;
using Gameplay.Generation;
using NUnit.Framework;
using UnityEngine;

namespace UnitTests.GameplayTests
{
    public class LayerGeneratorTest
    {
        public void TestLayerForBoss()
        {
            LayersGenerator generator = new(13, new Vector2Int());
            Room room = new(Vector2Int.zero, null)
            {
                Type = RoomType.Boss,
            };

            Layers layers = generator.Generate(room);

            foreach (Layers.LayerData layer in layers)
            {
                bool isEnemySpawned = false;
                int length = layer.Tiles.GetLength(0);

                for (int i = 0; i < length; i++)
                {
                    for (int j = 0; j < length; j++)
                    {
                        if (layer.Tiles[i, j] is TileType.Enemy)
                        {
                            isEnemySpawned = true;
                            goto outerFor;
                        }
                    }
                }

            outerFor:
                Assert.IsTrue(isEnemySpawned);
            }
        }

        [Test]
        public void Repeted()
        {
            for (int i = 0; i < 1000; i++)
            {
                TestLayerForBoss();
            }
        }
    }
}
