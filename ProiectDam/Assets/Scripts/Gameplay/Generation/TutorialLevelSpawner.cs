using Core;
using UnityEngine;

namespace Gameplay.Generation
{
    public class TutorialLevelSpawner : BaseLevelSpawner
    {
        [SerializeField] private RoomType[] _rooms;

        public override void Spawn()
        {
            Room start = new TutorialLevelGenerator().Generate(_rooms);
            _traverser = new RoomTraverser<RoomBehaviour>(start);

            _data.DifficultyMultiplier = 1;

            SpawnRoomAssets();
            GenereteLayers();
            SetDoorPositions();
            SpawnLayers();
            SpawnDoors();
            ScanRooms();
            SpawnPlayer();

            _data.RoomBehaviourEvent.Value = _traverser.Start;
        }
    }
}
