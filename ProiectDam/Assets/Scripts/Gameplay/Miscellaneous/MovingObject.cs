using Gameplay.Generation;
using System.Collections;
using UnityEngine;
using Utilities;

namespace Gameplay
{
    public abstract class MovingObject : KillableObject
    {
        public bool CanMove { get; private set; } = true;
        public bool IsMoving { get; private set; } = false;
        public float MoveTime { get; private set; }

        private TileType _tileType;
        private TileType _lastType;
        private float _cellSize;
        private float _inverseMoveTime;

        protected abstract void OnMove(Vector2Int direction);
        protected abstract void OnStopMoving();
        protected abstract bool CanMoveToTile(TileType tile);

        public void StopMoving()
        {
            CanMove = true;
            IsMoving = false;
        }

        protected void SetMove(float moveTime, float cellSize, TileType tileType)
        {
            MoveTime = moveTime;
            _inverseMoveTime = 1 / moveTime;
            _cellSize = cellSize;
            _tileType = tileType;
        }

        protected IEnumerator TryMove(Vector2Int direction)
        {
            TileType[,] lastLayer = LayerPosition.Layer;
            Vector2Int lastPos = LayerPosition.Position;
            Vector2Int layerDirection = Utils.GetMatrixPos(direction);

            if (!CanMoveToTile(LayerPosition.GetTile(layerDirection)))
            {
                yield break;
            }

            Vector3 endPosition = transform.position + (new Vector3(direction.x, direction.y) * _cellSize);
            CanMove = false;
            IsMoving = true;
            OnMove(direction);

            while (transform.position != endPosition && !CanMove && !IsDead)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPosition, _inverseMoveTime * Time.deltaTime);
                yield return null;
            }

            if (LayerPosition.Layer == lastLayer)
            {
                LayerPosition.Layer[lastPos.x, lastPos.y] = _lastType;
            }

            if (transform.position == endPosition)
            {
                _lastType = LayerPosition.GetTile(layerDirection);
                LayerPosition.Move(layerDirection);
            }
            
            LayerPosition.TileType = _tileType;

            StopMoving();
            OnStopMoving();
        }
    }
}
