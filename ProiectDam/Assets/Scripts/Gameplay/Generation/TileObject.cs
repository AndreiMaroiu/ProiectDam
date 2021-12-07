using UnityEngine;

namespace Gameplay.Generation
{
    public class TileObject : MonoBehaviour
    {
        private LayerPosition _layerPostion;

        public LayerPosition LayerPosition
        {
            get => _layerPostion;

            set
            {
                _layerPostion?.Clear();
                _layerPostion = value;
            }
        }
    }
}
