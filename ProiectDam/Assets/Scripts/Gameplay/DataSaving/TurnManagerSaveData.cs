namespace Gameplay.DataSaving
{
    [System.Serializable]
    public class TurnManagerSaveData : ObjectSaveData
    {
        public bool WasRoomChanged { get; set; }
    }
}
