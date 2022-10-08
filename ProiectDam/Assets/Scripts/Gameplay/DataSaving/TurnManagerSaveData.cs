namespace Gameplay.DataSaving
{
    [System.Serializable]
    public sealed class TurnManagerSaveData : ObjectSaveData
    {
        public bool WasRoomChanged { get; set; }
    }
}
