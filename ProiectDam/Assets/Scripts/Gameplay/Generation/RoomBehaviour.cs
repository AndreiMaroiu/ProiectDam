using Core;
using UnityEngine;

namespace Gameplay.Generation
{
    /// <summary>
    /// Behavior that handles both layers and doors logic
    /// </summary>
    public sealed class RoomBehaviour : MonoBehaviour
    {
        [SerializeField] private int _a;

        private LayerBehaviour[] _layersObjects;

        private Room _room;
        private Layers _layers;
        private int _currentLayer;

        public Room Room => _room;
        public Layers Layers => _layers;
        public int CurrentLayer => _currentLayer;
        public LayerBehaviour ActiveLayerBehaviour => _layersObjects[_currentLayer];
        public TileType[,] ActiveLayer => Layers.GetTiles(_currentLayer);
        public LayerBehaviour[] LayerBehaviours => _layersObjects;

        private LayerBehaviour CreateEmptyObject(string name)
        {
            GameObject result = new(name);
            LayerBehaviour behaviour = result.AddComponent<LayerBehaviour>();

            result.transform.parent = transform;
            result.transform.localPosition = Vector3.zero;

            return behaviour;
        }

        public void Set(Room room, Layers layers)
        {
            _room = room;
            _layers = layers;
            _layersObjects = new LayerBehaviour[layers.Count];

            for (int i = 0; i < layers.Count; i++)
            {
                _layersObjects[i] = CreateEmptyObject($"Layer {i.ToString()}");
                _layersObjects[i].gameObject.SetActive(false);
            }

            _currentLayer = _layers.Count / 2;
            _layersObjects[_currentLayer].gameObject.SetActive(true);
        }

        public void ChangedLayer(int layer)
        {
            if (layer >= _layers.Count)
            {
                return;
            }

            _layersObjects[_currentLayer].gameObject.SetActive(false);
            _currentLayer = layer;
            _layersObjects[layer].gameObject.SetActive(true);
        }

        public Transform GetTransform(int index)
            => _layersObjects[index].transform;

        public void Scan()
        {
            foreach (var layer in _layersObjects)
            {
                layer.ScanForDoors();
            }
        }

        public void UpdateDoors(bool isLocked)
        {
            foreach (var layer in _layersObjects)
            {
                layer.UpdateDoors(isLocked);
            }
        }
    }
}
