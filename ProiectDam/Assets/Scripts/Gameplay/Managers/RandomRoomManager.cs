using Gameplay.DataSaving;
using Gameplay.Generation;
using UnityEngine;

namespace Gameplay.Managers
{
    public class RandomRoomManager : RoomManager
    {
        [Header("Game objects")]
        [SerializeField] private RandomLevelSaverManager _levelSaver;

        protected override void SetPlayerPos(RoomBehaviour behaviour)
        {
            if (_levelSaver.ShouldLoad)
            {
                int layerIndex = _levelSaver.SaveData.PlayerData.LayerPos.Biome;
                TileType[,] layer = behaviour.Layers[layerIndex];
                var pos = _levelSaver.SaveData.PlayerData.LayerPos.Position;
                layer[pos.X, pos.Y] = TileType.Player; // todo: this may not be needed in future
                _player.LayerPosition = new(pos, layer);
                _player.transform.position = _levelSaver.SaveData.PlayerData.PlayerPos;

                _layerEvent.CurrentLayer.Value = layerIndex;
                behaviour.ChangedLayer(layerIndex);
                _layerEvent.CurrentBiome.Value = _roomBehaviourEvent.Value.Layers.GetBiome(layerIndex);
            }
            else
            {
                base.SetPlayerPos(behaviour);
            }
        }

        protected override RoomBehaviour GetActiveBehaviour()
        {
            if (_levelSaver.ShouldLoad)
            {
                RoomBehaviour roomBehaviour = _spawner.Traverser[_levelSaver.SaveData.CurrentRoom];
                _roomBehaviourEvent.Value = roomBehaviour;
                return roomBehaviour;
            }

            return base.GetActiveBehaviour();
        }
    }
}
