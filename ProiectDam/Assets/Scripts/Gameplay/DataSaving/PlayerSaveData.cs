﻿namespace Gameplay.DataSaving
{
    [System.Serializable]
    public class PlayerSaveData
    {
        public Vector3Pos PlayerPos { get; set; }
        public LayerPositionData LayerPos { get; set; }
        public bool IsFliped { get; set; }

        public int Health { get; set; }
        public int Energy { get; set; }
        public int Bullets { get; set; }
        public int Coins { get; set; }
        public int Score { get; set; }
    }
}