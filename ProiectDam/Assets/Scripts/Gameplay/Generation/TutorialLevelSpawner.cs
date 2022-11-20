using Core;
using Core.Values;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Generation
{
    public class TutorialLevelSpawner : BaseLevelSpawner
    {

        public override void Spawn()
        {
            Room start = new TutorialLevelGenerator().Generate(RoomType.Start, RoomType.Normal, RoomType.End);
            _traverser = new RoomTraverser<RoomBehaviour>(start);

            SpawnRoomAssets();
            GenereteLayers();
            SetDoorPositions();
            SpawnLayers();
            SpawnDoors();
            ScanRooms();

            _data.RoomBehaviourEvent.Value = _traverser.Start;
        }

        //private new void SpawnRoomAssets()
        //{
        //    _traverser.Traverse(room =>
        //    {
        //        SpawnRoom(room, new Vector3(room.Pos.x * _data.Distance, room.Pos.y * _data.Distance));
        //    });
        //}

        //private new void SetDoorPositions()
        //{
        //    _traverser.Traverse(room =>
        //    {
        //        if (room.LastRoom is null)
        //        {
        //            return;
        //        }

        //        RoomBehaviour current = _traverser[room.Pos];
        //        RoomBehaviour previous = _traverser[room.LastRoom.Pos];

        //        SetDoorPosition(room.LastRoom.Pos - room.Pos, current.Layers.Middle, TileType.Door);
        //        SetDoorPosition(room.Pos - room.LastRoom.Pos, previous.Layers.Middle, TileType.Door);
        //    });
        //}
    }
}
