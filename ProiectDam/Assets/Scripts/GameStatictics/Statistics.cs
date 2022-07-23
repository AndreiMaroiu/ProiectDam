using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameStatistics
{
    [Serializable]
    public class Statistics
    {
 	    public int TotalRuns { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Highscore { get; set; }

        public void AddWin()
        {
            Wins++;
            TotalRuns++;
        }

        public void AddLoss()
        {
            Losses++;
            TotalRuns++;
        }
    }
}
