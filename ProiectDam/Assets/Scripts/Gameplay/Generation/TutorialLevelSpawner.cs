using Core;

namespace Gameplay.Generation
{
    public class TutorialLevelSpawner : BaseLevelSpawner
    {
        public override void Spawn()
        {
            Room start = new TutorialLevelGenerator().Generate(RoomType.Start, RoomType.Normal,
                RoomType.Normal, RoomType.End);
            _traverser = new RoomTraverser<RoomBehaviour>(start);

            SpawnRoomAssets();
            GenereteLayers();
            SetDoorPositions();
            SpawnLayers();
            SpawnDoors();
            ScanRooms();

            _data.RoomBehaviourEvent.Value = _traverser.Start;
        }
    }
}
