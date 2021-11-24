using Gameplay.Generation;
using System.Collections;
using UnityEngine;
using Utilities;

namespace Gameplay
{
    public abstract class MovingObject : KillableObject
    {
        public LayerPosition LayerPosition { get; set; }
        public bool CanMove { get; private set; } = true;

        private TileType _tileType;
        private float _cellSize;
        private float _inverseMoveTime;

        protected abstract void OnMove(Vector2Int direction);
        protected abstract void OnStopMoving();

        public void StopMoving()
        {
            CanMove = true;
        }

        protected void SetMove(float moveTime, float cellSize, TileType tileType)
        {
            _inverseMoveTime = 1 / moveTime;
            _cellSize = cellSize;
            _tileType = tileType;
        }

        protected IEnumerator TryMove(Vector2Int direction)
        {
            Vector2Int lastPos = LayerPosition.Position;
            Vector2Int layerdirectionection = Utils.GetMatrixPos(direction);
            if (!LayerPosition.CanMove(layerdirectionection))
            {
                yield break;
            }

            LayerPosition.Move(layerdirectionection);

            Vector3 endPosition = transform.position + (new Vector3(direction.x, direction.y) * _cellSize);
            CanMove = false;
            OnMove(direction);

            while (transform.position != endPosition && !CanMove)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPosition, _inverseMoveTime * Time.deltaTime);
                yield return null;
            }

            if (transform.position == endPosition)
            {
                Vector2Int currentPos = LayerPosition.Position;
                LayerPosition.Layer[lastPos.x, lastPos.y] = TileType.None;
                LayerPosition.Layer[currentPos.x, currentPos.y] = _tileType;
            }

            StopMoving();
            OnStopMoving();
        }
    }
}
