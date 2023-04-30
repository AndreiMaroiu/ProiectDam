using Core.Values;
using Gameplay.Generation;
using UnityEngine;
using Utilities;

namespace Gameplay.Enemies
{
    public class BossMinionsSpawner : MonoBehaviour
    {
        [SerializeField] private BossEnemy _bossBehaviour;
        [SerializeField] private TileSettings _tiles;
        [SerializeField] private Range<int> _enemyCountRange;

        private void Start()
        {
            _bossBehaviour.OnPhaseChange += OnPhaseChange;
        }

        public void OnPhaseChange(BossEnemy.BossPhase bossPhase)
        {
            if (bossPhase is BossEnemy.BossPhase.Two)
            {
                int enemyCount = Random.Range(_enemyCountRange.start, _enemyCountRange.end + 1);
                int enemiesKilled = 0;
                LayerBehaviour layerBehaviour = _bossBehaviour.GetComponentInParent<LayerBehaviour>();
                for (int i = 0; i < enemyCount; i++)
                {
                    GameObject tile = _tiles.GetTile(TileType.Enemy, Core.RoomType.Normal).Prefab;
                    GameObject clone = Instantiate(tile, transform.parent);
                    BaseEnemy enemy = clone.GetComponent<BaseEnemy>();

                    SetPosition(enemy);
                    layerBehaviour.AddEnemy(enemy);

                    enemy.OnDeathEvent += _ =>
                    {
                        enemiesKilled++;

                        if (enemiesKilled == enemyCount)
                        {
                            _bossBehaviour.AdvancePhase();
                        }
                    };
                }
            }
        }

        private void SetPosition(TileObject enemy)
        {
            if (enemy is null)
            {
                return;
            }

            int x;
            int y;

            Vector2Int target;
            var layerPosition = _bossBehaviour.LayerPosition;

            do
            {
                x = Random.Range(2, 5) * (int)Mathf.Pow(-1, Random.Range(0, 2));
                y = Random.Range(2, 5) * (int)Mathf.Pow(-1, Random.Range(0, 2));
                target = layerPosition.Position + new Vector2Int(x, y);
            } while (IsInBounds(target) is false || layerPosition.Layer[target.x, target.y] is not TileType.None);

            Vector2Int direction = new(x, y);

            Vector3 movePos = transform.position + Utils.GetVector3FromMatrixPos(direction, _bossBehaviour.CellSize);
            enemy.LayerPosition = _bossBehaviour.LayerPosition.Clone();
            enemy.transform.position = movePos;
            enemy.LayerPosition.TileType = TileType.None;
            enemy.LayerPosition.Move(direction);
            enemy.LayerPosition.TileType = TileType.Enemy;
        }

        private bool IsInBounds(Vector2Int target)
        {
            var layerPosition = _bossBehaviour.LayerPosition;

            return target.x >= 0 && target.x < layerPosition.Layer.GetLength(0) &&
                    target.y >= 0 && target.y < layerPosition.Layer.GetLength(0);
        }
    }
}
